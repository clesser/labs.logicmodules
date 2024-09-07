using System;

namespace neleo_com.Logic.Bridges.Velux.Definitions {

    /// <summary>
    ///   Request confirmation states.</summary>
    public enum GW_SceneCommandRequestState : Byte {

        /// <summary>
        ///   OK - Request accepted.</summary>
        RequestAccepted = 0,

        /// <summary>
        ///   Error – Request rejected; invalid identifier.</summary>
        RequestRejectedInvalidId = 1,

        /// <summary>
        ///   Error – Request rejected.</summary>
        RequestRejected = 2

    }

}
