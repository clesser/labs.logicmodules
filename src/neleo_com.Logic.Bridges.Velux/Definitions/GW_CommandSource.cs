using System;

namespace neleo_com.Logic.Bridges.Velux.Definitions {
    
    /// <summary>
    ///   Specifies the command originator type (USER/TIMER/SECURITY etc.) 
    ///   Typically, only USER or SAAC are used./summary>
    public enum  GW_CommandSource: Byte {

        /// <summary>
        ///   User Remote control causing action on actuator.</summary>
        User = 1,

        /// <summary>
        ///   Rain sensor.</summary>
        Rain = 2,

        /// <summary>
        ///   Timer controlled.</summary>
        Timer = 3,

        /// <summary>
        ///   Uninterruptible power supply unit.</summary>
        UPS = 5,

        /// <summary>
        ///   Stand Alone Automatic Controls.</summary>
        SAAC = 8,

        /// <summary>
        ///   Wind sensor.</summary>
        Wind = 9,

        /// <summary>
        ///   Managers for requiring a particular electric load shed.</summary>
        LoadShedding = 11,

        /// <summary>
        ///   Local light sensor.</summary>
        LocalLight = 12,

        /// <summary>
        ///   Used in context with commands transmitted on basis of an unknown sensor 
        ///   for protection of an end-product or house goods.</summary>
        UnspecificEnvironmentSensor = 13,

        /// <summary>
        ///   Used in context with emergency or security commands.</summary>
        Emergency = 255

    }

}
