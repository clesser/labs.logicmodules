using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace neleo_com.Logic.Timing.Parser {

    /// <summary>
    ///   Implementation of an iCal/vCal parser.</summary>
    public class CalendarParser : Dictionary<String, ContentLine> {

        private const String ParametersPattern = "BEGIN:VCALENDAR\\r\\n(.+?)\\r\\nBEGIN:";
        private const RegexOptions ParametersOptions = RegexOptions.Singleline;

        private const String EventsPattern = "(BEGIN:VEVENT.+?END:VEVENT)";
        private const RegexOptions EventsOptions = RegexOptions.Singleline;

        private const String TimezonesPattern = "(BEGIN:VTIMEZONE.+?END:VTIMEZONE)";
        private const RegexOptions TimezonesOptions = RegexOptions.Singleline;

        /// <summary>
        ///   A collection of all timezones defined in the calendar.</summary>
        public ICollection<CalendarTimezone> Timezones {
            get; private set;
        } = new List<CalendarTimezone>();

        /// <summary>
        ///   A collection of all events defined in the calendar.</summary>
        public ICollection<CalendarEvent> Events {
            get; private set;
        } = new List<CalendarEvent>();

        /// <summary>
        ///   Creates a new calendar by parsing the <paramref name="source"/>.</summary>
        /// <param name="source">
        ///   The calendar definition as text.</param>
        public CalendarParser(String source) {

            Match parametersMatch = Regex.Match(source, CalendarParser.ParametersPattern, CalendarParser.ParametersOptions);

            String parametersString = parametersMatch.Groups[1].ToString();
            MatchCollection parametersMatches = Regex.Matches(parametersString, ContentLine.ContentPattern, ContentLine.ContentOptions);

            foreach (Match parameterMatch in parametersMatches) {

                String contentLineString = parameterMatch.Groups[0].ToString();
                ContentLine contentLine = new ContentLine(contentLineString);
                this[contentLine.Name] = contentLine;

            }

            foreach (Match timezoneMatch in Regex.Matches(source, CalendarParser.TimezonesPattern, CalendarParser.TimezonesOptions)) {

                String timezoneString = timezoneMatch.Groups[1].ToString();
                this.Timezones.Add(new CalendarTimezone(timezoneString));

            }

            foreach (Match eventMatch in Regex.Matches(source, CalendarParser.EventsPattern, CalendarParser.EventsOptions)) {

                String eventString = eventMatch.Groups[1].ToString();
                this.Events.Add(new CalendarEvent(eventString));

            }

        }

    }

}
