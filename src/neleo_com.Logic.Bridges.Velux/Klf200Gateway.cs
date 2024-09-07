using System;
using System.Net;

using neleo_com.Logic.Bridges.Velux.Datagrams;
using System.Collections.Generic;

namespace neleo_com.Logic.Bridges.Velux {

    public class Klf200Gateway : IDisposable {

        /// <summary>
        ///   The low-level connection to the Velux KLF-200 gateway.</summary>
        private readonly Klf200SocketBc Socket;
        //private readonly Klf200Socket Socket;

        /// <summary>
        ///   Catalog to resolve names and identifiers from nodes, groups and scenes.</summary>
        private readonly Klf200Catalog Catalog;

        /// <summary>
        ///   Indicates whether this instance has been disposed.</summary>
        private Boolean IsDisposed = false;

        /// <summary>
        ///   High-level connection to a Velux KLF-200 gateway. This class manages the translation 
        ///   of high-level requests and responses (strings, <see cref="VeluxGateway"/>) 
        ///   and the lower-level datagrams (byte arrays, <see cref="Klf200Socket"/>).</summary>
        /// <param name="host">
        ///   The gateway's IP address.</param>
        /// <param name="password">
        ///   The gateway's password.</param>
        public Klf200Gateway(IPAddress host, String password) {

            if (host == null)
                throw new ArgumentNullException(nameof(host));

            if (String.IsNullOrWhiteSpace(password))
                throw new ArgumentNullException(nameof(password));

            this.Catalog = new Klf200Catalog();

            this.Socket = new Klf200SocketBc(host, password);
            //this.Socket = new Klf200Socket(host, password);
            this.Socket.OnResponse += this.Socket_OnResponse;
            this.Socket.OnError += this.Socket_OnError;

        }

        /// <summary>
        ///   Sets the internal clock (utc date/time).</summary>
        /// <param name="utcDateTime">
        ///   The </param>
        public void SetClock(DateTime utcDateTime) {

            // send date/time
            this.Socket.SendRequest(new GW_SET_UTC_REQ(utcDateTime));

        }

        /// <summary>
        ///   Keeps the connection to the Velux KLF-200 gateway open and alive.</summary>
        public void KeepAlive() {

            // send "keep alive" datagram
            this.Socket.SendRequest(new GW_GET_STATE_REQ());

        }

        /// <summary>
        ///   Requests the state of all nodes, groups and scenes.</param>
        public void RequestState() {

            // see Klf200Socket > HandleResponse() for cascading requests on groups and scenes
            this.Socket.SendRequest(new GW_GET_ALL_NODES_INFORMATION_REQ());

        }

        /// <summary>
        ///   Requests the KLF-200 to reboot.</summary>
        public void RequestReboot() {

            this.Socket.SendRequest(new GW_REBOOT_REQ());

        }

        /// <summary>
        ///   Handles an incoming request by translating it to a Velux KLF-200 gateway 
        ///   request frame and sending it.</summary>
        /// <param name="telegram">
        ///   The telegram that should be send to the gateway.</param>
        public void SendRequest(Klf200Telegram telegram) {

            // ignore empty telegrams
            if (telegram == null)
                return;

            // handle telegrams by mode
            switch (telegram.Mode) {

                case Klf200TelegramMode.Info:

                    // request table of contents
                    String catalogToc = this.Catalog.GetCatalogToc(telegram.Scope);

                    // reply with table of contents
                    if (String.IsNullOrWhiteSpace(catalogToc))
                        this.ThrowInfo(Properties.Resources.Info_CatalogTocEmpty, telegram.Scope);
                    else
                        this.ThrowInfo(Properties.Resources.Info_CatalogToc, telegram.Scope, catalogToc);
                    return;

                case Klf200TelegramMode.Request:

                    // try to map the telegram to a datagram
                    Klf200Datagram datagram = Klf200DatagramService.CreateRequest(telegram.Scope, telegram.Action);

                    // configure datagram (if it has been created) by the telegram and send it to the gateway
                    if (datagram != null && datagram is IKlf200DatagramTelegramDecoder telegramDecoder)
                        if (telegramDecoder.DecodeTelegram(telegram, (scope, name) => this.Catalog.ResolveIdentifier(scope, name)))
                            this.Socket.SendRequest(datagram);
                        else
                            this.ThrowError(Properties.Resources.Error_UnsupportedTelegram, telegram);
                    else
                        this.ThrowError(Properties.Resources.Error_UnsupportedTelegram, telegram);
                    return;

                case Klf200TelegramMode.Response:
                    this.ThrowError(Properties.Resources.Error_OutgoingTelegram, telegram);
                    return;

                default:
                    return;

            }

        }

        /// <summary>
        ///   Handles a response from the Velux KLF-200 gateway.</summary>
        /// <param name="sender">
        ///   The sender (a <see cref="Klf200Socket"/>).</param>
        /// <param name="datagram">
        ///   The datagram.</param>
        private void Socket_OnResponse(Object sender, Klf200Datagram datagram) {

            // ignore empty datagrams
            if (datagram == null)
                return;

            // send heartbeat
            this.NotifyHeartbeat();

            // handle specific datagrams
            switch (datagram.Id) {

                case Klf200Command.GW_GET_NODE_INFORMATION_NTF:
                case Klf200Command.GW_GET_ALL_NODES_INFORMATION_NTF:
                    if (datagram is Klf200NodeInformationDatagram nodeDatagram)
                        this.Catalog.Collect(Klf200TelegramScope.Node, nodeDatagram.Name, nodeDatagram.NodeId);
                    break;

                case Klf200Command.GW_NODE_INFORMATION_CHANGED_NTF:
                    if (datagram is GW_NODE_INFORMATION_CHANGED_NTF nodeDatagram2)
                        this.Catalog.Collect(Klf200TelegramScope.Node, nodeDatagram2.Name, nodeDatagram2.NodeId);
                    return;

                case Klf200Command.GW_GET_GROUP_INFORMATION_NTF:
                case Klf200Command.GW_GET_ALL_GROUPS_INFORMATION_NTF:
                    if (datagram is Klf200GroupInformationDatagram groupDatagram)
                        this.Catalog.Collect(Klf200TelegramScope.Group, groupDatagram.Name, groupDatagram.GroupId);
                    return;

                case Klf200Command.GW_GROUP_INFORMATION_CHANGED_NTF:
                    if (datagram is GW_GROUP_INFORMATION_CHANGED_NTF groupDatagram2)
                        this.Catalog.Collect(Klf200TelegramScope.Group, groupDatagram2.Name, groupDatagram2.GroupId);
                    return;

                case Klf200Command.GW_GET_SCENE_LIST_NTF:
                    if (datagram is GW_GET_SCENE_LIST_NTF scenesDatagram)
                        foreach (KeyValuePair<Byte, String> scene in scenesDatagram.Scenes)
                            this.Catalog.Collect(Klf200TelegramScope.Scene, scene.Value, scene.Key);
                    return;

                case Klf200Command.GW_GET_SCENE_INFORMATION_NTF:
                    if (datagram is GW_GET_SCENE_INFORMATION_NTF sceneDatagram)
                        this.Catalog.Collect(Klf200TelegramScope.Scene, sceneDatagram.Name, sceneDatagram.SceneId);
                    return;

            }

            if (this.OnResponse != null && datagram is IKlf200DatagramTelegramEncoder telegramEncoder) {

                // create telegram and try to resolve names for the node/group/scene
                Klf200Telegram telegram = telegramEncoder.EncodeTelegram((scope, id) => this.Catalog.ResolveName(scope, id));

                // telegram is null, if there was an error when resolving the datagram
                if (telegram != null)
                    this.OnResponse(this, telegram);
                else
                    this.ThrowError(Properties.Resources.Error_UnknownDatagram, datagram.Id);

            }

        }

        /// <summary>
        ///   Handles errors from the socket by passing them to the logic module.</summary>
        /// <param name="sender">
        ///   The sender (a <see cref="Klf200Socket"/>).</param>
        /// <param name="message">
        ///   A formatable message.</param>
        private void Socket_OnError(Object sender, String message) {

            // ignore errors if properties aren't initialized properly
            if (this.OnError == null || String.IsNullOrWhiteSpace(message))
                return;

            // send formatted message
            this.OnError(sender, message);

        }

        /// <summary>
        ///   Notifies the logic module about incoming messages 
        ///   (not all incoming datagrams are translated and forwared as telegrams).</summary>
        private void NotifyHeartbeat() {

            if (this.OnHeartbeat != null)
                this.OnHeartbeat(this, new EventArgs());

        }

        /// <summary>
        ///   Indicates that a message from the KLF-200 has arrived.</summary>
        public event EventHandler OnHeartbeat;

        /// <summary>
        ///   Passes translated response telegrams (confirmations and notifications) 
        ///   from the Velux KLF-200 gateway to the logic module.</summary>
        public event EventHandler<Klf200Telegram> OnResponse;

        /// <summary>
        ///   Shares information by passing them to the logic module.</summary>
        /// <param name="message">
        ///   A formatable message.</param>
        /// <param name="parameters">
        ///   Parameters for the formatable message.</param>
        private void ThrowInfo(String message, params Object[] parameters) {

            // ignore errors if properties aren't initialized properly
            if (this.OnInfo == null || String.IsNullOrWhiteSpace(message))
                return;

            // send formatted message
            this.OnInfo(this, String.Format(message, parameters));

        }

        /// <summary>
        ///   Passes responses to "info://"-requests to the logic module.</summary>
        public event EventHandler<String> OnInfo;

        /// <summary>
        ///   Throws errors by passing them to the logic module.</summary>
        /// <param name="message">
        ///   A formatable message.</param>
        /// <param name="parameters">
        ///   Parameters for the formatable message.</param>
        private void ThrowError(String message, params Object[] parameters) {

            // ignore errors if properties aren't initialized properly
            if (this.OnError == null || String.IsNullOrWhiteSpace(message))
                return;

            // send formatted message
            this.OnError(this, String.Format(message, parameters));

        }

        /// <summary>
        ///   Passes errors to the logic module.</summary>
        public event EventHandler<String> OnError;

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

            if (this.Socket != null)
                this.Socket.Dispose();

            this.IsDisposed = true;

        }

    }

}
