using System;

namespace neleo_com.Logic.Bridges.Velux.Definitions {

    /// <summary>
    ///   Velux KLF-200 gateway states.</summary>
    public enum GW_GatewayState : Byte {

        /// <summary>
        ///   Test mode</summary>
        TestMode = 0,

        /// <summary>
        ///   Gateway mode, no actuator nodes in the system table</summary>
        GatewayMode_EmptyTable = 1,

        /// <summary>
        ///   Gateway mode, with one or more actuator nodes in the system table</summary>
        GatewayMode_FilledTable = 2,

        /// <summary>
        ///   Beacon mode, not configured by a remote controller</summary>
        BeaconMode_NotConfigured = 3,

        /// <summary>
        ///   Beacon mode, has been configured by a remote controller</summary>
        BeconMode_IsConfigured = 4

    }

}
