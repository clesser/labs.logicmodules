using System;

namespace neleo_com.Logic.Bridges.Velux.Datagrams {

    /// <summary>
    ///   This event holds the information on a group.</summary>
    public sealed class GW_GET_GROUP_INFORMATION_NTF : Klf200GroupInformationDatagram {

        /// <summary>
        ///   Initialize the command.</summary>
        public GW_GET_GROUP_INFORMATION_NTF() : base(Klf200Command.GW_GET_GROUP_INFORMATION_NTF, 99) { }

    }

}
