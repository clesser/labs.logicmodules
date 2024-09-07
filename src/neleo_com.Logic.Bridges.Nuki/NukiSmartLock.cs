using System;

using LogicModule.ObjectModel;
using LogicModule.ObjectModel.TypeSystem;

using neleo_com.Logic.Bridges.Nuki.Datagrams;
using neleo_com.Logic.Bridges.Nuki.Definitions;

namespace neleo_com.Logic.Bridges.Nuki {

    /// <summary>
    ///   A gateway to a Nuki Smart Lock using the Nuki Bridge HTTP API.</summary>
    public class NukiSmartLock : NukiBridge {

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
        ///   Allows users to lock (true) the door lock.</summary>
        [Input(DisplayOrder = 12, IsInput = true, IsDefaultShown = true)]
        public BoolValueObject LockAction {
            get; private set;
        }

        /// <summary>
        ///   Allows users to unlock (true) the door lock.</summary>
        [Input(DisplayOrder = 13, IsInput = true, IsDefaultShown = true)]
        public BoolValueObject UnlockAction {
            get; private set;
        }

        /// <summary>
        ///   Allows users to unlatch (true) the door lock.</summary>
        [Input(DisplayOrder = 14, IsInput = true, IsDefaultShown = true)]
        public BoolValueObject UnlatchAction {
            get; private set;
        }

        /// <summary>
        ///   Requests the current (non-cached) state of a device.</summary>
        [Input(DisplayOrder = 15, IsRequired = false)]
        public BoolValueObject InfoAction {
            get; private set;
        }

        /// <summary>
        ///   Requests the Nuki Bridge to publish the cached state of all connected devices.</summary>
        [Input(DisplayOrder = 16, IsDefaultShown = true, IsInput = true)]
        public BoolValueObject StateRequest {
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
        ///   Initializes a new Nuki SmartLock that is connected to the Nuki Bridge.</summary>
        /// <param name="context">
        ///   Context of the node instance to connect to services.</param>
        public NukiSmartLock(INodeContext context) : base(context) {

            // initialize ports
            this.DeviceId = this.TypeService.CreateString(PortTypes.String, nameof(this.DeviceId), String.Empty);
            this.DeviceType = this.TypeService.CreateInt(PortTypes.Integer, nameof(this.DeviceType), 0);

            this.LockAction = this.TypeService.CreateBool(PortTypes.Binary, nameof(this.LockAction), false);
            this.UnlockAction = this.TypeService.CreateBool(PortTypes.Binary, nameof(this.UnlockAction), false);
            this.UnlatchAction = this.TypeService.CreateBool(PortTypes.Binary, nameof(this.UnlatchAction), false);
            this.InfoAction = this.TypeService.CreateBool(PortTypes.Binary, nameof(this.InfoAction), false);
            this.StateRequest = this.TypeService.CreateBool(PortTypes.Binary, nameof(this.StateRequest), false);

            this.LockState = this.TypeService.CreateInt(PortTypes.Integer, nameof(this.LockState), 0);
            this.DoorState = this.TypeService.CreateInt(PortTypes.Integer, nameof(this.DoorState), 0);
            this.BatteryState = this.TypeService.CreateInt(PortTypes.Integer, nameof(this.BatteryState), 0);

        }

        /// <summary>
        ///    Evaluates incoming values and executes the logic.</summary>
        public override void Execute() {

            // abort if shared values aren't initialized properly
            if (!this.SharedPortValuesInitialized())
                return;

            if (this.LockAction.WasSet && this.LockAction.HasValue && this.LockAction.Value == true)
                this.HandleLockActionCommand(this.DeviceId.Value, this.DeviceType.Value, NukiActionType.Lock);

            else if (this.UnlockAction.WasSet && this.UnlockAction.HasValue && this.UnlockAction.Value == true)
                this.HandleLockActionCommand(this.DeviceId.Value, this.DeviceType.Value, NukiActionType.Unlock);

            else if (this.UnlatchAction.WasSet && this.UnlatchAction.HasValue && this.UnlatchAction.Value == true)
                this.HandleLockActionCommand(this.DeviceId.Value, this.DeviceType.Value, NukiActionType.Unlatch);

            else if (this.InfoAction.WasSet && this.InfoAction.HasValue && this.InfoAction.Value == true)
                this.HandleLockStateCommand(this.DeviceId.Value, this.DeviceType.Value);

            else if (this.StateRequest.WasSet && this.StateRequest.HasValue && this.StateRequest.Value == true)
                this.HandleListCommand(this.DeviceId.Value, this.DeviceType.Value);

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

            if (state.Success) {

                switch (action) {

                    case NukiActionType.Lock:
                        if (this.LockState != null)
                            this.LockState.Value = 1; // locked
                        return;

                    case NukiActionType.Unlock:
                        if (this.LockState != null)
                            this.LockState.Value = 3; // unlocked
                        return;

                    case NukiActionType.Unlatch:
                        if (this.LockState != null)
                            this.LockState.Value = 5; // unlatched
                        return;

                }

            }

        }

        /// <summary>
        ///   Requests the state for the specified deviceand passes the result to the output port.</summary>
        /// <param name="deviceId">
        ///   The Nuki device identifier.</param>
        /// <param name="deviceType">
        ///   The Nuki device type.</param>
        private void HandleLockStateCommand(String deviceId, Int32 deviceType) {

            NukiDeviceState state = this.ProcessLockStateCommand(deviceId, deviceType);

            if (state.Success) {

                this.LockState.Value = state.LockState;
                this.DoorState.Value = state.DoorState;
                this.BatteryState.Value = state.BatteryState;

            }

        }

        /// <summary>
        ///   Requests the state for all devices that are connected to the Nuki Bridge 
        ///   and passes the result to the output port.</summary>
        /// <param name="deviceId">
        ///   The Nuki device identifier.</param>
        /// <param name="deviceType">
        ///   The Nuki device type.</param>
        private void HandleListCommand(String deviceId, Int32 deviceType) {

            NukiDeviceInfo[] deviceInfos = this.ProcessListCommand();

            foreach (NukiDeviceInfo info in deviceInfos) {

                if (info.DeviceId == deviceId && info.DeviceType == deviceType) {

                    this.LockState.Value = info.DeviceState.LockState;
                    this.DoorState.Value = info.DeviceState.DoorState;
                    this.BatteryState.Value = info.DeviceState.BatteryState;

                    return;

                }

            }

        }

    }

}
