using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

using LogicModule.Nodes.Helpers;
using LogicModule.ObjectModel;
using LogicModule.ObjectModel.TypeSystem;

namespace neleo_com.Logic.Timing {

    /// <summary>
    ///   Alarm setup modes.</summary>
    public static class AlarmMode {

        public const String DateTime = nameof(AlarmMode.DateTime);
        public const String Time = nameof(AlarmMode.Time);
        public const String Trigger = nameof(AlarmMode.Trigger);

    }


    /// <summary>
    ///   An alarm that triggers steps of a sequence before it actually become active.</summary>
    public class Alarm : LogicNodeBase {

        /// <summary>
        ///   The Type Service manages incoming and outgoing ports.</summary>
        private readonly ITypeService TypeService;

        /// <summary>
        ///   The Scheduler Service manages access to the clock and scheduled callbacks.</summary>
        private readonly ISchedulerService SchedulerService;

        /// <summary>
        ///   Minimal wait time between two steps or the snooze-alarm-cycle.</summary>
        private static readonly TimeSpan MinDuration = new TimeSpan(0, 0, 30);

        /// <summary>
        ///   Maximum wait time between two steps or the snooze-alarm-cycle.</summary>
        private static readonly TimeSpan MaxDuration = new TimeSpan(0, 30, 0);

        /// <summary>
        ///   Default wait time between two steps or the snooze-alarm-cycle.</summary>
        private static readonly TimeSpan DefaultDuration = new TimeSpan(0, 5, 0);

        /// <summary>
        ///   Minimal number of sequence steps prior to the alarm trigger.</summary>
        private static readonly Int32 MinSteps = 0;

        /// <summary>
        ///   Maximum number of sequence steps prior to the alarm trigger.</summary>
        private static readonly Int32 MaxSteps = 16;

        /// <summary>
        ///   The active step in a sequence.</summary>
        private Int32 SequenceActivePosition = 0;

        /// <summary>
        ///   The available setup modes for the alarm.</summary>
        [Parameter(DisplayOrder = 1, IsDefaultShown = false)]
        public EnumValueObject AlarmModeSelector {
            get; private set;
        }

        /// <summary>
        ///   The alarm date and time.</summary>
        [Input(DisplayOrder = 2, IsRequired = true)]
        public DateTimeValueObject AlarmDateTime {
            get; private set;
        }

        /// <summary>
        ///   The alarm time.</summary>
        [Input(DisplayOrder = 2, IsRequired = true)]
        public TimeSpanValueObject AlarmTime {
            get; private set;
        }

        /// <summary>
        ///   Triggers the alarm to start the sequence (not the alarm itself!).</summary>
        [Input(DisplayOrder = 2, IsRequired = true)]
        public BoolValueObject AlarmTrigger {
            get; private set;
        }

        /// <summary>
        ///   Allows users to define a number of sequence steps.</summary>
        [Parameter(DisplayOrder = 3, IsDefaultShown = false)]
        public IntValueObject SequenceSteps {
            get; private set;
        }

        /// <summary>
        ///   Trigger actuates when the sequence starts and stops.</summary>
        [Output(DisplayOrder = 3, IsDefaultShown = true)]
        public BoolValueObject SequenceActive {
            get; private set;
        }

        /// <summary>
        ///   Duration option of each sequence step.</summary>
        [Parameter(DisplayOrder = 4, IsDefaultShown = true)]
        public IList<TimeSpanValueObject> SequenceStepDuration {
            get; private set;
        }

        /// <summary>
        ///   Trigger actuates for each sequence step and waits for the given duration to move to the next step.</summary>
        [Output(DisplayOrder = 4, IsDefaultShown = true)]
        public IList<BoolValueObject> SequenceStepTrigger {
            get; private set;
        }

        /// <summary>
        ///   Confirms the alarm. Sending <c>false</c> will result in triggering <see cref="SequenceSnoozeTrigger"/> (if <see cref="SequenceAlarmTrigger"/> has been triggered before), sending <c>true</c> instead will stop the sequence or alarm and trigger <see cref="SequenceQuitTrigger"/>.</summary>
        [Input(DisplayOrder = 5, IsRequired = true)]
        public BoolValueObject ConfirmAlarm {
            get; private set;
        }

        /// <summary>
        ///   Trigger actuates after the last sequence step and after <see cref="SnoozeDuration"/>.</summary>
        [Output(DisplayOrder = 5, IsDefaultShown = true)]
        public BoolValueObject SequenceAlarmTrigger {
            get; private set;
        }

        /// <summary>
        ///   Duration of a snooze step (in minutes).</summary>
        [Input(DisplayOrder = 6, IsDefaultShown = true, IsInput = false)]
        public TimeSpanValueObject SnoozeDuration {
            get; private set;
        }

        /// <summary>
        ///   Trigger actuates if the users sends a <c>false</c> telegram to <see cref="ConfirmAlarm"/>.</summary>
        [Output(DisplayOrder = 6, IsDefaultShown = true)]
        public BoolValueObject SequenceSnoozeTrigger {
            get; private set;
        }

        /// <summary>
        ///   Switch to enable or disable the alarm (if the alarm is disabled, the sequence won't start when it's triggered).</summary>
        [Input(DisplayOrder = 7, IsDefaultShown = true)]
        public BoolValueObject Enabled {
            get; private set;
        }

        /// <summary>
        ///   States if the alarm is active (will start the sequence if the time/trigger asks for it).</summary>
        [Output(DisplayOrder = 7, IsDefaultShown = true)]
        public BoolValueObject EnabledState {
            get; private set;
        }

        /// <summary>
        ///   Date / time of the next wakeup (without sequence lead time).</summary>
        private DateTime LocalNextWakeupDateTime {
            get; set;
        } = DateTime.MinValue;

        /// <summary>
        ///   A token for the scheduled refresh cycle for <see cref="EnabledState"/>.</summary>
        private SchedulerToken UpdateEnabledStateToken {
            get; set;
        }

        /// <summary>
        ///   A token to manage the timed start of a sequence.</summary>
        private SchedulerToken SequenceWakeupToken {
            get; set;
        }

        /// <summary>
        ///   A list of tokens to manage the timed start of the sequence steps, alarm and snooze triggers.</summary>
        private IList<SchedulerToken> SequenceStepWakeupTokens {
            get; set;
        }

        /// <summary>
        ///   Constructor to setup the ports and services.</summary>
        /// <param name="context">
        ///   Context of the node instance to connect to services.</param>
        public Alarm(INodeContext context) : base(context) {

            context.ThrowIfNull(nameof(context));

            this.TypeService = context.GetService<ITypeService>();
            this.SchedulerService = context.GetService<ISchedulerService>();

            this.AlarmModeSelector = this.TypeService.CreateEnum(nameof(AlarmMode), nameof(AlarmModeSelector),
                new String[] { AlarmMode.DateTime, AlarmMode.Time, AlarmMode.Trigger }, AlarmMode.Trigger);
            this.AlarmModeSelector.ValueSet += this.AlarmModeSelector_ValueSet;

            this.AlarmTrigger = this.TypeService.CreateBool(PortTypes.Binary, nameof(this.AlarmTrigger), false);

            this.SequenceSteps = this.TypeService.CreateInt(PortTypes.Integer, nameof(this.SequenceSteps), 0);
            this.SequenceSteps.MinValue = Alarm.MinSteps;
            this.SequenceSteps.MaxValue = Alarm.MaxSteps;

            this.SequenceActive = this.TypeService.CreateBool(PortTypes.Binary, nameof(this.SequenceActive), false);

            this.SequenceStepDuration = new List<TimeSpanValueObject>();
            ListHelpers.ConnectListToCounter(this.SequenceStepDuration, this.SequenceSteps,
                this.TypeService.GetValueObjectCreator(PortTypes.TimeSpan, nameof(this.SequenceStepDuration)), (s, args) => {
                    foreach (TimeSpanValueObject stepDuration in this.SequenceStepDuration) {
                        stepDuration.MinValue = Alarm.MinDuration;
                        stepDuration.MaxValue = Alarm.MaxDuration;
                        if (!stepDuration.HasValue)
                            stepDuration.Value = Alarm.DefaultDuration;
                    }
                });

            this.SequenceStepTrigger = new List<BoolValueObject>();
            ListHelpers.ConnectListToCounter(this.SequenceStepTrigger, this.SequenceSteps,
                this.TypeService.GetValueObjectCreator(PortTypes.Binary, nameof(this.SequenceStepTrigger), false), null);

            this.ConfirmAlarm = this.TypeService.CreateBool(PortTypes.Binary, nameof(this.ConfirmAlarm), true);

            this.SequenceAlarmTrigger = this.TypeService.CreateBool(PortTypes.Binary, nameof(this.SequenceAlarmTrigger), false);

            this.SnoozeDuration = this.TypeService.CreateTimeSpan(PortTypes.TimeSpan, nameof(this.SnoozeDuration));
            this.SnoozeDuration.MinValue = Alarm.MinDuration;
            this.SnoozeDuration.MaxValue = Alarm.MaxDuration;
            this.SnoozeDuration.Value = Alarm.DefaultDuration;

            this.SequenceSnoozeTrigger = this.TypeService.CreateBool(PortTypes.Binary, nameof(this.SequenceSnoozeTrigger), false);

            this.Enabled = this.TypeService.CreateBool(PortTypes.Binary, nameof(this.Enabled), false);
            this.EnabledState = this.TypeService.CreateBool(PortTypes.Binary, nameof(this.EnabledState), false);

        }

        /// <summary>
        ///   Handle configuration changes for the alarm setup.</summary>
        /// <param name="sender">
        ///   The sender.</param>
        /// <param name="args">
        ///   Information about changed properties.</param>
        private void AlarmModeSelector_ValueSet(Object sender, ValueChangedEventArgs args) {

            if (!args.NewValue.Equals(args.OldValue)) {

                switch (args.NewValue.ToString()) {

                    case AlarmMode.DateTime:
                        this.AlarmDateTime = this.TypeService.CreateDateTime(PortTypes.DateTime, nameof(this.AlarmDateTime));
                        this.AlarmTime = null;
                        this.AlarmTrigger = null;
                        break;

                    case AlarmMode.Time:
                        this.AlarmDateTime = null;
                        this.AlarmTime = this.TypeService.CreateTimeSpan(PortTypes.Time, nameof(this.AlarmTime));
                        this.AlarmTime.MinValue = new TimeSpan(0, 0, 0);
                        this.AlarmTime.MaxValue = new TimeSpan(23, 59, 59);
                        this.AlarmTrigger = null;
                        break;

                    default:
                        this.AlarmDateTime = null;
                        this.AlarmTime = null;
                        this.AlarmTrigger = this.TypeService.CreateBool(PortTypes.Binary, nameof(this.AlarmTrigger), false);
                        break;

                }

            }

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
            if (key.StartsWith(nameof(this.SequenceStepDuration))) {

                String identifier = key.Substring(nameof(this.SequenceStepDuration).Length);
                return String.Format(ResourceManager.GetString(nameof(this.SequenceStepDuration), culture) ?? key, identifier);

            }
            else if (key.StartsWith(nameof(this.SequenceStepTrigger))) {

                String identifier = key.Substring(nameof(this.SequenceStepTrigger).Length);
                return String.Format(ResourceManager.GetString(nameof(this.SequenceStepTrigger), culture) ?? key, identifier);

            }
            else {

                return ResourceManager.GetString(key, culture) ?? key;

            }

        }

        /// <summary>
        ///   Initializes the alarm and outputs.</summary>
        public override void Startup() {

            this.ResetSequence();
            this.ResetTriggers();

            this.TriggerOnChange(this.SequenceActive, false);
            this.TriggerOnChange(this.EnabledState, this.Enabled != null && this.Enabled.HasValue && this.Enabled.Value);

            if (this.AlarmDateTime != null && this.AlarmDateTime.HasValue)
                this.SetupSequenceWakeupTime(true);

            if (this.AlarmTime != null && this.AlarmTime.HasValue)
                this.SetupSequenceWakeupTime(true);

            if (this.Enabled.HasValue)
                this.TriggerOnChange(this.EnabledState, this.Enabled.Value);

            this.UpdateEnabledState();

        }

        /// <summary>
        ///   Configures the alarm and starts or reacts to trigger telegrams.</summary>
        public override void Execute() {

            if (this.AlarmDateTime != null && this.AlarmDateTime.HasValue && this.AlarmDateTime.WasSet)
                this.SetupSequenceWakeupTime(true);

            if (this.AlarmTime != null && this.AlarmTime.HasValue && this.AlarmTime.WasSet)
                this.SetupSequenceWakeupTime(true);

            if (this.AlarmTrigger != null && this.AlarmTrigger.HasValue && this.AlarmTrigger.WasSet && this.AlarmTrigger.Value)
                this.SetupSequence();

            if (this.SequenceActive.HasValue && this.SequenceActive.Value) {

                if (this.ConfirmAlarm.HasValue && this.ConfirmAlarm.WasSet)
                    if (this.ConfirmAlarm.Value)
                        this.TriggerStop();
                    else
                        this.TriggerSnooze();

                if (this.Enabled.HasValue && this.Enabled.WasSet && !this.Enabled.Value)
                    this.TriggerStop();

            }

            this.UpdateEnabledState();

        }

        /// <summary>
        ///   Setup an internal callback to the next possible (today or tomorrow) sequence start time.</summary>
        /// <param name="newValue">
        ///   <c>true</c> to setup a new alarm time, otherwise <c>false</c>.</param>
        private void SetupSequenceWakeupTime(Boolean newValue) {

            DateTime localNow = this.SchedulerService.Now;
            this.LocalNextWakeupDateTime = DateTime.MinValue;

            // ensure that there's an alarm time configured
            if (this.AlarmDateTime != null && this.AlarmDateTime.HasValue)
                this.LocalNextWakeupDateTime = this.AlarmDateTime.Value;

            else if (this.AlarmTime != null && this.AlarmTime.HasValue)
                this.LocalNextWakeupDateTime = localNow.Date.Add(this.AlarmTime.Value);

            else
                return;

            // setup alarm sequence
            // Note: newValue == false: this method is triggered by the SchedulerService / wakeup time
            if (!newValue)
                this.SetupSequence();

            // remove an old token if there is one
            // Note: newValue == true: this method is triggered by the KNX / new alarm time
            if (newValue && this.SequenceWakeupToken != null)
                this.SchedulerService.Remove(this.SequenceWakeupToken);

            // calculate the duration of the sequence
            TimeSpan sequenceDuration = this.SequenceStepDuration.Aggregate(TimeSpan.Zero,
                (subtotal, t) => subtotal.Add(t.HasValue ? t.Value : TimeSpan.Zero));

            // find the next start date time for the sequence (local time)
            DateTime localNextStart = this.LocalNextWakeupDateTime.Subtract(sequenceDuration);
            if (localNextStart < localNow)
                localNextStart = localNextStart.AddDays(1);

            // create callback to setup the wakeup for the next day and start the sequence (universal time)
            DateTime utcNextStart = localNextStart.ToUniversalTime();
            this.SequenceWakeupToken = this.SchedulerService.InvokeAt(utcNextStart, () => this.SetupSequenceWakeupTime(false));

        }

        /// <summary>
        ///   Aborts potentially scheduled sequence step wakeup handlers and removes the sequence step wakeup time tokens.</summary>
        private void ResetSequence() {

            // exit if there's no token
            if (this.SequenceStepWakeupTokens == null)
                return;

            // deregister the handlers that are associated with the tokens
            foreach (SchedulerToken token in this.SequenceStepWakeupTokens)
                this.SchedulerService.Remove(token);

            // remove the token references
            this.SequenceStepWakeupTokens = null;

        }

        /// <summary>
        ///   If the alarm is active, all sequence steps (incl. the alarm) will be registered with the SchedulerService.</summary>
        private void SetupSequence() {

            // avoid starting a new sequence if there is an active one
            if (this.SequenceActive.HasValue && this.SequenceActive.Value)
                return;

            // remove previous tokens if there are any
            this.ResetSequence();

            // avoid setting up a new sequence if the alarm isn't active
            if (!this.Enabled.HasValue || !this.Enabled.Value)
                return;

            // start the sequence
            this.TriggerStart();
            this.SequenceActivePosition = 0;

            // add all triggers to the queue
            TimeSpan waitTime = new TimeSpan(0, 0, 3);
            this.SequenceStepWakeupTokens = new List<SchedulerToken>();
            foreach (TimeSpanValueObject stepDuration in this.SequenceStepDuration) {
                this.SequenceStepWakeupTokens.Add(this.SchedulerService.InvokeIn(waitTime, () => this.TriggerStep()));
                waitTime = waitTime.Add(stepDuration.HasValue ? stepDuration.Value : TimeSpan.Zero);
            }

            // add the alarm to the queue
            this.SequenceStepWakeupTokens.Add(this.SchedulerService.InvokeIn(waitTime, () => this.TriggerAlarm()));

        }

        /// <summary>
        ///   Sends a telegram if the value differs from the currently set value.</summary>
        /// <param name="output">
        ///   The output port.</param>
        /// <param name="value">
        ///   The value.</param>
        private void TriggerOnChange(BoolValueObject output, Boolean value) {

            if (output != null)
                if (!output.HasValue || (output.HasValue && output.Value != value))
                    output.Value = value;

        }

        /// <summary>
        ///   Set all external triggers off (false).</summary>
        private void ResetTriggers() {

            foreach (BoolValueObject stepTrigger in this.SequenceStepTrigger)
                this.TriggerOnChange(stepTrigger, false);

            this.TriggerOnChange(this.SequenceAlarmTrigger, false);
            this.TriggerOnChange(this.SequenceSnoozeTrigger, false);

        }

        /// <summary>
        ///   Notifies external listeners that the execution of the wake-up sequence has started.</summary>
        private void TriggerStart() {

            // reset triggers
            this.ResetTriggers();

            // send sequence activation telegram
            this.TriggerOnChange(this.SequenceActive, true);

        }

        /// <summary>
        ///   Notifies external listeners about the execution of a sequence step.</summary>
        private void TriggerStep() {

            // reset triggers
            this.ResetTriggers();

            // enable current step
            if (this.SequenceActivePosition < this.SequenceStepTrigger.Count())
                this.TriggerOnChange(this.SequenceStepTrigger[this.SequenceActivePosition], true);

            // iterate to next step
            this.SequenceActivePosition += 1;

        }

        /// <summary>
        ///   Notifies external listeners about the execution of the alarm sequence step.</summary>
        private void TriggerAlarm() {

            // reset triggers
            this.ResetTriggers();

            // enable the alarm
            this.TriggerOnChange(this.SequenceAlarmTrigger, true);

        }

        /// <summary>
        ///   Notifies external listeners that the alarm has entered the snooze step.</summary>
        private void TriggerSnooze() {

            // check if snoozing is possible (snooze duration is set and alarm is active)
            /*Boolean snoozable = (this.SnoozeDuration != null && this.SnoozeDuration.HasValue
                && this.SequenceAlarmTrigger != null && this.SequenceAlarmTrigger.HasValue && this.SequenceAlarmTrigger.Value);*/

            // ignore further steps if snooze mode can't be entered
            /*if(!snoozable)*/
            if (this.SnoozeDuration == null || !this.SnoozeDuration.HasValue)
                return;

            // reset triggers
            this.ResetTriggers();

            // remove previous tokens if there are any
            this.ResetSequence();

            // setup the alarm again
            this.SequenceStepWakeupTokens = new List<SchedulerToken>();
            this.SequenceStepWakeupTokens.Add(this.SchedulerService.InvokeIn(this.SnoozeDuration.Value, () => this.TriggerAlarm()));

            // enable snoozing
            this.TriggerOnChange(this.SequenceSnoozeTrigger, true);

        }

        /// <summary>
        ///   Notifies external listeners that the alarm has been turned off or that the sequence has been aborted.</summary>
        private void TriggerStop() {

            // reset triggers
            this.ResetTriggers();

            // remove previous tokens if there are any
            this.ResetSequence();

            // send sequence deactivation telegram
            this.TriggerOnChange(this.SequenceActive, false);

        }

        /// <summary>
        ///   Updates the enabled output port based on the current configuration.</summary>
        private void UpdateEnabledState() {

            Boolean enabled = this.Enabled.HasValue && this.Enabled.Value;

            // pass "enabled" if module is in Trigger mode
            // if the module is deactivate in any mode, turn off update scheduler
            if (this.AlarmModeSelector.Value == AlarmMode.Trigger || !enabled) {

                this.TriggerOnChange(this.EnabledState, enabled);

                if (this.UpdateEnabledStateToken != null)
                    this.SchedulerService.Remove(this.UpdateEnabledStateToken);

                this.UpdateEnabledStateToken = null;

            }

            // check if there's a wakeup time scheduled for the next 24h
            // setup the update scheduler to refresh the value every hour
            else {

                DateTime localNow = this.SchedulerService.Now;
                enabled &= this.LocalNextWakeupDateTime > localNow;
                enabled &= this.LocalNextWakeupDateTime < localNow.AddDays(1);

                this.TriggerOnChange(this.EnabledState, enabled);

                this.UpdateEnabledStateToken = this.SchedulerService.InvokeIn(TimeSpan.FromHours(1), this.UpdateEnabledState);

            }

        }

    }

}
