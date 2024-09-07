using System;

namespace neleo_com.Logic.Bridges.Velux.Definitions {

    /// <summary>
    ///   Contains the execution status of the node.</summary>
    public enum GW_RunState : Byte {

        /// <summary>
        ///   Execution is completed with no errors.</summary>
        Completed = 0,

        /// <summary>
        ///   Execution has failed.</summary>
        Failed = 1,

        /// <summary>
        ///   Execution is still active.</summary>
        Active = 2

    }

}
