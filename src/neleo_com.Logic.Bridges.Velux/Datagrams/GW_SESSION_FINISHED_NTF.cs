using System;

namespace neleo_com.Logic.Bridges.Velux.Datagrams {

    /// <summary>
    ///   Indicates that a session has been completed.</summary>
    public sealed class GW_SESSION_FINISHED_NTF : Klf200Datagram, IKlf200DatagramSessionResponse {

        /// <summary>
        ///   Initialize the command.</summary>
        public GW_SESSION_FINISHED_NTF() : base(Klf200Command.GW_COMMAND_SEND_CFM, 2) { }

        /// <summary>
        ///   Gets a session idenfier.</summary>
        public UInt16 SessionId {
            get => this.Data.ReadUInt16(0);
        }

        /// <summary>
        ///   Gets an indicator whether this is the final datagram for this session.</summary>
        public Boolean IsFinal {
            get => true;
        }

    }
}
