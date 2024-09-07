using System;

namespace neleo_com.Logic.Bridges.Velux.Datagrams {

    /// <summary>
    ///   This event holds the information on a node.</summary>
    public sealed class GW_GET_ALL_NODES_INFORMATION_NTF : Klf200NodeInformationDatagram {

        /// <summary>
        ///   Initialize the command.</summary>
        public GW_GET_ALL_NODES_INFORMATION_NTF() : base(Klf200Command.GW_GET_ALL_NODES_INFORMATION_NTF, 124) { }

    }

}
