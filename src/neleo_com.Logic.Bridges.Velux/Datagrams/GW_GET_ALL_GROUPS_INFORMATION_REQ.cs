using System;

namespace neleo_com.Logic.Bridges.Velux.Datagrams {

    /// <summary>
    ///   Request information for all groups. Every group information is sent in a 
    ///   <see cref="GW_GET_ALL_GROUPS_INFORMATION_NTF"/> event. The event 
    ///   <see cref="GW_GET_ALL_GROUPS_INFORMATION_FINISHED_NTF"/> is sent after the last group information. </summary>
    public sealed class GW_GET_ALL_GROUPS_INFORMATION_REQ : Klf200Datagram {

        /// <summary>
        ///   Initialize the command.</summary>
        public GW_GET_ALL_GROUPS_INFORMATION_REQ() : base(Klf200Command.GW_GET_ALL_GROUPS_INFORMATION_REQ, 2) { }

    }

}
