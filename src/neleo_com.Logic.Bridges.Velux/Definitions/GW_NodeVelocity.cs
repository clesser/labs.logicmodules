using System;

namespace neleo_com.Logic.Bridges.Velux.Definitions {

    /// <summary>
    ///   Node velocity definitions.</summary>
    public enum GW_NodeVelocity : Byte {

        /// <summary>
        ///   The node operates by its default velocity.</summary>
        Default = 0,

        /// <summary>
        ///   The node operates in silent mode (slow).</summary>
        Silent = 1,

        /// <summary>
        ///   The node operates with fast velocity.</summary>
        Fast = 2,

        /// <summary>
        ///   Not supported by node.</summary>
        Undefined = 255

    }

}
