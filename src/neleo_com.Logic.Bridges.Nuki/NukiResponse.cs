using System;
using System.Collections.Generic;

using LogicModule.Nodes.Helpers;
using LogicModule.ObjectModel;
using LogicModule.ObjectModel.TypeSystem;

using neleo_com.Logic.Bridges.Nuki.Definitions;

namespace neleo_com.Logic.Bridges.Nuki {

    /// <summary>
    ///   A response evaluator to emulate Nuki devices.</summary>
    public class NukiResponse : LogicNodeBase {

        /// <summary>
        ///   The Type Service manages incoming and outgoing ports.</summary>
        private readonly ITypeService TypeService;

        /// <summary>
        ///   The device identifier routes the request to a specific device that is connected to the bridge.</summary>
        [Parameter(DisplayOrder = 10, IsDefaultShown = false)]
        public StringValueObject DeviceId {
            get; private set;
        }

        /// <summary>
        ///   The type of the Nuki device.</summary>
        [Parameter(DisplayOrder = 11, IsDefaultShown = false)]
        public IntValueObject DeviceType {
            get; private set;
        }

        /// <summary>
        ///   Receives gateway responses.</summary>
        [Input(DisplayOrder = 12, IsDefaultShown = true, IsInput = true)]
        public StringValueObject GatewayResponse {
            get; private set;
        }

        /// <summary>
        ///   Makes the state of the door lock available.</summary>
        [Output(DisplayOrder = 1, IsDefaultShown = true)]
        public IntValueObject LockState {
            get; private set;
        }

        /// <summary>
        ///   Makes the state of the door available.</summary>
        [Output(DisplayOrder = 2, IsDefaultShown = true)]
        public IntValueObject DoorState {
            get; private set;
        }

        /// <summary>
        ///   Notifies users about the battery state of the smart lock.</summary>
        [Output(DisplayOrder = 3, IsDefaultShown = true)]
        public IntValueObject BatteryState {
            get; private set;
        }

        /// <summary>
        ///   The filter for incoming messages.</summary>
        private String MessageFilter = Guid.NewGuid().ToString();

        /// <summary>
        ///   Initializes the Nuki Bridge Response Filter.</summary>
        /// <param name="context">
        ///   Context of the node instance to connect to services.</param>
        public NukiResponse(INodeContext context) {

            // ensure that the context is set
            context.ThrowIfNull(nameof(context));

            // initialize services
            this.TypeService = context.GetService<ITypeService>();

            // initialize ports
            this.DeviceId = this.TypeService.CreateString(PortTypes.String, nameof(this.DeviceId), String.Empty);
            this.DeviceType = this.TypeService.CreateInt(PortTypes.Integer, nameof(this.DeviceType), 0);
            this.GatewayResponse = this.TypeService.CreateString(PortTypes.String, nameof(this.GatewayResponse), String.Empty);

            this.LockState = this.TypeService.CreateInt(PortTypes.Integer, nameof(this.LockState), 0);
            this.DoorState = this.TypeService.CreateInt(PortTypes.Integer, nameof(this.DoorState), 0);
            this.BatteryState = this.TypeService.CreateInt(PortTypes.Integer, nameof(this.BatteryState), 0);

        }

        /// <summary>
        ///   Sets the component's initial state.</summary>
        public override void Startup() {

            this.SetupFilter();

        }

        /// <summary>
        ///   React on incoming telegrams.</summary>
        public override void Execute() {

            if (this.DeviceId.WasSet || this.DeviceType.WasSet)
                this.SetupFilter();

            if (this.GatewayResponse.WasSet)
                this.HandleResponse(this.GatewayResponse.Value);

        }

        /// <summary>
        ///   Configures the filter for incoming messages.</summary>
        private void SetupFilter() {

            // evaluate input ports
            String deviceId = (this.DeviceId != null && this.DeviceId.HasValue) ? this.DeviceId.Value : String.Empty;
            Int32 deviceType = (this.DeviceType != null && this.DeviceType.HasValue) ? this.DeviceType.Value : 0;

            // set message filter properties
            this.MessageFilter = String.Format("{0}://{1}:{2}/", NukiTelegramMode.Response.ToString().ToLowerInvariant(),
                deviceId, deviceType);

        }

        /// <summary>
        ///   Evaluates incoming messages and tries to handle them as telegrams to map the values to the output ports.</summary>
        /// <param name="message">
        ///   The message to be treated as a telegram.</param>
        private void HandleResponse(String message) {

            // stop processing if message does not pass the filter
            if (String.IsNullOrWhiteSpace(message) || !message.StartsWith(this.MessageFilter, StringComparison.OrdinalIgnoreCase))
                return;

            // parse the message into a telegram and extract the properties
            NukiTelegram telegram = NukiTelegram.Parse(message);
            foreach (KeyValuePair<NukiTelegramParameter, String> parameter in telegram.Parameters) {

                switch (parameter.Key) {

                    case NukiTelegramParameter.Success:
                        if (String.Equals(parameter.Value, "true", StringComparison.InvariantCultureIgnoreCase)) {

                            switch (telegram.Action) {

                                case NukiActionType.Lock:
                                    this.ResolveResponse(this.LockState, 1); // locked
                                    break;

                                case NukiActionType.Unlock:
                                    this.ResolveResponse(this.LockState, 3); // unlocked
                                    break;

                                case NukiActionType.Unlatch:
                                    this.ResolveResponse(this.LockState, 5); // unlatched
                                    break;

                            }

                        }
                        break;

                    case NukiTelegramParameter.Lock:
                        this.ResolveResponse(this.LockState, parameter.Value);
                        break;

                    case NukiTelegramParameter.Door:
                        this.ResolveResponse(this.DoorState, parameter.Value);
                        break;

                    case NukiTelegramParameter.Battery:
                        this.ResolveResponse(this.BatteryState, parameter.Value);
                        break;

                }

            }

        }

        /// <summary>
        ///   Tries to set a value on an output port.</summary>
        /// <param name="port">
        ///   The output port.</param>
        /// <param name="value">
        ///   The value.</param>
        private void ResolveResponse(IntValueObject port, String value) {

            if (Int32.TryParse(value, out Int32 intValue))
                this.ResolveResponse(port, intValue);
        }

        /// <summary>
        ///   Tries to set a value on an output port.</summary>
        /// <param name="port">
        ///   The output port.</param>
        /// <param name="value">
        ///   The value.</param>
        private void ResolveResponse(IntValueObject port, Int32 value) {

            if (port != null)
                port.Value = value;
        }

    }

}
