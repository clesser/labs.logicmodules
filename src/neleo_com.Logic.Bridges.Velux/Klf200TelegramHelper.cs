using System;
using System.Linq;

namespace neleo_com.Logic.Bridges.Velux {

    /// <summary>
    ///   Helper methods to pretty-print parameters in a telegram.</summary>
    public static class Klf200TelegramHelper {

        /// <summary>
        ///   Converts a String to camelCase format.</summary>
        /// <param name="value">
        ///   A String.</param>
        /// <returns>
        ///   The converted String.</returns>
        public static String ToCamelCase(this String value) {

            // see https://code-maze.com/csharp-convert-string-titlecase-camelcase/

            if (String.IsNullOrWhiteSpace(value))
                return String.Empty;

            var words = value.Split(new[] { "_", " " }, StringSplitOptions.RemoveEmptyEntries);

            if (words.Length == 1) {

                var word = words[0];
                return $"{Char.ToLowerInvariant(word[0])}{word.Substring(1)}";

            }
            else {

                var leadWord = words[0].ToLower();

                var tailWords = words.Skip(1)
                    .Select(word => char.ToUpper(word[0]) + word.Substring(1))
                    .ToArray();

                return $"{leadWord}{string.Join(string.Empty, tailWords)}";

            }

        }

        /// <summary>
        ///   Converts a parameter into an enumeration.</summary>
        /// <typeparam name="T">
        ///   The type of the enumeration.</typeparam>
        /// <param name="telegram">
        ///   The telegram.</param>
        /// <param name="parameter">
        ///   The name of the parameter.</param>
        /// <param name="defaultValue">
        ///   The default value.</param>
        /// <returns>
        ///   The value of the specified parameter or the default value.</returns>
        public static T ParseEnumParameter<T>(this Klf200Telegram telegram, Klf200TelegramParameter parameter, T defaultValue) where T : Enum {

            String value;
            if (telegram.Parameters.ContainsKey(parameter))
                value = telegram.Parameters[parameter];
            else
                return defaultValue;

            if (Enum.IsDefined(typeof(T), value))
                return (T)Enum.Parse(typeof(T), value);
            else
                return defaultValue;

        }

        /// <summary>
        ///   Converts a parameter into an unsigned 2-byte integer.</summary>
        /// <param name="telegram">
        ///   The telegram.</param>
        /// <param name="parameter">
        ///   The name of the parameter.</param>
        /// <param name="defaultValue">
        ///   The default value.</param>
        /// <returns>
        ///   The value of the specified parameter or the default value.</returns>
        public static UInt16 ParseUInt16Parameter(this Klf200Telegram telegram, Klf200TelegramParameter parameter, UInt16 defaultValue) {

            String value;
            if (telegram.Parameters.ContainsKey(parameter))
                value = telegram.Parameters[parameter];
            else
                return defaultValue;

            if (UInt16.TryParse(value, out UInt16 number))
                return number;
            else
                return defaultValue;

        }

    }

}
