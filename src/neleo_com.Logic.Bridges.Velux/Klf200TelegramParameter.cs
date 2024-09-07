using System;

namespace neleo_com.Logic.Bridges.Velux {

    /// <summary>
    ///   Enumeration of telegram parameters to ensure consistency.</summary>
    public enum Klf200TelegramParameter : Byte {

        /// <summary>
        ///   Current value of the main parameter of a node.</summary>
        Current = 0x00,

        Current01 = 0x01,
        Current02 = 0x02,
        Current03 = 0x03,
        Current04 = 0x04,
        Current05 = 0x05,
        Current06 = 0x06,
        Current07 = 0x07,
        Current08 = 0x08,
        Current09 = 0x09,
        Current10 = 0x0A,
        Current11 = 0x0B,
        Current12 = 0x0C,
        Current13 = 0x0D,
        Current14 = 0x0E,
        Current15 = 0x0F,
        Current16 = 0x10,

        /// <summary>
        ///   Target value of the main parameter of a node or group.</summary>
        Target = 0x20,

        Target01 = 0x21,
        Target02 = 0x22,
        Target03 = 0x23,
        Target04 = 0x24,
        Target05 = 0x25,
        Target06 = 0x26,
        Target07 = 0x27,
        Target08 = 0x28,
        Target09 = 0x29,
        Target10 = 0x2A,
        Target11 = 0x2B,
        Target12 = 0x2C,
        Target13 = 0x2D,
        Target14 = 0x2E,
        Target15 = 0x2F,
        Target16 = 0x30,

        /// <summary>
        ///   The remaining time in seconds to complete an action.</summary>
        Countdown = 0X40,

        Countdown01 = 0X41,
        Countdown02 = 0X42,
        Countdown03 = 0X43,
        Countdown04 = 0X44,
        Countdown05 = 0X45,
        Countdown06 = 0X46,
        Countdown07 = 0X47,
        Countdown08 = 0X48,
        Countdown09 = 0X49,
        Countdown10 = 0X4A,
        Countdown11 = 0X4B,
        Countdown12 = 0X4C,
        Countdown13 = 0X4D,
        Countdown14 = 0X4E,
        Countdown15 = 0X4F,
        Countdown16 = 0X50,

        /// <summary>
        ///   The minimum limitation (range) for a parameter.</summary>
        Min = 0X60,

        Min01 = 0X61,
        Min02 = 0X62,
        Min03 = 0X63,
        Min04 = 0X64,
        Min05 = 0X65,
        Min06 = 0X66,
        Min07 = 0X67,
        Min08 = 0X68,
        Min09 = 0X69,
        Min10 = 0X6A,
        Min11 = 0X6B,
        Min12 = 0X6C,
        Min13 = 0X6D,
        Min14 = 0X6E,
        Min15 = 0X6F,
        Min16 = 0X70,

        /// <summary>
        ///   The maximum limitation (range) for a parameter.</summary>
        Max = 0X80,

        Max01 = 0X81,
        Max02 = 0X82,
        Max03 = 0X83,
        Max04 = 0X84,
        Max05 = 0X85,
        Max06 = 0X86,
        Max07 = 0X87,
        Max08 = 0X88,
        Max09 = 0X89,
        Max10 = 0X8A,
        Max11 = 0X8B,
        Max12 = 0X8C,
        Max13 = 0X8D,
        Max14 = 0X8E,
        Max15 = 0X8F,
        Max16 = 0X90,

        /// <summary>
        ///   (Internal) The hardware type and variation of a node - or - type of a group.</summary>
        Type = 0xD0,

        /// <summary>
        ///   (Internal) The node identifiers that are associated with a group.</summary>
        Nodes = 0xD1,

        /// <summary>
        ///   The Command Originator of a request.</summary>
        Source = 0xE0,

        /// <summary>
        ///   The priority level of a request.</summary>
        Priority = 0xE1,

        /// <summary>
        ///   The velocity for nodes of a request.</summary>
        Velocity = 0xE2,

        /// <summary>
        ///   The execution state of a node.</summary>
        State = 0xF0,

        /// <summary>
        ///   Information about an error that occurred.</summary>
        Error = 0xF1,

        /// <summary>
        ///   More information about an error that occurred.</summary>
        ErrorInfo = 0xF2

    }

}
