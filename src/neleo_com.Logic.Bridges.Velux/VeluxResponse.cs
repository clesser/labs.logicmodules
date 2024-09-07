using System;
using System.Collections.Generic;

using LogicModule.Nodes.Helpers;
using LogicModule.ObjectModel;
using LogicModule.ObjectModel.TypeSystem;

using neleo_com.Logic.Bridges.Velux.Definitions;

namespace neleo_com.Logic.Bridges.Velux {

    /// <summary>
    ///   Handles responses from a Velux KLF-200 gateway.</summary>
    public class VeluxResponse : LogicNodeBase {

        /// <summary>
        ///   The Type Service manages incoming and outgoing ports.</summary>
        private readonly ITypeService TypeService;

        /// <summary>
        ///   Receives gateway responses.</summary>
        [Input(DisplayOrder = 1, IsDefaultShown = true, IsInput = true)]
        public StringValueObject GatewayResponse {
            get; private set;
        }

        /// <summary>
        ///   Name of the node to filter the incoming telegrams.</summary>
        [Parameter(DisplayOrder = 2, IsDefaultShown = true)]
        public StringValueObject NodeIdentifier {
            get; private set;
        }

        /// <summary>
        ///   Defines the output value range and unit.</summary>
        [Parameter(DisplayOrder = 3, IsDefaultShown = false)]
        public EnumValueObject OutputRangeAndUnit {
            get; private set;
        }

        // TODO: add function parameters (input field 0..16) + current and target fields

        /// <summary>
        ///   Information about the current value of the node parameter.</summary>
        [Output(DisplayOrder = 1, IsDefaultShown = true)]
        public IntValueObject NodeCurrentInteger {
            get; private set;
        }

        /// <summary>
        ///   Information about the current value of the node parameter.</summary>
        [Output(DisplayOrder = 1, IsDefaultShown = true)]
        public DoubleValueObject NodeCurrentPercent {
            get; private set;
        }

        /// <summary>
        ///   Information about the target value of the node parameter.</summary>
        [Output(DisplayOrder = 20, IsDefaultShown = false)]
        public IntValueObject NodeTargetInteger {
            get; private set;
        }

        /// <summary>
        ///   Information about the target value of the node parameter.</summary>
        [Output(DisplayOrder = 20, IsDefaultShown = false)]
        public DoubleValueObject NodeTargetPercent {
            get; private set;
        }

        /// <summary>
        ///   Information about an executing node.</summary>
        [Output(DisplayOrder = 30, IsDefaultShown = true)]
        public BoolValueObject NodeExecuting {
            get; private set;
        }

        /// <summary>
        ///   Information about an error on a node.</summary>
        [Output(DisplayOrder = 31, IsDefaultShown = true)]
        public StringValueObject NodeError {
            get; private set;
        }

        /// <summary>
        ///   The filter for incoming messages.</summary>
        private String MessageFilter = Guid.NewGuid().ToString();

        /// <summary>
        ///   Initializes the Velux Gateway Response Filter.</summary>
        /// <param name="context">
        ///   Context of the node instance to connect to services.</param>
        public VeluxResponse(INodeContext context) {

            // ensure that the context is set
            context.ThrowIfNull(nameof(context));

            // initialize services
            this.TypeService = context.GetService<ITypeService>();

            // initialize standard ports
            this.GatewayResponse = this.TypeService.CreateString(PortTypes.String, nameof(this.GatewayResponse), String.Empty);
            this.NodeIdentifier = this.TypeService.CreateString(PortTypes.String, nameof(this.NodeIdentifier), String.Empty);
            this.NodeIdentifier.MaxLength = 65;

            this.OutputRangeAndUnit = this.TypeService.CreateEnum(nameof(RangeAndUnit), nameof(this.OutputRangeAndUnit),
                new string[] { RangeAndUnit.RangeDefault, RangeAndUnit.RangePercentAsc, RangeAndUnit.RangePercentDesc },
                RangeAndUnit.RangePercentAsc);
            this.OutputRangeAndUnit.ValueSet += this.OutputRangeAndUnit_ValueSet;

            this.NodeExecuting = this.TypeService.CreateBool(PortTypes.Binary, nameof(this.NodeExecuting), false);
            this.NodeError = this.TypeService.CreateString(PortTypes.String, nameof(this.NodeError), String.Empty);

            // initialize range dependent ports
            this.SetupRangeDependentPorts(this.OutputRangeAndUnit.Value);

        }

        /// <summary>
        ///   Reacts to range and value selection changes.</summary>
        /// <param name="sender">
        ///   The sender.</param>
        /// <param name="args">
        ///   The arguments.</param>
        private void OutputRangeAndUnit_ValueSet(Object sender, ValueChangedEventArgs args) {

            if (!String.Equals(args.NewValue, args.OldValue))
                this.SetupRangeDependentPorts(args.NewValue);

        }

        /// <summary>
        ///   Setup ports accoring to the selected range and unit.</summary>
        /// <param name="scope">
        ///   The range and unit.</param>
        private void SetupRangeDependentPorts(Object rangeAndUnit) {

            switch (rangeAndUnit) {

                case RangeAndUnit.RangeDefault:

                    if (this.NodeCurrentInteger == null)
                        this.NodeCurrentInteger = this.TypeService.CreateInt(PortTypes.Integer, nameof(this.NodeCurrentInteger), 0);

                    if (this.NodeTargetInteger == null)
                        this.NodeTargetInteger = this.TypeService.CreateInt(PortTypes.Integer, nameof(this.NodeTargetInteger), 0);

                    this.NodeCurrentPercent = null;
                    this.NodeTargetPercent = null;

                    break;

                case RangeAndUnit.RangePercentAsc:
                case RangeAndUnit.RangePercentDesc:

                    if (this.NodeCurrentPercent == null)
                        this.NodeCurrentPercent = this.TypeService.CreateDouble(PortTypes.Percent, nameof(this.NodeCurrentPercent), 0);

                    if (this.NodeTargetPercent == null)
                        this.NodeTargetPercent = this.TypeService.CreateDouble(PortTypes.Percent, nameof(this.NodeTargetPercent), 0);

                    this.NodeCurrentInteger = null;
                    this.NodeTargetInteger = null;

                    break;

                default:
                    this.NodeCurrentInteger = null;
                    this.NodeTargetInteger = null;
                    this.NodeCurrentPercent = null;
                    this.NodeTargetPercent = null;
                    break;

            }

        }

        /// <summary>
        ///   Sets the component's initial state.</summary>
        public override void Startup() {

            this.SetupFilter(this.NodeIdentifier.Value);

        }

        /// <summary>
        ///   React on incoming telegrams.</summary>
        public override void Execute() {

            if (this.NodeIdentifier.WasSet)
                this.SetupFilter(this.NodeIdentifier.Value);

            if (this.GatewayResponse.WasSet)
                this.HandleResponse(this.GatewayResponse.Value);

        }

        /// <summary>
        ///   Configures the filter for incoming messages.</summary>
        /// <param name="filter">
        ///   Filter configuration.</param>
        private void SetupFilter(String filter) {

            // reset message filter properties
            this.MessageFilter = Guid.NewGuid().ToString();

            // skip extraction for empty filters
            if (String.IsNullOrWhiteSpace(filter))
                return;

            // prefix filter when neccessary
            if (filter[0] == ':' || filter[0] == '/')
                this.MessageFilter = String.Format("{0}://{1}{2}", Klf200TelegramMode.Response, Klf200TelegramScope.Node, filter);
            else
                this.MessageFilter = String.Format("{0}://{1}/{2}", Klf200TelegramMode.Response, Klf200TelegramScope.Node, filter);

        }

        /// <summary>
        ///   Evaluates incoming messages and tries to handle them as telegrams to map the values to the output ports.</summary>
        /// <param name="message">
        ///   The message to be treated as a telegram.</param>
        private void HandleResponse(String message) {

            // stop processing if message does not pass the filter
            if (String.IsNullOrWhiteSpace(message) || !message.StartsWith(this.MessageFilter, StringComparison.OrdinalIgnoreCase))
                return;

            // reset error
            this.NodeError.Value = String.Empty;

            // parse the message into a telegram and extract the properties
            Klf200Telegram telegram = Klf200Telegram.Parse(message);
            foreach (KeyValuePair<Klf200TelegramParameter, String> parameter in telegram.Parameters) {

                switch (parameter.Key) {

                    case Klf200TelegramParameter.Current:
                        this.SetValue(this.NodeCurrentInteger, this.NodeCurrentPercent, parameter.Value, OutputRangeAndUnit.Value);
                        break;

                    case Klf200TelegramParameter.Target:
                        this.SetValue(this.NodeTargetInteger, this.NodeTargetPercent, parameter.Value, OutputRangeAndUnit.Value);
                        break;

                    case Klf200TelegramParameter.State:
                        this.SetValue(this.NodeExecuting, String.Equals(GW_NodeState.Executing.ToString(), parameter.Value, StringComparison.OrdinalIgnoreCase));
                        break;

                    case Klf200TelegramParameter.Error:
                        this.NodeError.Value = parameter.Value;
                        break;

                }

            }

        }

        /// <summary>
        ///   Converts the <paramref name="value"/> into the value range specified by <paramref name="range"/>.</summary>
        /// <param name="integerProperty">
        ///   A port to handle an integer property.</param>
        /// <param name="percentProperty">
        ///   A port to handle a double/percent property.</param>
        /// <param name="value">
        ///   The new value.</param>
        /// <param name="range">
        ///   The range.</param>
        private void SetValue(IntValueObject integerProperty, DoubleValueObject percentProperty, String value, String range) {

            // validate passed number
            if (String.IsNullOrWhiteSpace(value) || !Int32.TryParse(value, out Int32 valueAsInt))
                return;

            // ignore values outside of safe range (e.g. #stop or unknown)
            if (valueAsInt < 0 || valueAsInt > 51200)
                return;

            // map value to value range and unit
            switch (range) {

                case RangeAndUnit.RangePercentDesc:

                    if (percentProperty == null)
                        return;

                    Int32 oldValueDesc = percentProperty.HasValue ? (Int32)percentProperty.Value : Int32.MinValue;
                    Int32 newValueDesc = 100 - this.Round(valueAsInt);

                    if (oldValueDesc != newValueDesc)
                        percentProperty.Value = newValueDesc;

                    return;

                case RangeAndUnit.RangePercentAsc:

                    if (percentProperty == null)
                        return;

                    Int32 oldValueAsc = percentProperty.HasValue ? (Int32)percentProperty.Value : Int32.MinValue;
                    Int32 newValueAsc = this.Round(valueAsInt);

                    if (oldValueAsc != newValueAsc)
                        percentProperty.Value = newValueAsc;

                    return;

                case RangeAndUnit.RangeDefault:

                    if (integerProperty == null)
                        return;

                    if (!integerProperty.HasValue || integerProperty.Value != valueAsInt)
                        integerProperty.Value = valueAsInt;

                    return;

                default:
                    return;

            }

        }

        /// <summary>
        ///   Sets the value if it's different to the current value of the port.</summary>
        /// <param name="booleanProperty">
        ///   A port to handle a boolean property.</param>
        /// <param name="value">
        ///   The new value.</param>
        private void SetValue(BoolValueObject booleanProperty, Boolean value) {

            if (!booleanProperty.HasValue || booleanProperty.Value != value)
                booleanProperty.Value = value;

        }

        /// <summary>
        ///   Custom round implementation to map a full-range-value (0..51200) to a percent-range-value (0..100).</summary>
        /// <param name="fullRangeValue">
        ///   The full-range-value.</param>
        /// <returns>
        ///   A percent-range-value.</returns>
        private Int32 Round(Int32 fullRangeValue) {

            // mono's Math.Round() rounds down too often, so a custom implementation is needed

            Int32 quotient = fullRangeValue / 512;
            Int32 remainder = fullRangeValue % 512;

            if (remainder > 256)
                return quotient + 1;
            else
                return quotient;

        }

    }

}
