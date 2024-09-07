using System;

namespace neleo_com.Logic.Bridges.Velux.Datagrams {

    /// <summary>
    ///   Confirm reboot request.</summary>
    public sealed class GW_REBOOT_CFM : Klf200Datagram {

        /// <summary>
        ///   Initialize the command.</summary>
        public GW_REBOOT_CFM() : base(Klf200Command.GW_REBOOT_CFM, 0) { }

    }

}
