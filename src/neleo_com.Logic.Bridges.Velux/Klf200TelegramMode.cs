using System;

namespace neleo_com.Logic.Bridges.Velux {

    /// <summary>
    ///   Definition of the telegram modes.</summary>
    public enum Klf200TelegramMode {

        /// <summary>
        ///   Mode is not specified.</summary>
        None,

        /// <summary>
        ///   Telegrams send to the Velux Klf-200 gateway.</summary>
        Request,

        /// <summary>
        ///   Telegrams received by the Velux Klf-200 gateway.</summary>
        Response,

        /// <summary>
        ///    Info - internally used to retrieve system table for debugging.</summary>
        Info

    }

}
