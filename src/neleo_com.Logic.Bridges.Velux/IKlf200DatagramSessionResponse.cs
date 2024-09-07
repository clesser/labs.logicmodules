using System;

namespace neleo_com.Logic.Bridges.Velux {

    /// <summary>
    ///   Extends the <see cref="Klf200Datagram"/> to respond with session identifiers.</summary>
    public interface IKlf200DatagramSessionResponse {

        /// <summary>
        ///   Gets a session idenfier.</summary>
        UInt16 SessionId {
            get;
        }

        /// <summary>
        ///   Gets an indicator whether this is the final datagram for this session.</summary>
        Boolean IsFinal {
            get;
        }

    }

}
