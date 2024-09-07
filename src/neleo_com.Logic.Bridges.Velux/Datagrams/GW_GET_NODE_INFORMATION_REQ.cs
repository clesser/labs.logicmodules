using System;

namespace neleo_com.Logic.Bridges.Velux.Datagrams {

    /// <summary>
    ///   Request information about a specific node.</summary>
    public sealed class GW_GET_NODE_INFORMATION_REQ : Klf200Datagram {

        /// <summary>
        ///   Initialize the command.</summary>
        /// <param name="nodeId">
        ///   The node identifier.</param>
        public GW_GET_NODE_INFORMATION_REQ(Byte nodeId) : base(Klf200Command.GW_GET_NODE_INFORMATION_REQ, 1) {

            this.Data.WriteByte(nodeId, 0);

        }

    }

}