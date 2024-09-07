using System;

using neleo_com.Logic.Bridges.Velux.Definitions;

namespace neleo_com.Logic.Bridges.Velux.Datagrams {

    /// <summary>
    ///   For each actuator addressed by IndexArray in the <see cref="GW_COMMAND_SEND_REQ"/> 
    ///   and other request frames, the gateway will return with two <see cref="GW_COMMAND_RUN_STATUS_NTF"/>
    ///   frames.One before and one after the given actuators movement.</summary>
    public sealed class GW_COMMAND_RUN_STATUS_NTF : Klf200Datagram, IKlf200DatagramSessionResponse, IKlf200DatagramTelegramEncoder {

        /// <summary>
        ///   Initialize the command.</summary>
        public GW_COMMAND_RUN_STATUS_NTF() : base(Klf200Command.GW_COMMAND_RUN_STATUS_NTF, 13) { }

        /// <summary>
        ///   Gets a session idenfier.</summary>
        public UInt16 SessionId {
            get => this.Data.ReadUInt16(0);
        }

        /// <summary>
        ///   Gets an indicator whether this is the final datagram for this session.</summary>
        public Boolean IsFinal {
            get => false;
        }

        /// <summary>
        ///   Identification of the status owner.</summary>
        public GW_RunStateOwner StatusOwner {
            get => this.Data.ReadEnum<GW_RunStateOwner>(2, 1, GW_RunStateOwner.Unknown);
        }

        /// <summary>
        ///   NodeID is an Actuator index in the system table, to get information from. 
        ///   It must be a value from 0 to 199.</summary>
        public Byte NodeId {
            get => this.Data.ReadByte(3);
        }

        /// <summary>
        ///   Identifies the parameter that ParameterValue carry information about.</summary>
        public GW_ParameterType ParameterId {
            get => this.Data.ReadEnum<GW_ParameterType>(4, 1, GW_ParameterType.NotUsed);
        }

        /// <summary>
        ///   Contains the current value of the active parameter.</summary>
        public UInt16 ParameterValue {
            get => this.Data.ReadUInt16(5);
        }

        /// <summary>
        ///   Contains the execution status of the node.</summary>
        /// <remarks>
        ///   See <see cref="RunStateError"/> and <see cref="RunStateInfo"/> for detailed information if
        ///   <see cref="GW_RunState.Failed"/> is set.</remarks>
        public GW_RunState RunState {
            get => this.Data.ReadEnum<GW_RunState>(7, 1, GW_RunState.Active);
        }

        /// <summary>
        ///   Contains current state of the node. (Error code)</summary>
        public GW_RunStateError RunStateError {
            get => this.Data.ReadEnum<GW_RunStateError>(8, 1, GW_RunStateError.UnknownStatusReply);
        }

        /// <summary>
        ///   InformationCode is a 32-bit long integer. InformationCode contains the hexadecimal 
        ///   information code to show if system is unable to decode status.</summary>
        public UInt32 RunStateInfo {
            get => this.Data.ReadUInt32(9);
        }

        /// <summary>
        ///   Creates a telegram out of this datagram's properties.</summary>
        /// <param name="resolver">
        ///   Replace the identifier of a node/group/scene by it's name. Returns <see cref="String.Empty"/> if the identifier
        ///   can't be resolved.</param>
        /// <returns>
        ///   A telegram.</returns>
        public Klf200Telegram EncodeTelegram(Func<Klf200TelegramScope, Byte, String> resolver) {

            Klf200Telegram telegram = new Klf200Telegram() {
                Mode = Klf200TelegramMode.Response,
                Scope = Klf200TelegramScope.Node,
                Identifier = this.NodeId
            };

            // resolve node name
            if (resolver != null)
                telegram.Name = resolver.Invoke(Klf200TelegramScope.Node, this.NodeId);

            // export parameter
            // identifiers of GW_ParameterType and Klf200TelegramParameters are equal
            // Main parameter = 0x00 .. Functional Parameter 16 = 0x10
            if ((Byte)this.ParameterId <= 0x10) {

                Byte parameterId = (Byte)this.ParameterId;
                telegram.SetParameter((Klf200TelegramParameter)parameterId, this.ParameterValue);

            }

            // return error code and error info on demand
            if (this.RunState == GW_RunState.Failed) {

                telegram.SetParameter(Klf200TelegramParameter.Error, this.RunStateError);
                telegram.SetParameter(Klf200TelegramParameter.ErrorInfo, this.RunStateInfo);

            }

            return telegram;

        }

    }

}
