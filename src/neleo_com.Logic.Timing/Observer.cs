using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

using LogicModule.Nodes.Helpers;
using LogicModule.ObjectModel;
using LogicModule.ObjectModel.TypeSystem;

namespace neleo_com.Logic.Timing {

    public class Observer : LogicNodeBase {

        /// <summary>
        ///   The Type Service manages incoming and outgoing ports.</summary>
        private readonly ITypeService TypeService;

        /// <summary>
        ///   The Scheduler Service manages access to the clock and scheduled callbacks.</summary>
        private readonly ISchedulerService SchedulerService;

        /// <summary>
        ///   Resets the current observation.</summary>
        [Input(IsRequired = true, IsInput = true, DisplayOrder = 1)]
        public BoolValueObject ResetTrigger {
            get; private set;
        }

        /// <summary>
        ///   Time to wait after <see cref="ResetTrigger"/> was fired before the <see cref="ObservationTimeSpans"/> will be evaluated.</summary>
        [Parameter(IsDefaultShown = false, DisplayOrder = 2)]
        public TimeSpanValueObject ResetLagTime {
            get; private set;
        }

        /// <summary>
        ///   Number of observation timespans and corresponding triggers.</summary>
        [Parameter(IsDefaultShown = false, DisplayOrder = 3)]
        public IntValueObject ObservationCount {
            get; private set;
        }

        /// <summary>
        ///   Defines multiple timeouts. The corresponding <see cref="ObservationTriggers"/> will fire 
        ///   if there was no positive incoming <see cref="InputTrigger"/> (1) during the given timespan.</summary>
        [Parameter(IsDefaultShown = true, DisplayOrder = 4)]
        public IList<TimeSpanValueObject> ObservationTimeSpans {
            get; private set;
        }

        /// <summary>
        ///   Recognizes incoming "1" telegrams and triggeres stops the observation.</summary>
        [Input(IsRequired = true, IsInput = true, DisplayOrder = 5)]
        public BoolValueObject InputTrigger {
            get; private set;
        }

        /// <summary>
        ///   Indicator whether an opbservation is active (starts with <see cref="ResetTrigger"/> 
        ///   and finishes with last <see cref="ObservationTimeSpans"/>/<see cref="ObservationTriggers"/>.</summary>
        [Output(IsDefaultShown = true, DisplayOrder = 1)]
        public BoolValueObject ObservationActive {
            get; private set;
        }

        /// <summary>
        ///   A set of corresponding notification triggers for <see cref="ObservationTimeSpans"/>.</summary>
        [Output(IsDefaultShown = true, DisplayOrder = 2)]
        public IList<BoolValueObject> ObservationTriggers {
            get; private set;
        }

        /// <summary>
        ///   Fired if there was a positive <see cref="InputTrigger"/> after <see cref="ResetLagTime"/> 
        ///   but before the first <see cref="ObservationTimeSpans"/> occur.</summary>
        [Output(IsDefaultShown = true, DisplayOrder = 3)]
        public BoolValueObject ActivityTrigger {
            get; private set;
        }

        /// <summary>
        ///   A token to manage the start of the observation.</summary>
        private SchedulerToken LeadWaitTimeWakeupToken {
            get; set;
        }

        /// <summary>
        ///   Points to the current observation step (-1 = lead wait time).</summary>
        private Int32 ObservationWakeupStep = -1;

        /// <summary>
        ///   A list of tokens to handle the observation steps.</summary>
        private IList<SchedulerToken> ObservationWakeupTokens {
            get; set;
        }

        /// <summary>
        ///   Constructor to setup the ports and services.</summary>
        /// <param name="context">
        ///   Context of the node instance to connect to services.</param>
        public Observer(INodeContext context) {

            // ensure context is set
            context.ThrowIfNull(nameof(context));

            // initializes services
            this.TypeService = context.GetService<ITypeService>();
            this.SchedulerService = context.GetService<ISchedulerService>();

            // initialize ports
            this.ResetTrigger = this.TypeService.CreateBool(PortTypes.Binary, nameof(this.ResetTrigger), false);
            this.ResetLagTime = this.TypeService.CreateTimeSpan(PortTypes.TimeSpan, nameof(this.ResetLagTime),
                new TimeSpan(0, 5, 0));
            this.ResetLagTime.MinValue = new TimeSpan(0, 0, 5);

            this.ObservationCount = this.TypeService.CreateInt(PortTypes.Integer, nameof(this.ObservationCount), 2);
            this.ObservationCount.MinValue = 1;
            this.ObservationCount.MaxValue = 15;

            this.ObservationTimeSpans = new List<TimeSpanValueObject>();
            ListHelpers.ConnectListToCounter(this.ObservationTimeSpans, this.ObservationCount,
                this.TypeService.GetValueObjectCreator(PortTypes.TimeSpan, nameof(this.ObservationTimeSpans)), null);

            this.InputTrigger = this.TypeService.CreateBool(PortTypes.Binary, nameof(this.InputTrigger), false);

            this.ObservationActive = this.TypeService.CreateBool(PortTypes.Binary, nameof(this.ObservationActive), false);

            this.ObservationTriggers = new List<BoolValueObject>();
            ListHelpers.ConnectListToCounter(this.ObservationTriggers, this.ObservationCount,
                this.TypeService.GetValueObjectCreator(PortTypes.Binary, nameof(this.ObservationTriggers), false), null);

            this.ActivityTrigger = this.TypeService.CreateBool(PortTypes.Binary, nameof(this.ActivityTrigger), false);

        }

        /// <summary>
        ///   Localizes all options and labels.</summary>
        /// <param name="language">
        ///   The language / culture.</param>
        /// <param name="key">
        ///   The key for the option or label.</param>
        /// <returns>
        ///   A localized key or label.</returns>
        public override String Localize(String language, String key) {

            // ensure that parameters are set
            language.ThrowIfNull(nameof(language));
            key.ThrowIfNull(nameof(key));

            // load culture
            CultureInfo culture;
            try {
                culture = CultureInfo.GetCultureInfo(language);
            }
            catch (CultureNotFoundException) {
                culture = CultureInfo.InvariantCulture;
            }

            // filter and map keys
            if (key.StartsWith(nameof(this.ObservationTimeSpans))) {

                String identifier = key.Substring(nameof(this.ObservationTimeSpans).Length);
                return String.Format(ResourceManager.GetString(nameof(this.ObservationTimeSpans), culture) ?? key, identifier);

            }
            else if (key.StartsWith(nameof(this.ObservationTriggers))) {

                String identifier = key.Substring(nameof(this.ObservationTriggers).Length);
                return String.Format(ResourceManager.GetString(nameof(this.ObservationTriggers), culture) ?? key, identifier);

            }
            else {

                return ResourceManager.GetString(key, culture) ?? key;

            }

        }

        /// <summary>
        ///   Configures the observer and reacts to trigger telegrams.</summary>
        public override void Execute() {

            if (this.ResetTrigger.WasSet && this.ResetTrigger.ValueEquals(true))
                this.Wait();

            // skip abortion during reset lag time (to allow sensors to go to default mode)
            else if (this.InputTrigger.WasSet && this.InputTrigger.ValueEquals(true) && this.ObservationWakeupStep != -1)
                this.Abort(true);

        }

        /// <summary>
        ///   Starts a new observation cycle by waiting for the lead time to end.</summary>
        private void Wait() {

            // remove existing wait and observer tokens
            this.Abort(false);

            // setup a new wait wakeup time and keep the token
            if (this.ResetLagTime.HasValue) {

                this.ObservationWakeupStep = -1;

                this.LeadWaitTimeWakeupToken = this.SchedulerService.InvokeIn(this.ResetLagTime.Value, this.SetupObservers);

            }

            // notify about ongoing obsevation
            this.UpdateValue(this.ObservationActive, true);

        }

        /// <summary>
        ///   Setup aggregated wakeup times for the observers. 
        ///   Caution: Don't call this method directly, it's triggered by <see cref="Wait"/>!</summary>
        private void SetupObservers() {

            // create observation steps
            this.ObservationWakeupStep = 0;
            this.ObservationWakeupTokens = new List<SchedulerToken>();
            TimeSpan observationTime = new TimeSpan(0, 0, 3);
            foreach (TimeSpanValueObject observationStep in this.ObservationTimeSpans) {

                observationTime = observationTime.Add(observationStep.HasValue ? observationStep.Value : TimeSpan.Zero);
                this.ObservationWakeupTokens.Add(this.SchedulerService.InvokeIn(observationTime, this.TriggerObserver));

            }

        }

        /// <summary>
        ///   Notifies an observer trigger that the time for this step is up.
        ///   Caution: Don't call this method directly, it's triggered by <see cref="SetupObservers"/>!</summary>
        private void TriggerObserver() {

            // trigger observation step
            this.ResetTriggers();
            if (this.ObservationWakeupStep >= 0 && this.ObservationWakeupStep < this.ObservationTriggers.Count())
                this.UpdateValue(this.ObservationTriggers[this.ObservationWakeupStep], true);

            // increment observation step
            this.ObservationWakeupStep += 1;

            // finish observation after last observation step
            if (this.ObservationWakeupStep >= this.ObservationTriggers.Count())
                this.UpdateValue(this.ObservationActive, false);

        }

        /// <summary>
        ///   Clears all exisiting observer steps and triggers the <see cref="ActivityTrigger"/> based on abortion type 
        ///   (<see cref="InputTrigger"/> = external/true, otherwise internal/false).</summary>
        /// <param name="externalTrigger">
        ///   <c>true</c> if the abortion was triggered externally, otherwise <c>false</c>.</param>
        private void Abort(Boolean externalTrigger) {

            // remove exisiting wait and observer tokens
            if (this.LeadWaitTimeWakeupToken != null)
                this.SchedulerService.Remove(this.LeadWaitTimeWakeupToken);

            this.LeadWaitTimeWakeupToken = null;

            if (this.ObservationWakeupTokens != null)
                foreach (SchedulerToken token in this.ObservationWakeupTokens)
                    this.SchedulerService.Remove(token);

            this.ObservationWakeupTokens = null;
            this.ObservationWakeupStep = -1;

            // trigger outputs (true, if trggered externally)
            this.ResetTriggers();
            this.UpdateValue(this.ActivityTrigger, externalTrigger);
            this.UpdateValue(this.ObservationActive, false);

        }

        /// <summary>
        ///   Set the activity and all observation triggers off (false).</summary>
        private void ResetTriggers() {

            this.UpdateValue(this.ActivityTrigger, false);

            foreach (BoolValueObject observationStep in this.ObservationTriggers)
                this.UpdateValue(observationStep, false);

        }

        /// <summary>
        ///   Sends a telegram if the value differs from the current value.</summary>
        /// <param name="output">
        ///   The output port.</param>
        /// <param name="value">
        ///   The value.</param>
        private void UpdateValue(BoolValueObject output, Boolean value) {

            if (output != null && !output.ValueEquals(value))
                output.Value = value;

        }

    }

}
