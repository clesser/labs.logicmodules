using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Security.Cryptography.X509Certificates;
using System.Threading;
using System.Threading.Tasks;

using neleo_com.Logic.Bridges.Velux.Datagrams;

using Org.BouncyCastle.Crypto.Tls;
using Org.BouncyCastle.Security;

namespace neleo_com.Logic.Bridges.Velux {

    /// <summary>
    ///   Extension methods for a very basic certificate validation.</summary>
    public static class IDictionaryHelpers {

        /// <summary>
        ///   Parses an issuer string into a dictionary.</summary>
        /// <param name="dictionary">
        ///   The dictionary.</param>
        /// <param name="value">
        ///   The value to be parsed into the dictionary.</param>
        /// <param name="kvpSeparator">
        ///   The key-value-pair separator (usually it's ',').</param>
        /// <param name="kvSeparator">
        ///   The key-value separator (usually it's '=').</param>
        public static void Parse(this IDictionary<String, String> dictionary, String value, Char kvpSeparator, Char kvSeparator) {

            if (String.IsNullOrWhiteSpace(value))
                return;

            foreach (String parameter in value.Split(new char[] { kvpSeparator })) {

                String[] keyValuePair = parameter.Split(new Char[] { kvSeparator });
                if (keyValuePair.Length == 2)
                    if (dictionary.ContainsKey(keyValuePair[0].Trim()))
                        dictionary[keyValuePair[0].Trim()] = keyValuePair[1].Trim();
                    else
                        dictionary.Add(keyValuePair[0].Trim(), keyValuePair[1].Trim());

            }

        }

        /// <summary>
        ///   Evaluates two dictionaries whether all key-value-pairs are equal.</summary>
        /// <param name="dictionary">
        ///   The dictionary.</param>
        /// <param name="otherDictionary">
        ///   Another dictionary.</param>
        /// <returns>
        ///   <c>true</c> if all key-value-pairs in both directories are equal.</returns>
        public static Boolean EqualValues(this IDictionary<String, String> dictionary, IDictionary<String, String> otherDictionary) {

            if (dictionary.Count() != otherDictionary.Count())
                return false;

            return dictionary.All((kvp) => {

                return otherDictionary.ContainsKey(kvp.Key) && kvp.Value.Equals(otherDictionary[kvp.Key]);

            });

        }

    }

    /// <summary>
    ///   A very basic Bouncy Castle TLS client implementation.</summary>
    internal class TlsClientBc : DefaultTlsClient {

        /// <summary>
        ///   The server certificate to match.</summary>
        private readonly X509Certificate ServerCertificate;

        /// <summary>
        ///   Initializes a new client.</summary>
        /// <param name="serverCertificate">
        ///   The server certificate.</param>
        public TlsClientBc(X509Certificate serverCertificate) {

            this.ServerCertificate = serverCertificate;

        }

        /// <summary>
        ///   Returns the authentication handler on request.</summary>
        /// <returns>
        ///   The authentication.</returns>
        public override TlsAuthentication GetAuthentication() {

            return new TlsAuthenticationBc(this.ServerCertificate);

        }

    }

    /// <summary>
    ///   A very basic Bouncy Castle TLS authentication implementation.</summary>
    internal class TlsAuthenticationBc : TlsAuthentication {

        /// <summary>
        ///   The server certificate to match.</summary>
        private readonly X509Certificate ServerCertificate;

        /// <summary>
        ///   Initializes a new authentication handler.</summary>
        /// <param name="serverCertificate">
        ///   The server certificate.</param>
        public TlsAuthenticationBc(X509Certificate serverCertificate) {

            this.ServerCertificate = serverCertificate;

        }

        /// <summary>
        ///   Returns client certificates on request.</summary>
        /// <param name="certificateRequest">
        ///   The request.</param>
        /// <returns>
        ///   <c>null</c> as there're no client certificates used.</returns>
        public TlsCredentials GetClientCredentials(CertificateRequest certificateRequest) {

            return null;

        }

        /// <summary>
        ///   Validates the server certificate (on a very basic way...).</summary>
        /// <param name="serverCertificate">
        ///   The server certificate.</param>
        public void NotifyServerCertificate(Certificate serverCertificate) {

            if (serverCertificate.IsEmpty)
                throw new GeneralSecurityException("No server certificate presented.");

            IDictionary<String, String> issuer1 = new Dictionary<String, String>();
            issuer1.Parse(this.ServerCertificate.Issuer, ',', '=');

            Boolean isValid = serverCertificate.GetCertificateList().Any((cert) => {

                IDictionary<String, String> issuer2 = new Dictionary<String, String>();
                issuer2.Parse(cert.Issuer.ToString(), ',', '=');

                return issuer1.EqualValues(issuer2);

            });

            if (!isValid)
                throw new GeneralSecurityException("No valid server certificate presented.");

        }

    }

    /// <summary>
    ///   Asynchronous socket implementation to handle the connection to the Velux KLF-200 gateway.</summary>
    public class Klf200SocketBc : IDisposable {

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
        ///   The TCP client socket for the gateway communication.</summary>
        private readonly TcpClient GatewaySocket;

        /// <summary>
        ///   The TCP protocol communication wrapper.</summary>
        private readonly TlsClientProtocol GatewayProtocol;

        /// <summary>
        ///   The secure channel to send and receicve data frames.</summary>
        private readonly Stream GatewayStream;

        /// <summary>
        ///   Controls the Listener to shut down before disposing the stream and socket.</summary>
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
        public Klf200SocketBc(IPAddress host, String password) {

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
            this.GatewayProtocol = new TlsClientProtocol(this.GatewaySocket.GetStream(), new SecureRandom());
            this.GatewayProtocol.Connect(new TlsClientBc(this.GatewayCertificate));
            this.GatewayStream = this.GatewayProtocol.Stream;

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
        ///   Sends a datagram to the gateway (asynchronous operation). </summary>
        /// <param name="datagram">
        ///   The datagram.</param>
        public void SendRequest(Klf200Datagram datagram) {

            // ensure datagram is set
            if (datagram == null)
                throw new ArgumentNullException(nameof(datagram));

            // request session id on demand
            if (datagram is IKlf200DatagramSessionRequest sessionRequestor)
                sessionRequestor.SessionId = this.LastSessionId++;

            // encode data frame
            Byte[] data = Klf200SocketBc.EncodeFrame(datagram);

            // send data frame
            this.GatewayStream.Write(data, 0, data.Length);
            this.GatewayStream.Flush();

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
            return Klf200SocketBc.DecodeFrame(buffer.Take(byteCount).ToArray());

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
                case Klf200Command.GW_GET_STATE_CFM:
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
                Klf200SocketBc.Protocol
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
            Int32 crc = Klf200SocketBc.CrcSalt;
            foreach (Byte data in dataFrame)
                crc ^= data;
            dataFrame.Add((Byte)crc);

            // ---------- Step 2: SLIP-wrap the composed frame ----------

            // add END mark
            ICollection<Byte> slippedDataFrame = new Collection<Byte> {
                Klf200SocketBc.SlipEnd
            };

            // encode dataFrame
            foreach (Byte data in dataFrame) {

                switch (data) {

                    case Klf200SocketBc.SlipEnd:
                        slippedDataFrame.Add(Klf200SocketBc.SlipEsc);
                        slippedDataFrame.Add(Klf200SocketBc.SlipEscEnd);
                        break;

                    case Klf200SocketBc.SlipEsc:
                        slippedDataFrame.Add(Klf200SocketBc.SlipEsc);
                        slippedDataFrame.Add(Klf200SocketBc.SlipEscEsc);
                        break;

                    default:
                        slippedDataFrame.Add(data);
                        break;

                }

            }

            // add END mark
            slippedDataFrame.Add(Klf200SocketBc.SlipEnd);

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
                || slippedDataFrame.First() != Klf200SocketBc.SlipEnd
                || slippedDataFrame.Last() != Klf200SocketBc.SlipEnd)
                throw new ArgumentException(nameof(slippedDataFrame));

            // unwrap slipped data frame (ignore first and last "END")
            ICollection<Byte> dataFrame = new Collection<Byte>();
            Int32 maxPos = slippedDataFrame.Count() - 1;
            Int32 pos = 1;

            while (pos < maxPos) {

                Byte data = slippedDataFrame.ElementAt(pos);
                pos++;

                switch (data) {

                    case Klf200SocketBc.SlipEsc:
                        data = slippedDataFrame.ElementAt(pos);
                        pos++;

                        switch (data) {

                            case Klf200SocketBc.SlipEscEnd:
                                dataFrame.Add(Klf200SocketBc.SlipEnd);
                                break;

                            case Klf200SocketBc.SlipEscEsc:
                                dataFrame.Add(Klf200SocketBc.SlipEsc);
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
            if (dataFrame.Count() < 5 || !dataFrame.First().Equals(Klf200SocketBc.Protocol))
                throw new ArgumentException(nameof(slippedDataFrame));

            // verify data frame checksum
            Int32 crc = Klf200SocketBc.CrcSalt;
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

            this.GatewayListener.Cancel();
            this.GatewayStream.Close();
            this.GatewayProtocol.Close();
            this.GatewaySocket.Close();

            this.IsDisposed = true;

        }

    }

}