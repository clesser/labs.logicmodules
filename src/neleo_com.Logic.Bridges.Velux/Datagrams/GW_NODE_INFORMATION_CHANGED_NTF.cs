using System;

using neleo_com.Logic.Bridges.Velux.Definitions;

namespace neleo_com.Logic.Bridges.Velux.Datagrams {

    /// <summary>
    ///   This event holds the information on a modified node.</summary>
    public sealed class GW_NODE_INFORMATION_CHANGED_NTF : Klf200Datagram {

        /// <summary>
        ///   Initialize the command.</summary>
        public GW_NODE_INFORMATION_CHANGED_NTF() : base(Klf200Command.GW_NODE_INFORMATION_CHANGED_NTF, 69) { }

        /// <summary>
        ///   NodeID is an Actuator index in the system table, to get information from. 
        ///   It must be a value from 0 to 199.</summary>
        public Byte NodeId {
            get => this.Data.ReadByte(0);
        }

        /// <summary>
        ///   This field Name holds the name of the actuator, ex. “Window 1”./// </summary>
        public String Name {
            get => this.Data.ReadString(1, 64);
        }

        /// <summary>
        ///   Order can be used to store a sort order. The sort order is used in client end, 
        ///   when presenting a list of nodes for the user.</summary>
        public UInt16 Order {
            get => this.Data.ReadUInt16(65);
        }

        /// <summary>
        ///   Placement can be used to store a room group index or house group index number.</summary>
        public Byte Placement {
            get => this.Data.ReadByte(67);
        }

        /// <summary>
        ///   The node variation.</summary>
        public GW_NodeVariation NodeVariation {
            get => this.Data.ReadEnum<GW_NodeVariation>(68, 1, GW_NodeVariation.NotSet);
        }

    }

}
