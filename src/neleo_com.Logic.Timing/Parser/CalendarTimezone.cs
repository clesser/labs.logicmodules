using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace neleo_com.Logic.Timing.Parser {

    /// <summary>
    ///   A timezone definition in a calendar.</summary>
    /// <remarks>
    ///   Timezone defintions may include multiple Stadndard and Daylight Saving Time definitions per Timezone object.
    ///   This implemenation handes just one for now...</remarks>
    public class CalendarTimezone : Dictionary<String, ContentLine> {

        private const String ContentPattern = "BEGIN:VTIMEZONE\\r\\n(.+)\\r\\nEND:VTIMEZONE";
        private const RegexOptions ContentOptions = RegexOptions.Singleline;

        private const String TimezonePrmContentPattern = "BEGIN:VTIMEZONE\\r\\n(.+?)\\r\\nBEGIN:";
        private const RegexOptions TimezonePrmContentOptions = RegexOptions.Singleline;

        private const String TimezoneStdContentPattern = "BEGIN:STANDARD\\r\\n(.+)\\r\\nEND:STANDARD";
        private const RegexOptions TimezoneStdContentOptions = RegexOptions.Singleline;

        private const String TimezoneDlsContentPattern = "BEGIN:DAYLIGHT\\r\\n(.+)\\r\\nEND:DAYLIGHT";
        private const RegexOptions TimezoneDlsContentOptions = RegexOptions.Singleline;

        /// <summary>
        ///   A collection of parameters to describe the Standard Time Settings.</summary>
        public IDictionary<String, ContentLine> StandardTimeParameters {
            get; private set;
        }

        /// <summary>
        ///   A collection of parameters to describe the Daylight Saving Time Settings.</summary>
        public IDictionary<String, ContentLine> DaylightSavingTimeParameters {
            get; private set;
        }

        /// <summary>
        ///   A .Net friendly representation of the timezone definition.</summary>
        public TimeZoneInfo AsTimeZoneInfo {
            get; private set;
        }

        /// <summary>
        ///   Creates a new timezone specification based on <paramref name="source"/>.</summary>
        /// <param name="source">
        ///   A timezone specification.</param>
        public CalendarTimezone(String source) {

            Match contentPrmMatch = Regex.Match(source, TimezonePrmContentPattern, TimezonePrmContentOptions);
            string contentPrm = contentPrmMatch.Groups[1].ToString();

            MatchCollection matchesPrm = Regex.Matches(contentPrm, ContentLine.ContentPattern, ContentLine.ContentOptions);
            foreach (Match match in matchesPrm) {
                string contentLineString = match.Groups[0].ToString();
                ContentLine contentLine = new ContentLine(contentLineString);
                this[contentLine.Name] = contentLine;
            }

            Match contentMatch = Regex.Match(source, ContentPattern, ContentOptions);
            string content = contentMatch.Groups[1].ToString();

            Match contentStdMatch = Regex.Match(content, TimezoneStdContentPattern, TimezoneStdContentOptions);
            string contentStd = contentStdMatch.Groups[1].ToString();

            MatchCollection matchesStd = Regex.Matches(contentStd, ContentLine.ContentPattern, ContentLine.ContentOptions);
            StandardTimeParameters = new Dictionary<string, ContentLine>();
            foreach (Match match in matchesStd) {
                string contentLineString = match.Groups[0].ToString();
                ContentLine contentLine = new ContentLine(contentLineString);
                StandardTimeParameters[contentLine.Name] = contentLine;
            }

            Match contentDlsMatch = Regex.Match(content, TimezoneDlsContentPattern, TimezoneDlsContentOptions);
            string contentDls = contentDlsMatch.Groups[1].ToString();

            MatchCollection matchesDls = Regex.Matches(contentDls, ContentLine.ContentPattern, ContentLine.ContentOptions);
            DaylightSavingTimeParameters = new Dictionary<string, ContentLine>();
            foreach (Match match in matchesDls) {
                string contentLineString = match.Groups[0].ToString();
                ContentLine contentLine = new ContentLine(contentLineString);
                DaylightSavingTimeParameters[contentLine.Name] = contentLine;
            }

            /*
                BEGIN:VTIMEZONE
                TZID:W. Europe Standard Time

                BEGIN:STANDARD
                DTSTART:16010101T030000
                TZOFFSETFROM:+0200
                TZOFFSETTO:+0100
                RRULE:FREQ=YEARLY;INTERVAL=1;BYDAY=-1SU;BYMONTH=10
                END:STANDARD

                BEGIN:DAYLIGHT
                DTSTART:16010101T020000
                TZOFFSETFROM:+0100
                TZOFFSETTO:+0200
                RRULE:FREQ=YEARLY;INTERVAL=1;BYDAY=-1SU;BYMONTH=3
                END:DAYLIGHT

                END:VTIMEZONE
            */

            // extract identifier or stop processing if there's no identifier defined
            String timezoneId;
            if (this.ContainsKey("TZID"))
                timezoneId = this["TZID"].Value;
            else
                return;

            // ensure that default offset is available or stop further processing
            TimeSpan defaultOffset;
            if (this.StandardTimeParameters.ContainsKey("TZOFFSETTO"))
                defaultOffset = this.StandardTimeParameters["TZOFFSETTO"].Value.ToTimeSpan();
            else if (this.DaylightSavingTimeParameters.ContainsKey("TZOFFSETFROM"))
                defaultOffset = this.DaylightSavingTimeParameters["TZOFFSETFROM"].Value.ToTimeSpan();
            else
                return;

            // extract parameters
            String stdDisplayName = String.Format("{0} (STD)", timezoneId);
            String dstDisplayName = String.Format("{0} (DST)", timezoneId);

            TimeZoneInfo.TransitionTime? stdTransitionTime = this.GetTransitionTime(this.StandardTimeParameters);
            TimeZoneInfo.TransitionTime? dstTransitionTime = this.GetTransitionTime(this.DaylightSavingTimeParameters);

            TimeSpan? stdTransitionDelta = this.GetTransitionDelta(this.StandardTimeParameters);
            TimeSpan? dstTransitionDelta = this.GetTransitionDelta(this.DaylightSavingTimeParameters);

            DateTime? stdTransitionDate = this.GetTransitionDate(this.StandardTimeParameters);
            DateTime? dstTransitionDate = this.GetTransitionDate(this.DaylightSavingTimeParameters);

            // compose timezone info based on data availability
            if (stdTransitionTime.HasValue && stdTransitionDelta.HasValue && stdTransitionDate.HasValue
                && dstTransitionTime.HasValue && dstTransitionDelta.HasValue && dstTransitionDate.HasValue) {

                TimeZoneInfo.AdjustmentRule adjustmentRule = TimeZoneInfo.AdjustmentRule.CreateAdjustmentRule(stdTransitionDate.Value, DateTime.MaxValue.Date, dstTransitionDelta.Value, dstTransitionTime.Value, stdTransitionTime.Value);

                TimeZoneInfo.AdjustmentRule[] adjustmentRules = { adjustmentRule };

                this.AsTimeZoneInfo = TimeZoneInfo.CreateCustomTimeZone(timezoneId, defaultOffset, timezoneId, stdDisplayName, dstDisplayName, adjustmentRules);

            }
            else {

                this.AsTimeZoneInfo = TimeZoneInfo.CreateCustomTimeZone(timezoneId, defaultOffset, timezoneId, stdDisplayName);

            }

        }

        /// <summary>
        ///   Extracts a transition time form a VTIMEZONE definition.</summary>
        /// <param name="source">
        ///   A VTIMEZONE (STANDARD or DAYLIGHT) definition</param>
        /// <returns>
        ///   The transition time - or - <c>null</c>.</returns>
        private TimeZoneInfo.TransitionTime? GetTransitionTime(IDictionary<String, ContentLine> source) {

            // stop processing if rule definition isn't complete
            if (source == null || !source.ContainsKey("DTSTART") || !source.ContainsKey("RRULE"))
                return null;

            // split rules
            Dictionary<String, String> rules = source["RRULE"].Value.ToDictionary(';', '=');

            // stop processing if rule definition isn't complete
            if (!rules.ContainsKey("BYMONTH") || !rules.ContainsKey("BYDAY"))
                return null;

            // extract date and time
            DateTime timeOfDay = source["DTSTART"].Value.ToTime();

            // extract month
            Int32 month = Int32.Parse(rules["BYMONTH"]);

            // extract week and day
            String dayAndWeek = rules["BYDAY"];
            DayOfWeek dayOfWeek = dayAndWeek.Substring(dayAndWeek.Length - 2).ToDayOfWeek(DayOfWeek.Sunday);
            Int32 weekPattern = Int32.Parse(dayAndWeek.Substring(0, dayAndWeek.Length - 2));
            if (weekPattern < 0)
                weekPattern = 5;

            return TimeZoneInfo.TransitionTime.CreateFloatingDateRule(timeOfDay, month, weekPattern, dayOfWeek);

        }

        /// <summary>
        ///   Extracts a transition rule start date form a VTIMEZONE definition.</summary>
        /// <param name="source">
        ///   A VTIMEZONE (STANDARD or DAYLIGHT) definition</param>
        /// <returns>
        ///   The transition rule start date - or - <c>null</c>.</returns>
        private DateTime? GetTransitionDate(IDictionary<String, ContentLine> source) {

            // stop processing if start date definition isn't complete
            if (source == null || !source.ContainsKey("DTSTART"))
                return null;

            return source["DTSTART"].Value.ToDateTime().Date;

        }

        /// <summary>
        ///   Calculates the delta between "to" and "from" offsets.</summary>
        /// <param name="source">
        ///   The source.</param>
        /// <returns>
        ///   The delta offset.</returns>
        private TimeSpan? GetTransitionDelta(IDictionary<String, ContentLine> source) {

            // stop processing if offset definition isn't complete
            if (source == null || !source.ContainsKey("TZOFFSETFROM") || !source.ContainsKey("TZOFFSETTO"))
                return null;

            // extract values
            TimeSpan offsetFrom = source["TZOFFSETFROM"].Value.ToTimeSpan();
            TimeSpan offsetTo = source["TZOFFSETTO"].Value.ToTimeSpan();

            // return calculation
            return offsetTo - offsetFrom;

        }

    }

}
