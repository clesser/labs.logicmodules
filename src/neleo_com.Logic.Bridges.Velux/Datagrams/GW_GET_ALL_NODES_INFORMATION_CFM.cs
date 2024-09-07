using System;

namespace neleo_com.Logic.Bridges.Velux.Datagrams {

    /// <summary>
    ///   Confirmation for <see cref="GW_GET_ALL_NODES_INFORMATION_REQ"/>.</summary>
    public sealed class GW_GET_ALL_NODES_INFORMATION_CFM : Klf200Datagram {

        /// <summary>
        ///   Initialize the command.</summary>
        public GW_GET_ALL_NODES_INFORMATION_CFM() : base(Klf200Command.GW_GET_ALL_NODES_INFORMATION_CFM, 2) { }

        /// <summary>
        ///   <c>true</c>, if the table has no items (nodes; <see cref="TableRowCount"/> == 0).</summary>
        public Boolean TableIsEmpty {

            get => this.Data.ReadByte(0) == 1;

        }

        /// <summary>
        ///   Gets the number of rows (items) in the node table.</summary>
        public Byte TableRowCount {

            get => this.Data.ReadByte(1);

        }

    }

}
