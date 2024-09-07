using System;

using neleo_com.Logic.Bridges.Velux.Definitions;

namespace neleo_com.Logic.Bridges.Velux.Datagrams {

    /// <summary>
    ///   Provides information about what triggered the error.</summary>
    public sealed class GW_ERROR_NTF : Klf200Datagram {

        /// <summary>
        ///   Initialize the command.</summary>
        public GW_ERROR_NTF() : base(Klf200Command.GW_ERROR_NTF, 1) { }

        /// <summary>
        ///   Informs about the gateway error.</summary>
        public GW_GatewayError GatewayError {

            get => this.Data.ReadEnum<GW_GatewayError>(1, 1, GW_GatewayError.Undefined);

        }

    }

}
