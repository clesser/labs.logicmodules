using System;

namespace neleo_com.Logic.Bridges.Velux.Datagrams {

    /// <summary>
    ///   The gateway has a real-time clock running at UTC. The client can set a local time zone 
    ///   and daylight savings rules. The UTC time must be set every time the gateway is powered on.</summary>
    public sealed class GW_SET_UTC_REQ : Klf200Datagram {

        /// <summary>
        ///   Initialize the command.</summary>
        public GW_SET_UTC_REQ(DateTime utcDateTime) : base(Klf200Command.GW_SET_UTC_REQ, 4) {

            this.Data.WriteDateTime(utcDateTime, 0);

        }

    }

}
