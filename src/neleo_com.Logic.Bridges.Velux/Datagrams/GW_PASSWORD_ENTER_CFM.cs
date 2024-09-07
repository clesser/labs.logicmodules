using System;

namespace neleo_com.Logic.Bridges.Velux.Datagrams {

    /// <summary>
    ///   Confirm authenticate request.</summary>
    public sealed class GW_PASSWORD_ENTER_CFM : Klf200Datagram {

        /// <summary>
        ///   Initialize the command.</summary>
        public GW_PASSWORD_ENTER_CFM() : base(Klf200Command.GW_PASSWORD_ENTER_CFM, 1) { }

        /// <summary>
        ///   Authentication was successful.</summary>
        public Boolean Success {

            get => this.Data.ReadByte(0) == 0;

        }

    }

}
