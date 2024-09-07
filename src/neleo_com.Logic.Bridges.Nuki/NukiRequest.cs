using System;

using LogicModule.Nodes.Helpers;
using LogicModule.ObjectModel;
using LogicModule.ObjectModel.TypeSystem;

using neleo_com.Logic.Bridges.Nuki.Definitions;

namespace neleo_com.Logic.Bridges.Nuki {

    /// <summary>
    ///   A request builder to emulate Nuki devices.</summary>
    public class NukiRequest : LogicNodeBase {

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
        [Input(DisplayOrder = 15, IsDefaultShown = false)]
        public BoolValueObject InfoAction {
            get; private set;
        }

        /// <summary>
        ///   Publishes translated requests to the gateway.</summary>
        [Output(DisplayOrder = 1, IsDefaultShown = true)]
        public StringValueObject GatewayRequest {
            get; private set;
        }

        /// <summary>
        ///   Initializes the Nuki Bridge Request Generator.</summary>
        /// <param name="context">
        ///   Context of the node instance to connect to services.</param>
        public NukiRequest(INodeContext context) {

            // ensure that the context is set
            context.ThrowIfNull(nameof(context));

            // initialize service
            this.TypeService = context.GetService<ITypeService>();

            // initialize ports
            this.DeviceId = this.TypeService.CreateString(PortTypes.String, nameof(this.DeviceId), String.Empty);
            this.DeviceType = this.TypeService.CreateInt(PortTypes.Integer, nameof(this.DeviceType), 0);

            this.LockAction = this.TypeService.CreateBool(PortTypes.Binary, nameof(this.LockAction), false);
            this.UnlockAction = this.TypeService.CreateBool(PortTypes.Binary, nameof(this.UnlockAction), false);
            this.UnlatchAction = this.TypeService.CreateBool(PortTypes.Binary, nameof(this.UnlatchAction), false);
            this.InfoAction = this.TypeService.CreateBool(PortTypes.Binary, nameof(this.InfoAction), false);

            this.GatewayRequest = this.TypeService.CreateString(PortTypes.String, nameof(this.GatewayRequest), String.Empty);

        }

        /// <summary>
        ///    Evaluates incoming values and executes the logic.</summary>
        public override void Execute() {

            if (this.LockAction.WasSet && this.LockAction.HasValue && this.LockAction.Value == true)
                this.RequestAction(NukiActionType.Lock);

            else if (this.UnlockAction.WasSet && this.UnlockAction.HasValue && this.UnlockAction.Value == true)
                this.RequestAction(NukiActionType.Unlock);

            else if (this.UnlatchAction.WasSet && this.UnlatchAction.HasValue && this.UnlatchAction.Value == true)
                this.RequestAction(NukiActionType.Unlatch);

            else if (this.InfoAction.WasSet && this.InfoAction.HasValue && this.InfoAction.Value == true)
                this.RequestAction(NukiActionType.Info);

        }

        /// <summary>
        ///   Compose telegram and pass it to the gateway.</summary>
        /// <param name="action">
        ///   The requested action.</param>
        private void RequestAction(NukiActionType action) {

            // compose telegram
            NukiTelegram telegram = new NukiTelegram() {
                Mode = NukiTelegramMode.Request,
                DeviceId = this.DeviceId,
                DeviceType = this.DeviceType,
                Action = action
            };

            // pass it to the output port
            if (this.GatewayRequest != null)
                this.GatewayRequest.Value = telegram.ToString();

        }

    }

}
