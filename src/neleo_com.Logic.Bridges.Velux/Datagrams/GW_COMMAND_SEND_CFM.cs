using System;

using neleo_com.Logic.Bridges.Velux.Definitions;

namespace neleo_com.Logic.Bridges.Velux.Datagrams {

    /// <summary>
    ///   Confirmation for <see cref="GW_COMMAND_SEND_REQ"/>.</summary>
    public sealed class GW_COMMAND_SEND_CFM : Klf200Datagram, IKlf200DatagramSessionResponse {

        /// <summary>
        ///   Initialize the command.</summary>
        public GW_COMMAND_SEND_CFM() : base(Klf200Command.GW_COMMAND_SEND_CFM, 3) { }

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
        ///   Indicates if the corresponding <see cref="GW_COMMAND_SEND_REQ"/> command is accepted 
        ///   or rejected by the Command Handler.</summary>
        public GW_NodeCommandRequestState State {
            get => this.Data.ReadEnum<GW_NodeCommandRequestState>(2, 1, GW_NodeCommandRequestState.RequestRejected);
        }

    }

}
