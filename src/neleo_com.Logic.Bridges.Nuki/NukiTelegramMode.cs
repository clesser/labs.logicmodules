using System;

namespace neleo_com.Logic.Bridges.Nuki {

    /// <summary>
    ///   Definition of the telegram modes.</summary>
    public enum NukiTelegramMode {

        /// <summary>
        ///   Mode is not specified.</summary>
        None,

        /// <summary>
        ///   Telegrams send to the Nuki gateway.</summary>
        Request,

        /// <summary>
        ///   Telegrams received from the Nuki gateway.</summary>
        Response

    }

}
