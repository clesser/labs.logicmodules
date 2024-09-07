using System;

namespace neleo_com.Logic.Bridges.Velux.Datagrams {

    /// <summary>
    ///   House Status Monitor service will be enabled.</summary>
    public sealed class GW_HOUSE_STATUS_MONITOR_ENABLE_REQ : Klf200Datagram {

        /// <summary>
        ///   Initialize the command.</summary>
        public GW_HOUSE_STATUS_MONITOR_ENABLE_REQ() : base(Klf200Command.GW_HOUSE_STATUS_MONITOR_ENABLE_REQ, 0) { }

    }

}
