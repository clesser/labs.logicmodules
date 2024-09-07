using System;

namespace neleo_com.Logic.Bridges.Velux.Datagrams {

    /// <summary>
    ///   House Status Monitor service will be disabled.</summary>
    public sealed class GW_HOUSE_STATUS_MONITOR_DISABLE_REQ : Klf200Datagram {

        /// <summary>
        ///   Initialize the command.</summary>
        public GW_HOUSE_STATUS_MONITOR_DISABLE_REQ() : base(Klf200Command.GW_HOUSE_STATUS_MONITOR_DISABLE_REQ, 0) { }

    }

}
