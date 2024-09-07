using System;

namespace neleo_com.Logic.Bridges.Velux.Datagrams {

    /// <summary>
    ///   Confirmation for <see cref="GW_HOUSE_STATUS_MONITOR_ENABLE_REQ"/>.</summary>
    public sealed class GW_HOUSE_STATUS_MONITOR_ENABLE_CFM : Klf200Datagram {

        /// <summary>
        ///   Initialize the command.</summary>
        public GW_HOUSE_STATUS_MONITOR_ENABLE_CFM() : base(Klf200Command.GW_HOUSE_STATUS_MONITOR_ENABLE_CFM, 0) { }

    }

}
