using System;
using System.Diagnostics;
using System.Text;

using LogicModule.Nodes.Helpers;
using LogicModule.ObjectModel;
using LogicModule.ObjectModel.TypeSystem;

using neleo_com.Logic.Bridges.Nuki.Datagrams;
using neleo_com.Logic.Bridges.Nuki.Definitions;

using Newtonsoft.Json;

namespace neleo_com.Logic.Bridges.Nuki {

    /// <summary>
    ///   A response evaluator to emulate Nuki devices.</summary>
    public abstract class NukiBridge : LogicNodeBase {

        /// <summary>
        ///   The Type Service manages incoming and outgoing ports.</summary>
        protected readonly ITypeService TypeService;

        /// <summary>
        ///   Base address of the Nuki Bridge.</summary>
        [Parameter(DisplayOrder = 1, IsDefaultShown = false)]
        public StringValueObject BridgeAddress {
            get; private set;
        }

        /// <summary>
        ///   Communication port of the Nuki Bridge.</summary>
        protected String BridgePort = "8080";

        /// <summary>
        ///   The token is used to authenticate the logic module at a specific Nuki Bridge.</summary>
        [Parameter(DisplayOrder = 2, IsDefaultShown = false)]
        public StringValueObject BridgeToken {
            get; private set;
        }

        /// <summary>
        ///   Publishes internal errors.</summary>
        [Output(DisplayOrder = 10, IsDefaultShown = false)]
        public StringValueObject GatewayError {
            get; private set;
        }

        /// <summary>
        ///   Initializes the Nuki Bridge.</summary>
        /// <param name="context">
        ///   Context of the node instance to connect to services.</param>
        protected NukiBridge(INodeContext context) {

            // ensure that the context is set
            context.ThrowIfNull(nameof(context));

            // initialize services
            this.TypeService = context.GetService<ITypeService>();

            // initialize ports
            this.BridgeAddress = this.TypeService.CreateString(PortTypes.String, nameof(this.BridgeAddress), "127.0.0.1");
            this.BridgeToken = this.TypeService.CreateString(PortTypes.String, nameof(this.BridgeToken));

            this.GatewayError = this.TypeService.CreateString(PortTypes.String, nameof(this.GatewayError), String.Empty);

        }

        /// <summary>
        ///   Verifies that the shared ports are initialized properly.</summary>
        protected Boolean SharedPortValuesInitialized() {

            // ensure that bridge address and bridge token are set
            if (!this.BridgeAddress.HasValue || String.IsNullOrWhiteSpace(this.BridgeAddress.Value))
                return false;

            if (!this.BridgeToken.HasValue || String.IsNullOrWhiteSpace(this.BridgeToken.Value))
                return false;

            return true;

        }

        /// <summary>
        ///   Request the specified device to perform an action.</summary>
        /// <param name="deviceId">
        ///   The Nuki device identifier.</param>
        /// <param name="deviceType">
        ///   The Nuki device type.</param>
        /// <param name="action">
        ///   The requested action.</param>
        protected NukiActionState ProcessLockActionCommand(String deviceId, Int32 deviceType, NukiActionType action) {

            String request = String.Format("http://{0}:{1}/lockAction?nukiId={3}&deviceType={4}&action={5}&nowait=0&token={2}",
                this.BridgeAddress.Value, this.BridgePort, this.BridgeToken.Value, deviceId, deviceType, (Byte)action);

            String response = this.SendRequest(request);

            if (String.IsNullOrWhiteSpace(response))
                return new NukiActionState() { Success = false };

            try {

                return JsonConvert.DeserializeObject<NukiActionState>(response);

            }
            catch {

                return new NukiActionState() { Success = false };

            }

        }

        /// <summary>
        ///   Requests the state for the specified device.</summary>
        /// <param name="deviceId">
        ///   The Nuki device identifier.</param>
        /// <param name="deviceType">
        ///   The Nuki device type.</param>
        protected NukiDeviceState ProcessLockStateCommand(String deviceId, Int32 deviceType) {

            String request = String.Format("http://{0}:{1}/lockState?nukiId={3}&deviceType={4}&token={2}",
               this.BridgeAddress.Value, this.BridgePort, this.BridgeToken.Value, deviceId, deviceType);

            String response = this.SendRequest(request);

            if (String.IsNullOrWhiteSpace(response))
                return new NukiDeviceState() { Success = false };

            try {

                return JsonConvert.DeserializeObject<NukiDeviceState>(response);

            }
            catch {

                return new NukiDeviceState() { Success = false };

            }

        }

        /// <summary>
        ///   Requests the state for all devices that are connected to the Nuki Bridge.</summary>
        protected NukiDeviceInfo[] ProcessListCommand() {

            String request = String.Format("http://{0}:{1}/list?&token={2}",
                this.BridgeAddress.Value, this.BridgePort, this.BridgeToken.Value);

            String response = this.SendRequest(request);


            if (String.IsNullOrWhiteSpace(response))
                return new NukiDeviceInfo[0];

            try {

                return JsonConvert.DeserializeObject<NukiDeviceInfo[]>(response);

            }
            catch {

                return new NukiDeviceInfo[0];

            }

        }

        /// <summary>
        ///   Sends a request to the Nuki Bridge using "curl".</summary>
        /// <param name="request">
        ///   The request.</param>
        /// <returns>
        ///   The response.</returns>
        private String SendRequest(String request) {

            if (String.IsNullOrWhiteSpace(request))
                return String.Empty;

            try {

                Process process = new Process();
                process.StartInfo.FileName = "curl";
                process.StartInfo.Arguments = request;
                process.StartInfo.UseShellExecute = false;
                process.StartInfo.RedirectStandardOutput = true;
                process.StartInfo.RedirectStandardError = true;
                process.StartInfo.CreateNoWindow = true;
                process.StartInfo.StandardOutputEncoding = Encoding.UTF8;

                process.Start();
                String responseData = process.StandardOutput.ReadToEnd();
                String errorData = process.StandardError.ReadToEnd();
                process.WaitForExit();

                if (!String.IsNullOrWhiteSpace(errorData) && this.GatewayError != null)
                    this.GatewayError.Value = errorData;

                return responseData;

            }
            catch {

                return String.Empty;

            }

        }

    }

}
