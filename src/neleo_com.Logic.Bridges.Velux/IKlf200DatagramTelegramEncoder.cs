using System;

namespace neleo_com.Logic.Bridges.Velux {

    /// <summary>
    ///   Extends the <see cref="Klf200Datagram"/> to export telegrams 
    ///   using the datagram's properties. </summary>
    public interface IKlf200DatagramTelegramEncoder {

        /// <summary>
        ///   Creates a telegram out of this datagram's properties.</summary>
        /// <param name="resolver">
        ///   Replace the identifier of a node/group/scene by it's name. Returns <see cref="String.Empty"/> if the identifier
        ///   can't be resolved.</param>
        /// <returns>
        ///   A telegram.</returns>
        Klf200Telegram EncodeTelegram(Func<Klf200TelegramScope, Byte, String> resolver);

    }

}
