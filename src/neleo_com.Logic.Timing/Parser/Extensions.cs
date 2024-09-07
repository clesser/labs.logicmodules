using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace neleo_com.Logic.Timing.Parser {

    /// <summary>
    ///   A collection of extension methods.</summary>
    public static class Extensions {

        /// <summary>
        ///   Unfolds the multi-line text into a single-line text.</summary>
        /// <param name="source">
        ///   A multi-line source.</param>
        /// <returns>
        ///   A single-line result.</returns>
        public static String UnfoldAndUnescape(this String source) {

            if (source == null)
                throw new ArgumentNullException(nameof(source));

            if (String.IsNullOrWhiteSpace(source))
                return String.Empty;

            String unfold = Regex.Replace(source, "(\\r\\n )", "");
            String unescaped = Regex.Unescape(unfold);
            return unescaped;

        }

        /// <summary>
        ///   Converts an iCal/vCal date/time string into date and time.</summary>
        /// <param name="source">
        ///   The source.</param>
        /// <returns>
        ///   The <paramref name="source"/> as <see cref="DateTime"/>.</returns>
        public static DateTime ToDateTime(this String source) {

            if (String.IsNullOrWhiteSpace(source) || source.Length < 15 || source.Length > 16)
                throw new ArgumentException(nameof(source));

            return new DateTime(
                Int32.Parse(source.Substring(0, 4)),
                Int32.Parse(source.Substring(4, 2)),
                Int32.Parse(source.Substring(6, 2)),
                Int32.Parse(source.Substring(9, 2)),
                Int32.Parse(source.Substring(11, 2)),
                Int32.Parse(source.Substring(13, 2)),
                source.EndsWith("Z") ? DateTimeKind.Utc : DateTimeKind.Unspecified);

        }

        /// <summary>
        ///   Converts an iCal/vCal date string into a date.</summary>
        /// <param name="source">
        ///   The source.</param>
        /// <returns>
        ///   The <paramref name="source"/> as <see cref="DateTime"/>.</returns>
        public static DateTime ToDate(this String source) {

            if (String.IsNullOrWhiteSpace(source) || source.Length != 8)
                throw new ArgumentException(nameof(source));

            return new DateTime(
                Int32.Parse(source.Substring(0, 4)),
                Int32.Parse(source.Substring(4, 2)),
                Int32.Parse(source.Substring(6, 2)));

        }

        /// <summary>
        ///   Converts an iCal/vCal date/time string into the time.</summary>
        /// <param name="source">
        ///   The source.</param>
        /// <returns>
        ///   The <paramref name="source"/> as <see cref="DateTime"/>/time of day.</returns>
        public static DateTime ToTime(this String source) {

            return (new DateTime(1, 1, 1)).Add(source.ToDateTime().TimeOfDay);

        }

        /// <summary>
        ///   Converts an iCal/vCal timezone offset into a time span.</summary>
        /// <param name="source">
        ///   The source.</param>
        /// <returns>
        ///   The <paramref name="source"/> as <see cref="TimeSpan"/>.</returns>
        public static TimeSpan ToTimeSpan(this String source) {

            if (String.IsNullOrWhiteSpace(source))
                throw new ArgumentNullException(nameof(source));

            if ((source.StartsWith("+") || source.StartsWith("-")) && source.Length == 5)
                return new TimeSpan(
                    Int32.Parse(source.Substring(0, 3)),
                    Int32.Parse(source.Substring(3, 2)),
                    0);
            else if (source.Length == 6)
                return new TimeSpan(
                    Int32.Parse(source.Substring(0, 2)),
                    Int32.Parse(source.Substring(2, 2)),
                    Int32.Parse(source.Substring(4, 2)));
            else
                return TimeSpan.Parse(source);
        }


        /// <summary>
        ///   Converts a collection of key-value-pairs into a <see cref="Dictionary{String, String}"/>.</summary>
        /// <param name="source">
        ///   The source.</param>
        /// <param name="keyValuePairSeparator">
        ///   The key-value-pair separator.</param>
        /// <param name="keyValueSeparator">
        ///   The key-value separator.</param>
        /// <returns>
        ///   A dictionary.</returns>
        public static Dictionary<String, String> ToDictionary(this String source, Char keyValuePairSeparator, Char keyValueSeparator) {

            if (String.IsNullOrWhiteSpace(source))
                throw new ArgumentException(nameof(source));

            Dictionary<String, String> dictionary = new Dictionary<String, String>();
            IEnumerable<String> keyValuePairs = source.Split(new Char[] { keyValuePairSeparator });
            foreach (String keyValuePair in keyValuePairs) {

                String[] keyValue = keyValuePair.Split(new Char[] { keyValueSeparator });
                if (keyValue.Length == 2)
                    dictionary[keyValue[0]] = keyValue[1];

            }

            return dictionary;

        }

        /// <summary>
        ///   Converts a two-letter-week-day into a <see cref="DayOfWeek"/>.</summary>
        /// <param name="source">
        ///   The source.</param>
        /// <param name="fallback">
        ///   A fallback value.</param>
        /// <returns>
        ///   The day of the week.</returns>
        public static DayOfWeek ToDayOfWeek(this String source, DayOfWeek fallback) {

            if (String.IsNullOrWhiteSpace(source))
                return fallback;

            switch (source.ToUpperInvariant()) {

                case "MO":
                    return DayOfWeek.Monday;

                case "TU":
                    return DayOfWeek.Tuesday;

                case "WE":
                    return DayOfWeek.Wednesday;

                case "TH":
                    return DayOfWeek.Thursday;

                case "FR":
                    return DayOfWeek.Friday;

                case "SA":
                    return DayOfWeek.Saturday;

                case "SU":
                    return DayOfWeek.Sunday;

                default:
                    return fallback;

            }

        }

    }

}
