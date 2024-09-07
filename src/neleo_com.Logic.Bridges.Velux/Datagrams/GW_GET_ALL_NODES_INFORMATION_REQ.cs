using System;

namespace neleo_com.Logic.Bridges.Velux.Datagrams {

    /// <summary>
    ///   This event will get the information on all nodes. Every node information is sent in a 
    ///   <see cref="GW_GET_ALL_NODES_INFORMATION_NTF"/> event. The event 
    ///   <see cref="GW_GET_ALL_NODES_INFORMATION_FINISHED_NTF"/> is sent after the last node information.</summary>
    public sealed class GW_GET_ALL_NODES_INFORMATION_REQ : Klf200Datagram {

        /// <summary>
        ///   Initialize the command.</summary>
        public GW_GET_ALL_NODES_INFORMATION_REQ() : base(Klf200Command.GW_GET_ALL_NODES_INFORMATION_REQ, 0) { }

    }

}
