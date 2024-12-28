using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace neleo_com.Logic.Timing.Parser {

    /// <summary>
    ///   An event in a calendar.</summary>
    public class CalendarEvent : Dictionary<String, ContentLine> {

        private const String ContentPattern = "BEGIN:VEVENT\\r\\n(.+)\\r\\nEND:VEVENT";
        private const RegexOptions ContentOptions = RegexOptions.Singleline;

        /// <summary>
        ///   The timezone (can be "#UTC" or "#LOCAL") of the event's begin date/time.</summary>
        public String StartDateTimeTzId {
            get; private set;
        }

        /// <summary>
        ///   The local (local -> <see cref="StartDateTimeTzId"/>) begin date/time of the event.</summary>
        public DateTime StartDateTime {
            get; private set;
        }

        /// <summary>
        ///   The timezone (can be "#UTC" or "#LOCAL") of the event's end date/time.</summary>
        public String EndDateTimeTzId {
            get; private set;
        }

        /// <summary>
        ///   The local (local -> <see cref="EndDateTimeTzId"/>) end date/time of the event.</summary>
        public DateTime EndDateTime {
            get; private set;
        }

        /// <summary>
        ///   The subject of the event.</summary>
        public String Subject {
            get; private set;
        }

        /// <summary>
        ///   The location of the event.</summary>
        public String Location {
            get; private set;
        }

        /// <summary>
        ///   Creates a new calendar event by parsing the <paramref name="source"/>.</summary>
        /// <param name="source">
        ///   The event configuration as text.</param>
        public CalendarEvent(String source) {

            // extract all parameters
            Match contentMatch =
                Regex.Match(source, CalendarEvent.ContentPattern, CalendarEvent.ContentOptions);

            String content = contentMatch.Groups[1].ToString();
            MatchCollection matches = Regex.Matches(content, ContentLine.ContentPattern, ContentLine.ContentOptions);

            foreach (Match match in matches) {

                String contentLineString = match.Groups[0].ToString();
                ContentLine contentLine = new ContentLine(contentLineString);
                this[contentLine.Name] = contentLine;

            }

            // expose specific parameters
            if (this.ContainsKey("LOCATION"))
                this.Location = this["LOCATION"].Value;

            if (this.ContainsKey("SUMMARY"))
                this.Subject = this["SUMMARY"].Value;

            if (this.ContainsKey("DTEND")) {

                ContentLine contentLine = this["DTEND"];
                String contentLineValue = contentLine.Value;

                try {

                    // extract timezone information (UTC, specific or unspecific)
                    if (contentLineValue.EndsWith("Z"))
                        this.EndDateTimeTzId = "#UTC";
                    else if (contentLine.Parameters.ContainsKey("TZID"))
                        this.EndDateTimeTzId = contentLine.Parameters["TZID"].FirstOrDefault();
                    else
                        this.EndDateTimeTzId = "#LOCAL";

                    // extract date/time
                    if (contentLine.Parameters.Contains("VALUE", "DATE") && contentLineValue.Length == 8)
                        this.EndDateTime = contentLineValue.ToDate();
                    else if (contentLineValue.Length >= 15)
                        this.EndDateTime = contentLineValue.ToDateTime();
                    else
                        this.EndDateTime = DateTime.MinValue;

                }
                catch (ArgumentException) {

                    this.EndDateTime = DateTime.MinValue;

                }

            }

            if (this.ContainsKey("DTSTART")) {

                ContentLine contentLine = this["DTSTART"];
                String contentLineValue = contentLine.Value;

                try {

                    // extract timezone information (UTC, specific or unspecific)
                    if (contentLineValue.EndsWith("Z"))
                        this.StartDateTimeTzId = "#UTC";
                    else if (contentLine.Parameters.ContainsKey("TZID"))
                        this.StartDateTimeTzId = contentLine.Parameters["TZID"].FirstOrDefault();
                    else
                        this.StartDateTimeTzId = "#LOCAL";

                    // extract date/time
                    if (contentLine.Parameters.Contains("VALUE", "DATE") && contentLineValue.Length == 8)
                        this.StartDateTime = contentLineValue.ToDate();
                    else if (contentLineValue.Length >= 15)
                        this.StartDateTime = contentLineValue.ToDateTime();
                    else
                        this.StartDateTime = DateTime.MinValue;


                }
                catch (ArgumentException) {

                    this.StartDateTime = DateTime.MinValue;

                }

            }

        }

    }

}
