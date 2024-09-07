using System;

namespace neleo_com.Logic.Bridges.Velux.Definitions {

    /// <summary>
    ///   Node states.</summary>
    public enum GW_NodeState : Byte {

        /// <summary>
        ///   This status information is only returned about an ACTIAVTE_FUNC, an 
        ///   ACTIVATE_MODE, an ACTIVATE_STATE or a WINK command. The parameter is 
        ///   unable to execute due to given conditions. An example can be that
        ///   the temperature is too high. It indicates that the parameter could not execute per
        ///   the contents of the present activate command.</summary>
        NonExecuting = 0,

        /// <summary>
        ///   This status information is only returned about an ACTIVATE_STATUS_REQ command. 
        ///   An error has occurred while executing. This error information will be cleared the
        ///   next time the parameter is going into ‘Waiting for executing’, ‘Waiting for power’ or 
        ///   ‘Executing’. A parameter can have the execute status ‘Error while executing’ only if 
        ///   the previous execute status was ‘Executing’. Note that this execute status gives 
        ///   information about the previous execution of the parameter, and gives no indication 
        ///   whether the following execution will fail.</summary>
        ErrorWhileExecution = 1,

        /// <summary>
        ///   No further details available.</summary>
        NotUsed = 2,

        /// <summary>
        ///   The parameter is waiting for power to proceed execution.</summary>
        WaitingForPower = 3,

        /// <summary>
        ///   Execution for the parameter is in progress.</summary>
        Executing = 4,

        /// <summary>
        ///   The parameter is not executing and no error has been detected. No activation of the 
        ///   parameter has been initiated. The parameter is ready for activation.</summary>
        Done = 5,

        /// <summary>
        ///   The state is unknown.</summary>
        StateUnknown = 255

    }

}
