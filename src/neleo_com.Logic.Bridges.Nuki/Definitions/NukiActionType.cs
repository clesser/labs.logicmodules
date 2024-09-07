using System;

namespace neleo_com.Logic.Bridges.Nuki.Definitions {

    /// <summary>
    ///   Definition of actions that a Nuki device can handle.</summary>
    public enum NukiActionType: Byte {

        /// <summary>
        ///   Default; ignore.</summary>
        None = 0,

        /// <summary>
        ///   Lock action.</summary>
        Lock = 2,

        /// <summary>
        ///   Unlock action.</summary>
        Unlock = 1,

        /// <summary>
        ///   Unlatch action.</summary>
        Unlatch = 3,

        /// <summary>
        ///   Request device state.</summary>
        Info = 255

    }

}
