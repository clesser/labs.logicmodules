using System;

using neleo_com.Logic.Bridges.Velux.Definitions;

namespace neleo_com.Logic.Bridges.Velux.Datagrams {

    /// <summary>
    ///   Confirmation for <see cref="GW_GET_STATE_REQ"/>.</summary>
    public sealed class GW_GET_STATE_CFM : Klf200Datagram {

        /// <summary>
        ///   Initialize the command.</summary>
        public GW_GET_STATE_CFM() : base(Klf200Command.GW_GET_STATE_CFM, 6) { }

        /// <summary>
        ///   Informs about the gateway state.</summary>
        public GW_GatewayState GatewayState {

            get => this.Data.ReadEnum<GW_GatewayState>(0, 1, GW_GatewayState.TestMode);

        }

    }

}
