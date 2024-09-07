using System;

namespace neleo_com.Logic.Bridges.Velux.Definitions {

    /// <summary>
    ///   <see cref="GW_COMMAND_RUN_STATUS_NTF"/> frame carries the current value of one parameter. 
    ///   The ParameterActive parameter in <see cref="GW_COMMAND_SEND_REQ"/> frame is used to indicate
    ///   which parameter status is requested for. Default let ParameterActive = MainParameter (). </summary>
    public enum GW_ParameterType : Byte {

        /// <summary>
        ///   Main Parameter.</summary>
        MainParameter = 0x00,

        /// <summary>
        ///   Functional Parameter 01.</summary>
        FunctionalParameter01 = 0x01,

        /// <summary>
        ///   Functional Parameter 02.</summary>
        FunctionalParameter02 = 0x02,

        /// <summary>
        ///   Functional Parameter 03.</summary>
        FunctionalParameter03 = 0x03,

        /// <summary>
        ///   Functional Parameter 04.</summary>
        FunctionalParameter04 = 0x04,

        /// <summary>
        ///   Functional Parameter 05.</summary>
        FunctionalParameter05 = 0x05,

        /// <summary>
        ///   Functional Parameter 06.</summary>
        FunctionalParameter06 = 0x06,

        /// <summary>
        ///   Functional Parameter 07.</summary>
        FunctionalParameter07 = 0x07,

        /// <summary>
        ///   Functional Parameter 08.</summary>
        FunctionalParameter08 = 0x08,

        /// <summary>
        ///   Functional Parameter 09.</summary>
        FunctionalParameter09 = 0x09,

        /// <summary>
        ///   Functional Parameter 10.</summary>
        FunctionalParameter10 = 0x0A,

        /// <summary>
        ///   Functional Parameter 11.</summary>
        FunctionalParameter11 = 0x0B,

        /// <summary>
        ///   Functional Parameter 12.</summary>
        FunctionalParameter12 = 0x0C,

        /// <summary>
        ///   Functional Parameter 13.</summary>
        FunctionalParameter13 = 0x0D,

        /// <summary>
        ///   Functional Parameter 14.</summary>
        FunctionalParameter14 = 0x0E,

        /// <summary>
        ///   Functional Parameter 15.</summary>
        FunctionalParameter15 = 0x0F,

        /// <summary>
        ///   Functional Parameter 16.</summary>
        FunctionalParameter16 = 0x10,

        /// <summary>
        ///   Parameter not used.</summary>
        NotUsed = 0xFF

    }

}
