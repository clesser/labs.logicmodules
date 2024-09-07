using System;

namespace neleo_com.Logic.Bridges.Velux.Datagrams {

    /// <summary>
    ///   Enter password to authenticate request.</summary>
    public sealed class GW_PASSWORD_ENTER_REQ : Klf200Datagram {

        /// <summary>
        ///   Max number of characters for a password.</summary>
        private const Byte MaxPasswordSize = 32;

        /// <summary>
        ///   Initialize the command.</summary>
        /// <param name="password">
        ///   The password.</param>
        public GW_PASSWORD_ENTER_REQ(String password) : base(Klf200Command.GW_PASSWORD_ENTER_REQ, MaxPasswordSize) {

            this.Data.WriteString(password, 0, MaxPasswordSize);

        }

    }

}