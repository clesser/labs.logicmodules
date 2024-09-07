using System;

namespace neleo_com.Logic.Bridges.Velux {

    /// <summary>
    ///   Extends the <see cref="Klf200Datagram"/> to import telegrams 
    ///   to configure the datagram. </summary>
    public interface IKlf200DatagramTelegramDecoder {

        /// <summary>
        ///   Tries to decompose the telegram's parameters to set the datagram's properties.</summary>
        /// <param name="telegram">
        ///   A telegram.</param>
        /// <param name="resolver">
        ///   Replace the name of a node/group/scene by it's identifier. Returns <see cref="Byte.MaxValue"/> if the name
        ///   can't be resolved.</param>
        /// <returns>
        ///   <c>true</c> if decomposition was successful (all required properties were set); otherwise <c>false</c>.</returns>
        Boolean DecodeTelegram(Klf200Telegram telegram, Func<Klf200TelegramScope, String, Byte> resolver);

    }

}
