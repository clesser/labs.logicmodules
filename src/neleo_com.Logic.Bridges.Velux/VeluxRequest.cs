using System;

using LogicModule.Nodes.Helpers;
using LogicModule.ObjectModel;
using LogicModule.ObjectModel.TypeSystem;

using neleo_com.Logic.Bridges.Velux.Definitions;

namespace neleo_com.Logic.Bridges.Velux {

    /// <summary>
    ///   Definition of request scopes.</summary>
    public static class Scope {

        public const String ScopeNode = nameof(Scope.ScopeNode); // default
        public const String ScopeGroup = nameof(Scope.ScopeGroup);
        public const String ScopeScene = nameof(Scope.ScopeScene);

    }

    /// <summary>
    ///   Definition of request velocity options.</summary>
    public static class Velocity {

        public const String VelocityDefault = nameof(Velocity.VelocityDefault); // default
        public const String VelocitySilent = nameof(Velocity.VelocitySilent);
        public const String VelocityFast = nameof(Velocity.VelocityFast);

    }

    /// <summary>
    ///   Definition of request source.</summary>
    public static class Source {

        public const String SourceUser = nameof(Source.SourceUser); // default
        public const String SourceRain = nameof(Source.SourceRain);
        public const String SourceWind = nameof(Source.SourceWind);
        public const String SourceTimer = nameof(Source.SourceTimer);
        public const String SourceUPS = nameof(Source.SourceUPS);
        public const String SourceSAAC = nameof(Source.SourceSAAC);
        public const String SourceEmergency = nameof(Source.SourceEmergency);

    }

    /// <summary>
    ///   Definition of request priority.</summary>
    public static class Priority {

        public const String PriorityHumanProtection = nameof(Priority.PriorityHumanProtection);
        public const String PriorityEnvironmentProtection = nameof(Priority.PriorityEnvironmentProtection);
        public const String PriorityUserLevel1 = nameof(Priority.PriorityUserLevel1);
        public const String PriorityUserLevel2 = nameof(Priority.PriorityUserLevel2); // default
        public const String PriorityComfortLevel1 = nameof(Priority.PriorityComfortLevel1);
        public const String PriorityComfortLevel2 = nameof(Priority.PriorityComfortLevel2);
        public const String PriorityComfortLevel3 = nameof(Priority.PriorityComfortLevel3);
        public const String PriorityComfortLevel4 = nameof(Priority.PriorityComfortLevel4);

    }

    /// <summary>
    ///   Definition of ranges and units for output values.</summary>
    public static class RangeAndUnit {

        public const String RangeDefault = nameof(RangeAndUnit.RangeDefault);
        public const String RangePercentAsc = nameof(RangeAndUnit.RangePercentAsc); // default
        public const String RangePercentDesc = nameof(RangeAndUnit.RangePercentDesc);

    }

    /// <summary>
    ///   Handles requests to a Velux KLF-200 gateway.</summary>
    public class VeluxRequest : LogicNodeBase {

        /// <summary>
        ///   The Type Service manages incoming and outgoing ports.</summary>
        private readonly ITypeService TypeService;

        /// <summary>
        ///   The Scheduler Service manages access to the clock and scheduled callbacks.</summary>
        private readonly ISchedulerService SchedulerService;

        /// <summary>
        ///   Request scope selector.</summary>
        [Parameter(DisplayOrder = 1, IsDefaultShown = false)]
        public EnumValueObject RequestScope {
            get; set;
        }

        /// <summary>
        ///   Defines the output value range and unit.</summary>
        [Parameter(DisplayOrder = 2, IsDefaultShown = false)]
        public EnumValueObject RequestRangeAndUnit {
            get; private set;
        }

        /// <summary>
        ///   Information about the target value of the node or group default parameter.</summary>
        [Input(DisplayOrder = 3, IsInput = true, IsDefaultShown = true)]
        public IntValueObject RequestTargetInteger {
            get; private set;
        }

        /// <summary>
        ///   Information about the target value (in %) of the node or group default parameter.</summary>
        [Input(DisplayOrder = 3, IsInput = true, IsDefaultShown = true)]
        public DoubleValueObject RequestTargetPercent {
            get; private set;
        }

        /// <summary>
        ///   Request a node/group to move to min (=0) or max (=1) position.</summary>
        [Input(DisplayOrder = 4, IsInput = true, IsDefaultShown = false)]
        public BoolValueObject RequestMinMax {
            get; private set;
        }

        ///   Request a node/group to move to start (=1) or stop (=0) position or to start/stop a scene.</summary>
        [Input(DisplayOrder = 5, IsInput = true, IsDefaultShown = false)]
        public BoolValueObject RequestStartStop {
            get; private set;
        }

        /// <summary>
        ///   Request a node/group to move up (=1) or down (=0) position and finishes 
        ///   with a STOP signal after <see cref="RequestUpDownTimeSpan"/>.</summary>
        [Input(DisplayOrder = 6, IsInput = true, IsDefaultShown = false)]
        public BoolValueObject RequestLongUpDown {
            get; private set;
        }

        /// <summary>
        ///   Duration to move the actuator vom min to max (or vice versa). 
        ///   For <see cref="RequestUpDownTimeSpan"/> after a <see cref="RequestLongUpDown"/> 
        ///   the <see cref="RequestShortUpDown"/> will be handled as a STOP signal.</summary>
        [Parameter(DisplayOrder = 7, IsDefaultShown = false)]
        public TimeSpanValueObject RequestUpDownTimeSpan {
            get; private set;
        }

        /// <summary>
        ///   Request a node/group to move up (=1) or down (=0) 
        ///   for <see cref="RequestUpDownStepSize"/>%.</summary>
        [Input(DisplayOrder = 8, IsInput = true, IsDefaultShown = false)]
        public BoolValueObject RequestShortUpDown {
            get; private set;
        }

        /// <summary>
        ///   Defines the step size (0..100%) for a <see cref="RequestShortUpDown"/> signal.</summary>
        [Parameter(DisplayOrder = 9, IsDefaultShown = false)]
        public IntValueObject RequestUpDownStepSize {
            get; private set;
        }

        /// <summary>
        ///   Name of the node to associate the outgoing telegrams to a node, group or scene.</summary>
        [Parameter(DisplayOrder = 10, IsDefaultShown = true)]
        public StringValueObject RequestIdentifier {
            get; private set;
        }

        /// <summary>
        ///   Request execution velocity.</summary>
        [Parameter(DisplayOrder = 11, IsDefaultShown = false)]
        public EnumValueObject RequestVelocity {
            get; private set;
        }

        /// <summary>
        ///   The request source.</summary>
        [Parameter(DisplayOrder = 12, IsDefaultShown = false)]
        public EnumValueObject RequestSource {
            get; private set;
        }

        /// <summary>
        ///   The request priority.</summary>
        [Parameter(DisplayOrder = 13, IsDefaultShown = false)]
        public EnumValueObject RequestPriority {
            get; private set;
        }

        /// <summary>
        ///   Publishes translated requests to the gateway.</summary>
        [Output(DisplayOrder = 1, IsDefaultShown = true)]
        public StringValueObject GatewayRequest {
            get; private set;
        }

        /// <summary>
        ///   The mode of the telegram.</summary>
        private readonly Klf200TelegramMode TelegramMode = Klf200TelegramMode.Request;

        /// <summary>
        ///   The scope of the telegram.</summary>
        private Klf200TelegramScope TelegramScope = Klf200TelegramScope.Node;

        /// <summary>
        ///   The execution velocity.</summary>
        private GW_NodeVelocity TelegramVelocity = GW_NodeVelocity.Default;

        /// <summary>
        ///   The command originator.</summary>
        private GW_CommandSource TelegramSource = GW_CommandSource.User;

        /// <summary>
        ///   The command priority.</summary>
        private GW_CommandPriority TelegramPriority = GW_CommandPriority.UserLevel2;

        /// <summary>
        ///   Reference to a moving/executing actuator that can be stopped.</summary>
        private DateTime Countdown = DateTime.MinValue;

        /// <summary>
        ///   Captures the last target value to avoid resubmitting of values 
        ///   while the <see cref="Countdown"/> is active.</summary>
        private Int32 LastRequestTarget = Int32.MinValue;

        /// <summary>
        ///   Initializes the Velux Gateway Request Generator.</summary>
        /// <param name="context">
        ///   Context of the node instance to connect to services.</param>
        public VeluxRequest(INodeContext context) {

            // ensure that the context is set
            context.ThrowIfNull(nameof(context));

            // initialize services
            this.TypeService = context.GetService<ITypeService>();
            this.SchedulerService = context.GetService<ISchedulerService>();

            // initialize generic ports
            this.RequestScope = this.TypeService.CreateEnum(nameof(Scope), nameof(this.RequestScope),
                new String[] { Scope.ScopeNode, Scope.ScopeGroup, Scope.ScopeScene }, Scope.ScopeNode);
            this.RequestScope.ValueSet += this.RequestScope_ValueSet;

            this.RequestIdentifier = this.TypeService.CreateString(PortTypes.String, nameof(this.RequestIdentifier), String.Empty);
            this.RequestIdentifier.MaxLength = 65;

            this.RequestVelocity = this.TypeService.CreateEnum(nameof(Velocity), nameof(this.RequestVelocity),
                new String[] { Velocity.VelocitySilent, Velocity.VelocityDefault, Velocity.VelocityFast }, Velocity.VelocityDefault);
            this.RequestVelocity.ValueSet += this.RequestVelocity_ValueSet;

            this.RequestSource = this.TypeService.CreateEnum(nameof(Source), nameof(this.RequestSource),
                new String[] { Source.SourceUser, Source.SourceRain, Source.SourceWind, Source.SourceTimer, Source.SourceUPS,
                    Source.SourceSAAC, Source.SourceEmergency},
                Source.SourceUser);
            this.RequestSource.ValueSet += this.RequestSource_ValueSet;

            this.RequestPriority = this.TypeService.CreateEnum(nameof(Priority), nameof(this.RequestPriority),
                new String[] { Priority.PriorityHumanProtection, Priority.PriorityEnvironmentProtection,
                    Priority.PriorityUserLevel1, Priority.PriorityUserLevel2, Priority.PriorityComfortLevel1,
                    Priority.PriorityComfortLevel2, Priority.PriorityComfortLevel3, Priority.PriorityComfortLevel4 },
                Priority.PriorityUserLevel2);
            this.RequestPriority.ValueSet += this.RequestPriority_ValueSet;

            this.GatewayRequest = this.TypeService.CreateString(PortTypes.String, nameof(this.GatewayRequest), String.Empty);

            // initialize scope specific ports
            this.SetupScopedPorts(this.RequestScope.Value);

        }

        /// <summary>
        ///   Sets the <see cref="TelegramScope"/> property and adjusts the ports.</summary>
        /// <param name="sender">
        ///   The sender.</param>
        /// <param name="args">
        ///   The arguments.</param>
        private void RequestScope_ValueSet(Object sender, ValueChangedEventArgs args) {

            switch (args.NewValue) {

                case Scope.ScopeNode:
                    this.TelegramScope = Klf200TelegramScope.Node;
                    break;

                case Scope.ScopeGroup:
                    this.TelegramScope = Klf200TelegramScope.Group;
                    break;

                case Scope.ScopeScene:
                    this.TelegramScope = Klf200TelegramScope.Scene;
                    break;

                default:
                    this.TelegramScope = Klf200TelegramScope.None;
                    break;

            }

            if (!String.Equals(args.NewValue, args.OldValue))
                this.SetupScopedPorts(args.NewValue);

        }

        /// <summary>
        ///   Sets the <see cref="TelegramVelocity"/> property.</summary>
        /// <param name="sender">
        ///   The sender.</param>
        /// <param name="args">
        ///   The arguments.</param>
        private void RequestVelocity_ValueSet(Object sender, ValueChangedEventArgs args) {

            switch (args.NewValue) {

                case Velocity.VelocityFast:
                    this.TelegramVelocity = GW_NodeVelocity.Fast;
                    break;

                case Velocity.VelocitySilent:
                    this.TelegramVelocity = GW_NodeVelocity.Silent;
                    break;

                default:
                    this.TelegramVelocity = GW_NodeVelocity.Default;
                    break;

            }

        }

        /// <summary>
        ///   Sets the <see cref="TelegramSource"/> property.</summary>
        /// <param name="sender">
        ///   The sender.</param>
        /// <param name="args">
        ///   The arguments.</param>
        private void RequestSource_ValueSet(Object sender, ValueChangedEventArgs args) {

            switch (args.NewValue) {

                case Source.SourceEmergency:
                    this.TelegramSource = GW_CommandSource.Emergency;
                    break;

                case Source.SourceRain:
                    this.TelegramSource = GW_CommandSource.Rain;
                    break;

                case Source.SourceSAAC:
                    this.TelegramSource = GW_CommandSource.SAAC;
                    break;

                case Source.SourceTimer:
                    this.TelegramSource = GW_CommandSource.Timer;
                    break;

                case Source.SourceUPS:
                    this.TelegramSource = GW_CommandSource.UPS;
                    break;

                case Source.SourceWind:
                    this.TelegramSource = GW_CommandSource.Wind;
                    break;

                default:
                    this.TelegramSource = GW_CommandSource.User;
                    break;

            }

        }

        /// <summary>
        ///   Sets the <see cref="TelegramPriority"/> property.</summary>
        /// <param name="sender">
        ///   The sender.</param>
        /// <param name="args">
        ///   The arguments.</param>
        private void RequestPriority_ValueSet(Object sender, ValueChangedEventArgs args) {

            switch (args.NewValue) {

                case Priority.PriorityHumanProtection:
                    this.TelegramPriority = GW_CommandPriority.HumanProtection;
                    break;

                case Priority.PriorityEnvironmentProtection:
                    this.TelegramPriority = GW_CommandPriority.EnvironmentProtection;
                    break;

                case Priority.PriorityUserLevel1:
                    this.TelegramPriority = GW_CommandPriority.UserLevel1;
                    break;

                case Priority.PriorityComfortLevel1:
                    this.TelegramPriority = GW_CommandPriority.ComfortLevel1;
                    break;

                case Priority.PriorityComfortLevel2:
                    this.TelegramPriority = GW_CommandPriority.ComfortLevel2;
                    break;

                case Priority.PriorityComfortLevel3:
                    this.TelegramPriority = GW_CommandPriority.ComfortLevel3;
                    break;

                case Priority.PriorityComfortLevel4:
                    this.TelegramPriority = GW_CommandPriority.ComfortLevel4;
                    break;

                default:
                    this.TelegramPriority = GW_CommandPriority.UserLevel2;
                    break;

            }

        }

        /// <summary>
        ///   Setup ports accoring to the selected scope.</summary>
        /// <param name="scope">
        ///   The scope (node | group | scene).</param>
        private void SetupScopedPorts(Object scope) {

            Boolean isNode = Scope.ScopeNode.Equals(scope);
            Boolean isGroup = Scope.ScopeGroup.Equals(scope);
            Boolean isScene = Scope.ScopeScene.Equals(scope);

            // RequestRangeAndUnit
            if (isNode || isGroup) {

                if (this.RequestRangeAndUnit == null) {

                    this.RequestRangeAndUnit = this.TypeService.CreateEnum(nameof(RangeAndUnit), nameof(this.RequestRangeAndUnit),
                        new string[] { RangeAndUnit.RangeDefault, RangeAndUnit.RangePercentAsc, RangeAndUnit.RangePercentDesc },
                    RangeAndUnit.RangePercentAsc);

                    this.RequestRangeAndUnit.ValueSet += (sender, args) => SetupScopedPorts(this.RequestScope.Value);

                }

            }
            else {

                this.RequestRangeAndUnit = null;

            }

            // RequestTarget{Integer|Percent}
            if ((isNode || isGroup) && this.RequestRangeAndUnit.HasValue) {

                switch (this.RequestRangeAndUnit.Value) {

                    case RangeAndUnit.RangePercentAsc:
                    case RangeAndUnit.RangePercentDesc:
                        if (this.RequestTargetPercent == null) {

                            this.RequestTargetPercent = this.TypeService.CreateDouble(PortTypes.Percent, nameof(this.RequestTargetPercent), 0);
                            this.RequestTargetPercent.MinValue = 0;
                            this.RequestTargetPercent.MaxValue = 100;

                        }
                        this.RequestTargetInteger = null;
                        break;

                    default:
                        if (this.RequestTargetInteger == null) {

                            this.RequestTargetInteger = this.TypeService.CreateInt(PortTypes.Integer, nameof(this.RequestTargetInteger), 0);
                            this.RequestTargetInteger.MinValue = 0;
                            this.RequestTargetInteger.MaxValue = 51200;

                        }
                        this.RequestTargetPercent = null;
                        break;

                }

            }
            else {

                this.RequestTargetInteger = null;
                this.RequestTargetPercent = null;

            }

            // RequestMinMax
            if (isNode || isGroup) {

                if (this.RequestMinMax == null) {

                    this.RequestMinMax = this.TypeService.CreateBool(PortTypes.Binary, nameof(this.RequestMinMax), false);

                }

            }
            else {

                this.RequestMinMax = null;


            }

            // RequestStartStop
            if (isNode || isGroup || isScene) {

                if (this.RequestStartStop == null) {

                    this.RequestStartStop = this.TypeService.CreateBool(PortTypes.Binary, nameof(this.RequestStartStop), false);

                }

            }
            else {

                this.RequestStartStop = null;

            }

            // RequestLongUpDown
            if (isNode || isGroup) {

                if (this.RequestLongUpDown == null) {

                    this.RequestLongUpDown = this.TypeService.CreateBool(PortTypes.Binary, nameof(this.RequestLongUpDown), false);

                }

            }
            else {

                this.RequestLongUpDown = null;


            }

            // RequestUpDownTimeSpan
            if (isNode || isGroup) {

                if (this.RequestUpDownTimeSpan == null) {

                    this.RequestUpDownTimeSpan = this.TypeService.CreateTimeSpan(PortTypes.TimeSpan,
                        nameof(this.RequestUpDownTimeSpan), TimeSpan.FromMinutes(1));

                }

            }
            else {

                this.RequestUpDownTimeSpan = null;

            }

            // RequestShortUpDown
            if (isNode || isGroup) {

                if (this.RequestShortUpDown == null) {

                    this.RequestShortUpDown = this.TypeService.CreateBool(PortTypes.Binary, nameof(this.RequestShortUpDown), false);

                }

            }
            else {

                this.RequestShortUpDown = null;


            }

            // RequestUpDownStepSize
            if (isNode || isGroup) {

                if (this.RequestUpDownStepSize == null) {

                    this.RequestUpDownStepSize = this.TypeService.CreateInt(PortTypes.Integer, nameof(this.RequestUpDownStepSize), 10);
                    this.RequestUpDownStepSize.MinValue = 0;
                    this.RequestUpDownStepSize.MaxValue = 100;

                }

            }
            else {

                this.RequestUpDownStepSize = null;

            }

        }

        /// <summary>
        ///   React on incoming triggers and values.</summary>
        public override void Execute() {

            // ignore any incoming values if identifier is not set
            if (!this.RequestIdentifier.HasValue || String.IsNullOrWhiteSpace(this.RequestIdentifier.Value))
                return;

            // build a raw telegram
            Klf200Telegram telegram = this.ComposeRawTelegram();

            Boolean countdownIsActive = this.Countdown > this.SchedulerService.Now;
            Boolean setupNewCountdown = false;


            // target value (Integer) set?
            if (this.RequestTargetInteger != null && this.RequestTargetInteger.WasSet) {

                // get new target value
                Int32 newRequestTargetInteger = this.RequestTargetInteger.Value;

                // stop processing if the same value should be submitted while countdown is active
                if (countdownIsActive && this.LastRequestTarget == newRequestTargetInteger)
                    return;

                // set new target value intelegram
                this.LastRequestTarget = newRequestTargetInteger;
                setupNewCountdown = true;

                telegram.SetParameter(Klf200TelegramParameter.Target, newRequestTargetInteger);

            }

            // target value (Percent) set?
            if (this.RequestTargetPercent != null && this.RequestRangeAndUnit != null && this.RequestTargetPercent.WasSet) {

                // get new target value
                Int32 newRequestTargetPercent = (Int32)this.RequestTargetPercent.Value;

                // stop processing if the same value should be submitted while countdown is active
                if (countdownIsActive && this.LastRequestTarget == newRequestTargetPercent)
                    return;

                // set new target value intelegram
                this.LastRequestTarget = newRequestTargetPercent;
                setupNewCountdown = true;

                switch (this.RequestRangeAndUnit.Value) {

                    case RangeAndUnit.RangePercentDesc:

                        telegram.SetParameter(Klf200TelegramParameter.Target, (100 - newRequestTargetPercent) * 512);
                        break;

                    default:
                        telegram.SetParameter(Klf200TelegramParameter.Target, newRequestTargetPercent * 512);
                        break;

                }

            }

            // min / max set?
            if (this.RequestMinMax != null && this.RequestMinMax.WasSet) {

                telegram.Action = this.RequestMinMax.Value ? "max" : "min";
                setupNewCountdown = true;

            }

            // start / stop set?
            if (this.RequestStartStop != null && this.RequestStartStop.WasSet) {

                telegram.Action = this.RequestStartStop.Value ? "start" : "stop";
                setupNewCountdown = this.RequestStartStop.Value;

            }

            // long up / down set?
            if (this.RequestLongUpDown != null && this.RequestLongUpDown.WasSet) {

                telegram.Action = this.RequestLongUpDown.Value ? "max" : "min";
                setupNewCountdown = true;

            }

            // short up / down set?
            if (this.RequestShortUpDown != null && this.RequestShortUpDown.WasSet) {

                if (countdownIsActive) {

                    telegram.Action = "stop";
                    this.Countdown = DateTime.MinValue;

                }
                else {

                    Int32 step = this.RequestShortUpDown.Value ? 10 : -10;
                    if (this.RequestUpDownStepSize.HasValue)
                        step *= this.RequestUpDownStepSize.Value;
                    else
                        step *= 10; // default step size

                    // 0xC900 = 51456 = -100%, 0xCCE8 = 52456 = +/-0%, 0xD0D0 = 53456 = +100%
                    telegram.SetParameter(Klf200TelegramParameter.Target, 52456 + step);

                }

                setupNewCountdown = false;

            }

            // setup countdown?
            if (this.RequestUpDownTimeSpan != null && setupNewCountdown)
                this.Countdown = this.SchedulerService.Now.Add(this.RequestUpDownTimeSpan.Value);

            // send telegram
            this.GatewayRequest.Value = telegram.ToString();

        }

        /// <summary>
        ///   Composes a raw telegram with all standard parameters - 
        ///   but without any values to activate the node, group or scene.</summary>
        /// <returns>
        ///   A raw telegram.</returns>
        private Klf200Telegram ComposeRawTelegram() {

            // compose the telegram body
            Klf200Telegram telegram = new Klf200Telegram() {
                Mode = this.TelegramMode,
                Scope = this.TelegramScope
            };

            // extract identifier
            String identifierOrName = this.RequestIdentifier.Value;
            switch (identifierOrName[0]) {
                case ':':
                    telegram.Identifier = Byte.Parse(identifierOrName.Substring(1));
                    break;
                case '/':
                    telegram.Name = identifierOrName.Substring(1);
                    break;
                default:
                    telegram.Name = identifierOrName;
                    break;
            }

            // add standard parameters if they have non-default settings
            if (this.TelegramSource != GW_CommandSource.User)
                telegram.SetParameter(Klf200TelegramParameter.Source, this.TelegramSource);

            if (this.TelegramPriority != GW_CommandPriority.UserLevel2)
                telegram.SetParameter(Klf200TelegramParameter.Priority, this.TelegramPriority);

            if (this.TelegramVelocity != GW_NodeVelocity.Default)
                telegram.SetParameter(Klf200TelegramParameter.Velocity, this.TelegramVelocity);

            // return raw telegram
            return telegram;

        }

    }

}
