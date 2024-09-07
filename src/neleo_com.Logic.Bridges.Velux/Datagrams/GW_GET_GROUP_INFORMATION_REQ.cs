using System;

namespace neleo_com.Logic.Bridges.Velux.Datagrams {

    /// <summary>
    ///   Request information about a specific group.</summary>
    public sealed class GW_GET_GROUP_INFORMATION_REQ : Klf200Datagram {

        /// <summary>
        ///   Initialize the command.</summary>
        /// <param name="groupId">
        ///   The group identifier.</param>
        public GW_GET_GROUP_INFORMATION_REQ(Byte groupId) : base(Klf200Command.GW_GET_GROUP_INFORMATION_REQ, 1) {

            this.Data.WriteByte(groupId, 0);

        }

    }

}