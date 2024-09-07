using System;

namespace neleo_com.Logic.Bridges.Velux.Definitions {

    /// <summary>
    ///   Specifies the possible status owners./summary>
    public enum GW_RunStateOwner : Byte {

        /// <summary>
        ///   The status is from a user activation.</summary>
        User = 0x01,

        /// <summary>
        ///   The status is from a rain sensor activation.</summary>
        Rain = 0x02,

        /// <summary>
        ///   The status is from a timer generated action.</summary>
        Timer = 0x03,

        /// <summary>
        ///   The status is from a Uninterruptible power supply unit generated action.</summary>
        UPS = 0x05,

        /// <summary>
        ///   The status is from an automatic program generated action
        ///   (Stand Alone Automatic Controls).</summary>
        SAAC = 0x08,

        /// <summary>
        ///   The status is from a Wind sensor generated action.</summary>
        Wind = 0x09,

        /// <summary>
        ///   The status is from an actuator generated action.</summary>
        Myself = 0x0A,

        /// <summary>
        ///   The status is from a automatic cycle generated action.</summary>
        AutomaticCycle = 0x0B,

        /// <summary>
        ///   Used in context with emergency or security commands.</summary>
        Emergency = 0x0C,

        /// <summary>
        ///   The status is from an unknown command originator action.</summary>
        Unknown = 0xFF

    }

}
