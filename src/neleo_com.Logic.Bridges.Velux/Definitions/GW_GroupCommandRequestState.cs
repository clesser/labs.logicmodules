using System;

namespace neleo_com.Logic.Bridges.Velux.Definitions {

    /// <summary>
    ///   Request confirmation states.</summary>
    public enum GW_GroupCommandRequestState : Byte {

        /// <summary>
        ///   OK - Request accepted.</summary>
        RequestAccepted = 0,

        /// <summary>
        ///   Unknown ProductGroupID..</summary>
        GroupIdError = 1,

        /// <summary>
        ///   SessionID already in use.</summary>
        SessionError = 2,

        /// <summary>
        ///   Busy, all activation slot in use.</summary>
        BusyError = 3,

        /// <summary>
        ///   Wrong group type.</summary>
        GroupTypeError = 4,

        /// <summary>
        ///   Not further defined error.</summary>
        UndefinedError = 5,

        /// <summary>
        ///   Invalid parameter used.</summary>
        ParameterError = 6

    }

}
