/*
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net;
using System.Net.Security;
using System.Net.Sockets;
using System.Security.Authentication;
using System.Security.Cryptography.X509Certificates;
using System.Threading;
using System.Threading.Tasks;

using neleo_com.Logic.Bridges.Velux.Datagrams;

namespace neleo_com.Logic.Bridges.Velux {

    /// <summary>
    ///   Asynchronous socket implementation to handle the connection to the Velux KLF-200 gateway.</summary>
    public class Klf200Socket : IDisposable {

        /// <summary>
        ///   Velux KLF-200 gateway protocol is always "0".</summary>
        private const Byte Protocol = 0;

        /// <summary>
        ///   Initial value for CRC checksum calculation.</summary>
        private const Byte CrcSalt = 0;

        /// <summary>
        ///   "END" character code used by SLIP protocol encoding.</summary>
        private const Byte SlipEnd = 0xC0;

        /// <summary>
        ///   "ESC" character code used by SLIP protocol encoding.</summary>
        private const Byte SlipEsc = 0xDB;

        /// <summary>
        ///   "ESC_END" character code used by SLIP protocol encoding.</summary>
        private const Byte SlipEscEnd = 0xDC;

        /// <summary>
        ///   "ESC_ESC" character code used by SLIP protocol encoding.</summary>
        private const Byte SlipEscEsc = 0xDD;

        /// <summary>
        ///   Indicates whether this instance has been disposed.</summary>
        private Boolean IsDisposed = false;

        /// <summary>
        ///   Session identifier is used in some commands. Increment value before using it.</summary>
        private UInt16 LastSessionId = UInt16.MinValue;

        /// <summary>
        ///   The self-signed certificate of the Velux KLF-200 gateway.</summary>
        private readonly X509Certificate GatewayCertificate;

        /// <summary>
        ///   The tcp client socket for the gateway communication.</summary>
        private readonly TcpClient GatewaySocket;

        /// <summary>
        ///   The secure channel to send and receicve data frames.</summary>
        private readonly SslStream GatewayStream;

        /// <summary>
        ///   Controls the Listerner to shut down before disposing the stream and socket.</summary>
        private readonly CancellationTokenSource GatewayListener;

        /// <summary>
        ///   Lower-level connection to a Velux KLF-200 gateway. Connects to the gateway, 
        ///   establishes a secure channel and authenticates against it.
        ///   While communication is asynchronous in all other scenarios, 
        ///   socket creation (and first tasks) are performed synchronously!</summary>
        /// <param name="host">
        ///   The gateway's IP address.</param>
        /// <param name="password">
        ///   The gateway's password.</param>
        public Klf200Socket(IPAddress host, String password) {

            // verify parameters
            if (host == null)
                throw new ArgumentNullException(nameof(host));

            if (String.IsNullOrWhiteSpace(password))
                throw new ArgumentException(nameof(password));

            // connect to Velux Klf-200 gateway using a tcp client socket
            this.GatewayCertificate = new X509Certificate(Properties.Resources.velux_cert);
            this.GatewaySocket = new TcpClient(AddressFamily.InterNetwork);
            this.GatewaySocket.Connect(host, 51200);

            // stop furter processing if the connection could not be established
            if (!this.GatewaySocket.Connected)
                throw new WebException(String.Format(Properties.Resources.Exception_GatewayConnection, host));

            // use a secure channel, but don't verify certificate chain (it's self-signed anyway)
            this.GatewayStream = new SslStream(GatewaySocket.GetStream(), false, (sender, cert, chain, errors) => {
                return true;
            });

            // try to establish a secure channel and stop further processing in case of an error
            try {

                // configure secure channel certificates
                X509CertificateCollection certificateCollection = new X509CertificateCollection { this.GatewayCertificate };

                // set secure channel security protocol
                SslProtocols protocols = SslProtocols.Tls12;

                // shake hands
                this.GatewayStream.AuthenticateAsClient(host.ToString(), certificateCollection, protocols, false);

            }
            catch (AuthenticationException ex) {

                this.GatewayStream.Close();
                this.GatewaySocket.Close();
                throw new WebException(Properties.Resources.Exception_GatewayConnectionSecurity, ex);

            }

            // send password to authenticate
            this.SendRequest(new GW_PASSWORD_ENTER_REQ(password));

            // handle response
            Klf200Datagram response = this.NextResponse();
            if (!(response is GW_PASSWORD_ENTER_CFM confirmation && confirmation.Success))
                throw new WebException(Properties.Resources.Exception_GatewayConnectionAuthorization);

            // setup datagram listener
            this.GatewayListener = new CancellationTokenSource();
            CancellationToken cancellationToken = this.GatewayListener.Token;

            Task.Run(async () => {

                while (!cancellationToken.IsCancellationRequested) {

                    this.HandleResponse(await this.NextResponseAsync(cancellationToken));

                }

            }, cancellationToken);

            // set House Monitoring -> on;
            // this will also start a cascading request of all scenes, groups and nodes (see HandleResponse())
            this.SendRequest(new GW_HOUSE_STATUS_MONITOR_ENABLE_REQ());

        }

        /// <summary>
        ///   Sends a datagram to the gateway.</summary>
        /// <param name="datagram">
        ///   The datagram.</param>
        public void SendRequest(Klf200Datagram datagram) {

            // ensure datagram is set
            if (datagram == null)
                throw new ArgumentNullException(nameof(datagram));

            // send datagram asynchonously
            Task.Run(() => this.SendRequestAsync(datagram, CancellationToken.None));

        }

        /// <summary>
        ///   Sends a datagram to the gateway (asynchronous operation). </summary>
        /// <param name="datagram">
        ///   The datagram.</param>
        /// <param name="cancellationToken">
        ///   A cancellation token - or - <c>null</c>.</param>
        /// <returns>
        ///   A task handler.</returns>
        private async Task SendRequestAsync(Klf200Datagram datagram, CancellationToken cancellationToken) {

            // ensure datagram is set
            if (datagram == null)
                throw new ArgumentNullException(nameof(datagram));

            // request session id on demand
            if (datagram is IKlf200DatagramSessionRequest sessionRequestor)
                sessionRequestor.SessionId = this.LastSessionId++;

            // encode data frame
            Byte[] data = Klf200Socket.EncodeFrame(datagram);

            // send data frame
            await this.GatewayStream.WriteAsync(data, 0, data.Length, cancellationToken);
            await this.GatewayStream.FlushAsync(cancellationToken);

        }

        /// <summary>
        ///   Grabs the next datagram from the stream.</summary>
        /// <returns>
        ///   A datagram.</returns>
        private Klf200Datagram NextResponse() {

            // read next data frame asynchronously
            return Task.Run(() => this.NextResponseAsync(CancellationToken.None)).Result;

        }

        /// <summary>
        ///   Grabs the next datagram from the stream (asynchronous operation).</summary>
        /// <returns>
        ///   A datagram.</returns>
        private async Task<Klf200Datagram> NextResponseAsync(CancellationToken cancellationToken) {

            // read next data frame
            Byte[] buffer = new Byte[Byte.MaxValue * 2];
            Int32 byteCount = await this.GatewayStream.ReadAsync(buffer, 0, buffer.Length, cancellationToken);

            // decode data frame to datagram
            return Klf200Socket.DecodeFrame(buffer.Take(byteCount).ToArray());

        }

        /// <summary>
        ///   Handles an incomming datagram either internally or pass it to the next level handler (if set).</summary>
        /// <param name="datagram">
        ///   The datagram.</param>
        private void HandleResponse(Klf200Datagram datagram) {

            // ignore empty datagrams
            if (datagram == null)
                return;

            // handle some datagrams internally, pass all others to the next level handler
            switch (datagram.Id) {

                // process errors
                case Klf200Command.GW_ERROR_NTF:
                    this.ThrowError(Properties.Resources.Error_GatewayCommunication, ((GW_ERROR_NTF)datagram).GatewayError);
                    return;

                // catch all confirmations that don't require further handling
                //case Klf200Command.GW_GET_STATE_CFM:
                case Klf200Command.GW_PASSWORD_ENTER_CFM:
                case Klf200Command.GW_HOUSE_STATUS_MONITOR_DISABLE_CFM:
                case Klf200Command.GW_GET_ALL_NODES_INFORMATION_CFM:
                case Klf200Command.GW_GET_ALL_GROUPS_INFORMATION_CFM:
                case Klf200Command.GW_GET_SCENE_LIST_CFM:
                case Klf200Command.GW_SET_UTC_CFM:
                    return;

                // report identifier issues
                case Klf200Command.GW_GET_NODE_INFORMATION_CFM:
                    GW_GET_NODE_INFORMATION_CFM nodeInformationConfirmation = (GW_GET_NODE_INFORMATION_CFM)datagram;
                    if (nodeInformationConfirmation.State == Definitions.GW_InformationRequestState.RequestRejectedInvalidId)
                        this.ThrowError(Properties.Resources.Error_UnknownIdentifier, "node", nodeInformationConfirmation.NodeId);
                    return;

                case Klf200Command.GW_GET_GROUP_INFORMATION_CFM:
                    GW_GET_GROUP_INFORMATION_CFM groupInformationConfirmation = (GW_GET_GROUP_INFORMATION_CFM)datagram;
                    if (groupInformationConfirmation.State == Definitions.GW_InformationRequestState.RequestRejectedInvalidId)
                        this.ThrowError(Properties.Resources.Error_UnknownIdentifier, "group", groupInformationConfirmation.GroupId);
                    return;

                case Klf200Command.GW_GET_SCENE_INFORMATION_CFM:
                    GW_GET_SCENE_INFORMATION_CFM sceneInformationConfirmation = (GW_GET_SCENE_INFORMATION_CFM)datagram;
                    if (sceneInformationConfirmation.State == Definitions.GW_InformationRequestState.RequestRejectedInvalidId)
                        this.ThrowError(Properties.Resources.Error_UnknownIdentifier, "scene", sceneInformationConfirmation.SceneId);
                    return;

                // TODO handle activation errors

                // request scene information update when notified by the gateway
                case Klf200Command.GW_SCENE_INFORMATION_CHANGED_NTF:
                    if (datagram is GW_SCENE_INFORMATION_CHANGED_NTF sceneInformationRequest)
                        this.SendRequest(new GW_GET_SCENE_INFORMATION_REQ(sceneInformationRequest.SceneId));
                    return;

                // handle chained reading of the system table (nodes > groups > scenes)
                // step 1: request all nodes
                case Klf200Command.GW_HOUSE_STATUS_MONITOR_ENABLE_CFM:
                    this.SendRequest(new GW_GET_ALL_NODES_INFORMATION_REQ());
                    return;

                // step 2: request all groups
                case Klf200Command.GW_GET_ALL_NODES_INFORMATION_FINISHED_NTF:
                    this.SendRequest(new GW_GET_ALL_GROUPS_INFORMATION_REQ());
                    return;

                // step 3: request all scenes (note: there's no corresponding "finished" notification...)
                case Klf200Command.GW_GET_ALL_GROUPS_INFORMATION_FINISHED_NTF:
                    this.SendRequest(new GW_GET_SCENE_LIST_REQ());
                    return;

                // process other datagrams to the next level
                default:
                    if (this.OnResponse != null)
                        this.OnResponse(this, datagram);
                    return;

            }

        }

        /// <summary>
        ///   Handles incoming response frames (confirmations and notifications) from the 
        ///   Velux KLF-200 gateway and pass them to a higher abstraction level for further processing.</summary>
        public event EventHandler<Klf200Datagram> OnResponse;

        /// <summary>
        ///   Throws errors by passing them to the gateway / logic module.</summary>
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
        ///   Handles occurring errors and passes them higher abstraction level for further processing.</summary>
        public event EventHandler<String> OnError;

        /// <summary>
        ///   Composes a Velux KLF-200 gateway data frame based on a <paramref name="datagram"/> 
        ///   and SLIP-encode it afterwards.</summary>
        /// <param name="datagram">
        ///   The datagram.</param>
        /// <returns>
        ///   A Velux KLF-200 data frame.</returns>
        public static Byte[] EncodeFrame(Klf200Datagram datagram) {

            // ---------- Step 1: Compose the frame ----------

            // add header
            ICollection<Byte> dataFrame = new Collection<Byte> {
                Klf200Socket.Protocol
            };

            // add data frame length: count 1 Byte for "Length" + 2 Bytes for "Command"
            Int32 dataLength = 3 + (datagram.Data != null ? datagram.Data.Length : 0);
            dataFrame.Add((Byte)dataLength);

            // add command: conversion will return a two-byte-array in little endian, but must be trasferred as big endian
            IEnumerable<Byte> dataCommand = BitConverter.GetBytes((UInt16)datagram.Id).Reverse();
            dataFrame.Add(dataCommand.First());
            dataFrame.Add(dataCommand.Last());

            // add raw data
            if (datagram.Data != null)
                foreach (Byte data in datagram.Data)
                    dataFrame.Add(data);

            // add checksum
            Int32 crc = Klf200Socket.CrcSalt;
            foreach (Byte data in dataFrame)
                crc ^= data;
            dataFrame.Add((Byte)crc);

            // ---------- Step 2: SLIP-wrap the composed frame ----------

            // add END mark
            ICollection<Byte> slippedDataFrame = new Collection<Byte> {
                Klf200Socket.SlipEnd
            };

            // encode dataFrame
            foreach (Byte data in dataFrame) {

                switch (data) {

                    case Klf200Socket.SlipEnd:
                        slippedDataFrame.Add(Klf200Socket.SlipEsc);
                        slippedDataFrame.Add(Klf200Socket.SlipEscEnd);
                        break;

                    case Klf200Socket.SlipEsc:
                        slippedDataFrame.Add(Klf200Socket.SlipEsc);
                        slippedDataFrame.Add(Klf200Socket.SlipEscEsc);
                        break;

                    default:
                        slippedDataFrame.Add(data);
                        break;

                }

            }

            // add END mark
            slippedDataFrame.Add(Klf200Socket.SlipEnd);

            return slippedDataFrame.ToArray();

        }

        /// <summary>
        ///   Decomposes a Velux KLF-200 data frame into a datagram and SLIP-decodes it before.</summary>
        /// <param name="slippedDataFrame">
        ///   A Velux KLF-200 data frame.</param>
        /// <returns>
        ///   A datagram.</returns>
        public static Klf200Datagram DecodeFrame(Byte[] slippedDataFrame) {

            // ---------- Step 1: SLIP-unwrap the frame ----------

            // verify slipped data frame
            if (slippedDataFrame == null
                || slippedDataFrame.Count() < 3
                || slippedDataFrame.First() != Klf200Socket.SlipEnd
                || slippedDataFrame.Last() != Klf200Socket.SlipEnd)
                throw new ArgumentException(nameof(slippedDataFrame));

            // unwrap slipped data frame (ignore first and last "END")
            ICollection<Byte> dataFrame = new Collection<Byte>();
            Int32 maxPos = slippedDataFrame.Count() - 1;
            Int32 pos = 1;

            while (pos < maxPos) {

                Byte data = slippedDataFrame.ElementAt(pos);
                pos++;

                switch (data) {

                    case Klf200Socket.SlipEsc:
                        data = slippedDataFrame.ElementAt(pos);
                        pos++;

                        switch (data) {

                            case Klf200Socket.SlipEscEnd:
                                dataFrame.Add(Klf200Socket.SlipEnd);
                                break;

                            case Klf200Socket.SlipEscEsc:
                                dataFrame.Add(Klf200Socket.SlipEsc);
                                break;

                            default:
                                throw new ArgumentException(nameof(slippedDataFrame));

                        }
                        break;

                    default:
                        dataFrame.Add(data);
                        break;

                }

            }

            // ---------- Step 2: Decompose the frame ----------

            // verify data frame
            if (dataFrame.Count() < 5 || !dataFrame.First().Equals(Klf200Socket.Protocol))
                throw new ArgumentException(nameof(slippedDataFrame));

            // verify data frame checksum
            Int32 crc = Klf200Socket.CrcSalt;
            foreach (Byte data in dataFrame.Take(dataFrame.Count() - 1))
                crc ^= data;

            if (dataFrame.Last() != crc)
                throw new ArgumentException(nameof(dataFrame));

            // extract the command id (and convert from big-endian to little-endian order before (2,3 >> 3,2))
            UInt16 command = BitConverter.ToUInt16(new Byte[2] { dataFrame.ElementAt(3), dataFrame.ElementAt(2) }, 0);

            // create a datagram
            Klf200Datagram datagram = Klf200DatagramService.Create(command);

            // stop further processing if the datagram / command is not supported
            if (datagram == null)
                return null;

            // copy all raw data exluding the header (1 Byte (protocol) + 1 Byte(data length) + 2 Bytes (command))
            // and tail (1 Byte (checksum))
            Buffer.BlockCopy(dataFrame.ToArray(), 4, datagram.Data, 0, dataFrame.Count() - 5);

            return datagram;

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

            if (this.GatewayListener != null)
                this.GatewayListener.Cancel();

            if (this.GatewayStream != null)
                this.GatewayStream.Dispose();

            if (this.GatewaySocket != null)
                this.GatewaySocket.Close();

            this.IsDisposed = true;

        }

    }

}
*/