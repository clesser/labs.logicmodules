using System;

using neleo_com.Logic.Bridges.Velux.Definitions;

namespace neleo_com.Logic.Bridges.Velux.Datagrams {

    /// <summary>
    ///   Confirmation for <see cref="GW_GET_GROUP_INFORMATION_REQ"/>.</summary>
    public sealed class GW_GET_GROUP_INFORMATION_CFM : Klf200Datagram {

        /// <summary>
        ///   Initialize the command.</summary>
        public GW_GET_GROUP_INFORMATION_CFM() : base(Klf200Command.GW_GET_GROUP_INFORMATION_CFM, 2) { }

        /// <summary>
        ///   Information about the request state.</summary>
        public GW_InformationRequestState State {
            get => this.Data.ReadEnum<GW_InformationRequestState>(0, 1, GW_InformationRequestState.RequestAccepted);
        }

        /// <summary>
        ///   GroupID is the group index in the system table. It must be a value from 0 to 99.</summary>
        public Byte GroupId {
            get => this.Data.ReadByte(1);
        }

    }

}