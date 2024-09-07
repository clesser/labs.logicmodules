using System;

namespace neleo_com.Logic.Bridges.Velux.Datagrams {

    /// <summary>
    ///   Confirmation for <see cref="GW_SET_UTC_REQ"/>.</summary>
    public sealed class GW_SET_UTC_CFM : Klf200Datagram {

        /// <summary>
        ///   Initialize the command.</summary>
        public GW_SET_UTC_CFM() : base(Klf200Command.GW_SET_UTC_CFM, 0) { }

    }

}
