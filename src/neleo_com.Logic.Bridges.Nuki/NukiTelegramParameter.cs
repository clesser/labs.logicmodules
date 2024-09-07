using System;

namespace neleo_com.Logic.Bridges.Nuki {

    /// <summary>
    ///   Enumeration of telegram parameters to ensure consistency.</summary>
    public enum NukiTelegramParameter : Byte {

        /// <summary>
        ///   Response of an action request.</summary>
        Success = 0x01,

        /// <summary>
        ///   Response of the lock state.</summary>
        Lock = 0x10,

        /// <summary>
        ///   Response of the door state.</summary>
        Door = 0x20,

        /// <summary>
        ///   Response of the battery state.</summary>
        Battery = 0x30

    }

}
