using System;

using LogicModule.ObjectModel;
using LogicModule.ObjectModel.TypeSystem;

using neleo_com.Logic.Bridges.Nuki.Datagrams;
using neleo_com.Logic.Bridges.Nuki.Definitions;

namespace neleo_com.Logic.Bridges.Nuki {

    /// <summary>
    ///   A gateway to connect Nuki devices using the Nuki Bridge HTTP API.</summary>
    public class NukiGateway : NukiBridge {

        /// <summary>
        ///   Receives incoming requests to be translated by this class and processed by the Nuki Bridge.</summary>
        [Input(DisplayOrder = 10, IsDefaultShown = true, IsInput = true)]
        public StringValueObject GatewayRequest {
            get; private set;
        }

        /// <summary>
        ///   Requests the Nuki Bridge to publish the cached state of all connected devices.</summary>
        [Input(DisplayOrder = 11, IsDefaultShown = true, IsInput = true)]
        public BoolValueObject StateRequest {
            get; private set;
        }

        /// <summary>
        ///   Publishes translated Nuki Bridge responses.</summary>
        [Output(DisplayOrder = 1, IsDefaultShown = true)]
        public StringValueObject GatewayResponse {
            get; private set;
        }

        /// <summary>
        ///   Initializes the Nuki Bridge Gateway.</summary>
        /// <param name="context">
        ///   Context of the node instance to connect to services.</param>
        public NukiGateway(INodeContext context) : base(context) {

            this.GatewayRequest = this.TypeService.CreateString(PortTypes.String, nameof(this.GatewayRequest), String.Empty);
            this.StateRequest = this.TypeService.CreateBool(PortTypes.Binary, nameof(this.StateRequest), false);
            this.GatewayResponse = this.TypeService.CreateString(PortTypes.String, nameof(this.GatewayResponse), String.Empty);

        }

        /// <summary>
        ///   React on incoming telegrams.</summary>
        public override void Execute() {

            // abort if shared values aren't initialized properly
            if (!this.SharedPortValuesInitialized())
                return;

            // handle gateway requests 
            if (this.GatewayRequest.WasSet) {

                NukiTelegram telegram = NukiTelegram.Parse(this.GatewayRequest.Value);
                if (telegram.Mode == NukiTelegramMode.Request) {

                    switch (telegram.Action) {

                        case NukiActionType.Lock:
                        case NukiActionType.Unlock:
                        case NukiActionType.Unlatch:
                            this.HandleLockActionCommand(telegram.DeviceId, telegram.DeviceType, telegram.Action);
                            break;

                        case NukiActionType.Info:
                            this.HandleLockStateCommand(telegram.DeviceId, telegram.DeviceType);
                            break;

                        default:
                            break;

                    }

                }

            }

            // handle state requests
            if (this.StateRequest.WasSet && this.StateRequest.HasValue && this.StateRequest.Value) {

                this.HandleListCommand();

            }

        }

        /// <summary>
        ///   Request the specified device to perform an action and passes the result to the output port.</summary>
        /// <param name="deviceId">
        ///   The Nuki device identifier.</param>
        /// <param name="deviceType">
        ///   The Nuki device type.</param>
        /// <param name="action">
        ///   The requested action.</param>
        private void HandleLockActionCommand(String deviceId, Int32 deviceType, NukiActionType action) {

            NukiActionState state = this.ProcessLockActionCommand(deviceId, deviceType, action);

            NukiTelegram telegram = new NukiTelegram() {
                Mode = NukiTelegramMode.Response,
                DeviceId = deviceId,
                DeviceType = deviceType,
                Action = action
            };

            telegram.SetParameter(NukiTelegramParameter.Success, state.Success.ToString().ToLowerInvariant());

            if (this.GatewayResponse != null)
                this.GatewayResponse.Value = telegram.ToString();

       }

        /// <summary>
        ///   Requests the state for the specified deviceand passes the result to the output port.</summary>
        /// <param name="deviceId">
        ///   The Nuki device identifier.</param>
        /// <param name="deviceType">
        ///   The Nuki device type.</param>
        private void HandleLockStateCommand(String deviceId, Int32 deviceType) {

            NukiDeviceState state = this.ProcessLockStateCommand(deviceId, deviceType);

            NukiTelegram telegram = new NukiTelegram() {
                Mode = NukiTelegramMode.Response,
                DeviceId = deviceId,
                DeviceType = deviceType,
                Action = NukiActionType.Info
            };

            telegram.SetParameter(NukiTelegramParameter.Lock, state.LockState);
            telegram.SetParameter(NukiTelegramParameter.Door, state.DoorState);
            telegram.SetParameter(NukiTelegramParameter.Battery, state.BatteryState);

            if (this.GatewayResponse != null)
                this.GatewayResponse.Value = telegram.ToString();

        }

        /// <summary>
        ///   Requests the state for all devices that are connected to the Nuki Bridge 
        ///   and passes the result to the output port.</summary>
        private void HandleListCommand() {

            NukiDeviceInfo[] deviceInfos = this.ProcessListCommand();

            foreach (NukiDeviceInfo info in deviceInfos) {

                NukiTelegram telegram = new NukiTelegram() {
                    Mode = NukiTelegramMode.Response,
                    DeviceId = info.DeviceId,
                    DeviceType = info.DeviceType,
                    Action = NukiActionType.Info
                };

                telegram.SetParameter(NukiTelegramParameter.Lock, info.DeviceState.LockState);
                telegram.SetParameter(NukiTelegramParameter.Door, info.DeviceState.DoorState);
                telegram.SetParameter(NukiTelegramParameter.Battery, info.DeviceState.BatteryState);

                if (this.GatewayResponse != null)
                    this.GatewayResponse.Value = telegram.ToString();

            }

        }

    }

}
