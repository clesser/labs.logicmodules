using System;

namespace neleo_com.Logic.Bridges.Velux.Datagrams {

    /// <summary>
    ///   Request the state of the gateway.</summary>
    public sealed class GW_GET_STATE_REQ : Klf200Datagram {

        /// <summary>
        ///   Initialize the command.</summary>
        public GW_GET_STATE_REQ() : base(Klf200Command.GW_GET_STATE_REQ, 0) { }

    }

}
