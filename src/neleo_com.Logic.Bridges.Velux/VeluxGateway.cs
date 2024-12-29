using System;
using System.Net;

using LogicModule.Nodes.Helpers;
using LogicModule.ObjectModel;
using LogicModule.ObjectModel.TypeSystem;

namespace neleo_com.Logic.Bridges.Velux {

    /// <summary>
    ///   Definition of gateway connection state.</summary>
    public static class State {

        public const String Disconnecting = nameof(State.Disconnecting);
        public const String Disconnected = nameof(State.Disconnected); // default
        public const String Connecting = nameof(State.Connecting);
        public const String Connected = nameof(State.Connected);

    }


    /// <summary>
    ///   Manages the connection to a Velux KLF-200 gateway.</summary>
    public class VeluxGateway : LogicNodeBase, IDisposable {

        /// <summary>
        ///   The Type Service manages incoming and outgoing ports.</summary>
        private readonly ITypeService TypeService;

        /// <summary>
        ///   The Scheduler Service manages access to the clock and scheduled callbacks.</summary>
        private readonly ISchedulerService SchedulerService;

        /// <summary>
        ///   (1) Interval to wait before reconnecting after a reboot. 
        ///   (2) Grace period for the <see cref="HeartbeatInterval"/> to detect lost connections.</summary>
        private readonly TimeSpan GracePeriod = TimeSpan.FromMinutes(1);

        /// <summary>
        ///   Interval to notify the gateway to keep the connection alive.</summary>
        private readonly TimeSpan HeartbeatInterval = TimeSpan.FromMinutes(7);

        /// <summary>
        ///   Heardbeat token from the scheduling service.</summary>
        private SchedulerToken HeartbeatToken = null;

        /// <summary>
        ///   UTC date and time of the last recognized signal from the KLF-200.</summary>
        private DateTime HeartbeatTimeStamp = DateTime.MinValue;

        /// <summary>
        ///   Implementation of the Velux KLF 200 Gateway.</summary>
        private Klf200Gateway Gateway = null;

        /// <summary>
        ///   Receives incoming requests to be translated by this class and processed by the Velux gateway.</summary>
        [Input(DisplayOrder = 1, IsDefaultShown = true, IsInput = true)]
        public StringValueObject GatewayRequest {
            get; private set;
        }

        /// <summary>
        ///   The IP address of the Velux gateway.</summary>
        [Parameter(DisplayOrder = 2, IsDefaultShown = true)]
        public StringValueObject GatewayHost {
            get; private set;
        }

        /// <summary>
        ///   The password to authenticate/authorize a connection to the Velux gateway.</summary>
        [Parameter(DisplayOrder = 3, IsDefaultShown = false)]
        public StringValueObject GatewayPassword {
            get; private set;
        }

        /// <summary>
        ///   Request the state of all nodes.</summary>
        [Input(DisplayOrder = 4, IsDefaultShown = false, IsInput = true)]
        public BoolValueObject RequestState {
            get; private set;
        }

        /// <summary>
        ///   Request to reconnect the underlaying gateway / socket.</summary>
        [Input(DisplayOrder = 5, IsDefaultShown = false, IsInput = true)]
        public BoolValueObject RequestReconnect {
            get; private set;
        }

        /// <summary>
        ///   Request to reboot the Velux KLF-200.</summary>
        [Input(DisplayOrder = 6, IsDefaultShown = false, IsInput = true)]
        public BoolValueObject RequestReboot {
            get; private set;
        }

        /// <summary>
        ///   Publishes translated gateway responses.</summary>
        [Output(DisplayOrder = 1, IsDefaultShown = true)]
        public StringValueObject GatewayResponse {
            get; private set;
        }

        /// <summary>
        ///   Indicates that the state of the Velux KLF-200 connection.</summary>
        [Output(DisplayOrder = 2, IsDefaultShown = true)]
        public StringValueObject GatewayState {
            get; private set;
        }

        /// <summary>
        ///   Publishes internal errors.</summary>
        [Output(DisplayOrder = 3, IsDefaultShown = false)]
        public StringValueObject GatewayError {
            get; private set;
        }

        /// <summary>
        ///   Indicates whether this instance has been disposed.</summary>
        private Boolean IsDisposed = false;

        /// <summary>
        ///   Initializes the Velux Gateway connector.</summary>
        /// <param name="context">
        ///   Context of the node instance to connect to services.</param>
        public VeluxGateway(INodeContext context) {

            // ensure that the context is set
            context.ThrowIfNull(nameof(context));

            // initialize services
            this.TypeService = context.GetService<ITypeService>();
            this.SchedulerService = context.GetService<ISchedulerService>();

            // initialize ports
            this.GatewayRequest = this.TypeService.CreateString(PortTypes.String, nameof(this.GatewayRequest), String.Empty);
            this.GatewayHost = this.TypeService.CreateString(PortTypes.String, nameof(this.GatewayHost), String.Empty);
            this.GatewayPassword = this.TypeService.CreateString(PortTypes.String, nameof(this.GatewayPassword), String.Empty);
            this.RequestState = this.TypeService.CreateBool(PortTypes.Binary, nameof(this.RequestState), false);
            this.RequestReconnect = this.TypeService.CreateBool(PortTypes.Binary, nameof(this.RequestReconnect), false);
            this.RequestReboot = this.TypeService.CreateBool(PortTypes.Binary, nameof(this.RequestReboot), false);
            this.GatewayResponse = this.TypeService.CreateString(PortTypes.String, nameof(this.GatewayResponse), String.Empty);
            this.GatewayState = this.TypeService.CreateString(PortTypes.String, nameof(this.GatewayState), State.Disconnected);
            this.GatewayError = this.TypeService.CreateString(PortTypes.String, nameof(this.GatewayError), String.Empty);

        }

        /// <summary>
        ///   Try to establish a connection to the Velux Gateway.</summary>
        public override void Startup() {

            this.Connect();

        }

        /// <summary>
        ///   React on incoming telegrams.</summary>
        public override void Execute() {

            if (this.Gateway == null) {

                if (this.GatewayHost.WasSet || this.GatewayPassword.WasSet)
                    this.Reconnect();

            }
            else {

                if (this.GatewayRequest.WasSet)
                    this.Gateway.SendRequest(Klf200Telegram.Parse(this.GatewayRequest.Value));

                else if (this.RequestState.WasSet && this.RequestState.Value)
                    this.Gateway.RequestState();

                else if (this.RequestReconnect.WasSet && this.RequestReconnect.Value)
                    this.Reconnect();

                else if (this.RequestReboot.WasSet && this.RequestReboot.Value)
                    this.Reboot();

            }

        }

        /// <summary>
        ///   Try to setup a new connection to a Velux KLF-200 gateway.</summary>
        private void Connect() {

            // setup a new Velux Gateway connection
            try {

                if (this.GatewayHost.HasValue && !String.IsNullOrWhiteSpace(this.GatewayHost.Value)
                    && this.GatewayPassword.HasValue && !String.IsNullOrWhiteSpace(this.GatewayPassword.Value)) {

                    // set gateway state to "Connecting"
                    this.GatewayState.Value = State.Connecting;

                    // clear error port
                    if (!String.IsNullOrWhiteSpace(this.GatewayError.Value))
                        this.GatewayError.Value = String.Empty;

                    // establish a connection
                    IPAddress host = IPAddress.Parse(this.GatewayHost.Value);
                    this.Gateway = new Klf200Gateway(host, this.GatewayPassword.Value);

                    // register handlers
                    this.Gateway.OnHeartbeat += (sender, e) => {
                        this.HeartbeatTimeStamp = DateTime.UtcNow;
                    };

                    this.Gateway.OnInfo += (sender, message) => {
                        if (this.GatewayResponse != null)
                            this.GatewayResponse.Value = message;
                    };

                    this.Gateway.OnError += (sender, message) => {
                        if (!String.IsNullOrWhiteSpace(message) && message.StartsWith(ErrorCodes.SocketIoException))
                            this.Reconnect();

                        if (this.GatewayError != null)
                            this.GatewayError.Value = message;
                    };

                    this.Gateway.OnResponse += (sender, response) => {
                        if (this.GatewayResponse != null)
                            this.GatewayResponse.Value = response.ToString();
                    };

                    // set date and time
                    this.Gateway.SetClock(this.SchedulerService.Now.ToUniversalTime());

                    // start the heartbeat
                    this.HeartbeatToken = this.SchedulerService.InvokeIn(this.HeartbeatInterval, this.KeepAlive);

                    // set gateway state to "Connected"
                    this.GatewayState.Value = State.Connected;

                }

            }
            catch (WebException ex) {

                this.GatewayState.Value = State.Disconnected;

                if (this.GatewayError != null)
                    this.GatewayError.Value = ex.Message;

            }
            catch (Exception ex) {

                this.GatewayState.Value = State.Disconnected;

                if (this.GatewayError != null)
                    this.GatewayError.Value = String.Format("{0}: {1}", ex.GetType().Name, ex.Message);

            }

        }

        /// <summary>
        ///   Requests the Velux KLF-200 to reboot, waits for a minute and tries to reconnect.</summary>
        private void Reboot() {

            // update connection status
            if (this.Gateway != null)
                this.GatewayState.Value = State.Disconnecting;

            // request to reboot
            // (this request can fail if the connection has been lost)
            this.Gateway.RequestReboot();

            // stop heartbeat
            if (this.HeartbeatToken != null) {

                this.SchedulerService.Remove(this.HeartbeatToken);
                this.HeartbeatToken = null;

            }

            // close connection and destroy object
            if (this.Gateway != null) {

                this.Gateway.Dispose();
                this.Gateway = null;

            }

            // update connection status
            if (this.Gateway == null)
                this.GatewayState.Value = State.Disconnected;

            // use the heartbeat-toekn to reconnect
            // (if a user reconnects manually in between, the token will be invalidated by "Reconnect" function)
            this.HeartbeatToken = this.SchedulerService.InvokeIn(this.GracePeriod, this.Reconnect);

        }

        /// <summary>
        ///   Reconnects to the Velux Gateway if the gateway properties have changed.</summary>
        private void Reconnect() {

            // update connection status
            if (this.Gateway != null)
                this.GatewayState.Value = State.Disconnecting;

            // stop heartbeat
            if (this.HeartbeatToken != null) {

                this.SchedulerService.Remove(this.HeartbeatToken);
                this.HeartbeatToken = null;

            }

            // close connection and destroy object
            if (this.Gateway != null) {

                this.Gateway.Dispose();
                this.Gateway = null;

            }

            // update connection status
            if (this.Gateway == null)
                this.GatewayState.Value = State.Disconnected;

            // register a new connection
            this.Connect();

        }

        /// <summary>
        ///   Sends a heartbeat signal to the Velux Gateway to keep the connection alive.</summary>
        private void KeepAlive() {

            // abort if the gateway has not been initialized
            if (this.Gateway == null)
                return;

            // detect if the connection is still alive
            if (this.HeartbeatTimeStamp.Add(this.HeartbeatInterval).Add(this.GracePeriod) < DateTime.UtcNow) {

                // try to reestablish a connection
                this.Reconnect();

            }
            else {

                // send heartbeat to the Velux Gateway
                this.Gateway.KeepAlive();

                // request next heartbeat
                this.HeartbeatToken = this.SchedulerService.InvokeIn(this.HeartbeatInterval, this.KeepAlive);

            }

        }

        /// <summary>
        ///   Closes the connection to the Velux KLF-200 gateway.</summary>
        public void Dispose() {

            this.Dispose(true);

        }

        /// <summary>
        ///   Closes the connection to the Velux KLF-200 gateway.</summary>
        /// <param name="disposing">
        ///   <c>true</c> when called from <see cref="Dispose"/>; <c>false</c> when called from finalizer.</param>
        protected virtual void Dispose(Boolean disposing) {

            if (this.IsDisposed)
                return;

            if (this.Gateway != null)
                this.Gateway.Dispose();

            this.IsDisposed = true;

        }

    }

}
