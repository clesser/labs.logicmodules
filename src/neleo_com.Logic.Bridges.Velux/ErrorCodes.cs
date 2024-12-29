using System;

namespace neleo_com.Logic.Bridges.Velux {
    
    /// <summary>
    ///   Error codes that will be used to identify bubbling async errors.</summary>
    internal static class ErrorCodes {

        /// <summary>
        ///   Identifies an exception in the transport layer.</summary>
        public static readonly String SocketIoException = nameof(ErrorCodes.SocketIoException);

    }

}
