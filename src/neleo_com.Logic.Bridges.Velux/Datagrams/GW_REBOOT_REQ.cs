using System;

namespace neleo_com.Logic.Bridges.Velux.Datagrams {

    /// <summary>
    ///   Request the KLF-200 to reboot.</summary>
    public sealed class GW_REBOOT_REQ : Klf200Datagram {

        /// <summary>
        ///   Initialize the command.</summary>
        public GW_REBOOT_REQ() : base(Klf200Command.GW_REBOOT_REQ, 0) { }

    }

}
