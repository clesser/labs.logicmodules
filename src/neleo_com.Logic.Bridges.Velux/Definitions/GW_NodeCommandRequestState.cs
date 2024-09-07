using System;

namespace neleo_com.Logic.Bridges.Velux.Definitions {

    /// <summary>
    ///   Request confirmation states.</summary>
    public enum GW_NodeCommandRequestState : Byte {

        /// <summary>
        ///   Error – Request rejected.</summary>
        RequestRejected = 0,

        /// <summary>
        ///   OK - Request accepted.</summary>
        RequestAccepted = 1

    }

}
