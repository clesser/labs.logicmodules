using System;

namespace neleo_com.Logic.Bridges.Velux.Definitions {

    /// <summary>
    ///   Generic Velux KLF-200 gateway error messages (may occur at any time).</summary>
    public enum GW_GatewayError : Byte {

        /// <summary>
        ///   Not further defined error.</summary>
        Undefined = 0,

        /// <summary>
        ///   Unknown Command or command is not accepted at this state.</summary>
        UnknownCommand = 1,

        /// <summary>
        ///   ERROR on Frame Structure.</summary>
        FrameStructure = 2,

        /// <summary>
        ///   Busy. Try again later. </summary>
        DeviceBusy = 7,

        /// <summary>
        ///   Bad system table index.</summary>
        TableContent = 8,

        /// <summary>
        ///   Not authenticated.</summary>
        Authentication = 12

    }

}
