using System;

namespace neleo_com.Logic.Bridges.Velux {

    /// <summary>
    ///   Extends the <see cref="Klf200Datagram"/> to request session identifiers.</summary>
    public interface IKlf200DatagramSessionRequest {

        /// <summary>
        ///   Sets a session idenfier.</summary>
        UInt16 SessionId {
            set;
        }

    }

}
