using System;

namespace neleo_com.Logic.Bridges.Velux.Datagrams {

    /// <summary>
    ///   This event is sent after the last group information, indicating no more groups.</summary>
    public sealed class GW_GET_ALL_GROUPS_INFORMATION_FINISHED_NTF : Klf200Datagram {

        /// <summary>
        ///   Initialize the command.</summary>
        public GW_GET_ALL_GROUPS_INFORMATION_FINISHED_NTF() : base(Klf200Command.GW_GET_ALL_GROUPS_INFORMATION_FINISHED_NTF, 0) { }

    }

}
