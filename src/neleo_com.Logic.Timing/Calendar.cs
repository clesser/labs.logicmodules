﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;

using LogicModule.Nodes.Helpers;
using LogicModule.ObjectModel;
using LogicModule.ObjectModel.TypeSystem;

using neleo_com.Logic.Timing.Parser;

namespace neleo_com.Logic.Timing {

    /// <summary>
    ///   Simple Event Data class.</summary>
    internal class Event {

        /// <summary>
        ///   The local start date and time of the event;</summary>
        public DateTime LocalStartDateTime {
            get; set;
        }

        /// <summary>
        ///   The duration of the event.</summary>
        public TimeSpan Duration {
            get; set;
        }

        /// <summary>
        ///   The Subject of the event.</summary>
        public String Subject {
            get; set;
        }

        /// <summary>
        ///   The Location of the event.</summary>
        public String Location {
            get; set;
        }

    }

    /// <summary>
    ///   A calendar that handles the upcoming events for the next seven days.</summary>
    public class Calendar : LogicNodeBase {

        /// <summary>
        ///   The Type Service manages incoming and outgoing ports.</summary>
        private readonly ITypeService TypeService;

        /// <summary>
        ///   The Scheduler Service manages access to the clock and scheduled callbacks.</summary>
        private readonly ISchedulerService SchedulerService;

        /// <summary>
        ///   Default format string for the upcoming event summary.</summary>
        private const String DefaultSummaryFormatString = "[{0:t} {1:hh\\:mm}]: {2} ({3})";

        /// <summary>
        ///   Receives a iCal/vCal calendar.</summary>
        [Input(DisplayOrder = 1, IsInput = true, IsRequired = true)]
        public StringValueObject CalendarInput {
            get; private set;
        }

        /// <summary>
        ///   Number of parameters an event must meet to be handled.</summary>
        [Parameter(DisplayOrder = 2, IsDefaultShown = false)]
        public IntValueObject CalendarIncludeParamsCount {
            get; private set;
        }

        /// <summary>
        ///   Specification of parameters an event must meet to be handled.</summary>
        [Parameter(DisplayOrder = 3, IsDefaultShown = false)]
        public IList<StringValueObject> CalendarIncludeParameters {
            get; private set;
        }

        /// <summary>
        ///   Number of paramters an event must not meet to be handled.</summary>
        [Parameter(DisplayOrder = 4, IsDefaultShown = false)]
        public IntValueObject CalendarExcludeParamsCount {
            get; private set;
        }

        /// <summary>
        ///   Specification of paramters an event must not meet to be handled.</summary>
        [Parameter(DisplayOrder = 5, IsDefaultShown = false)]
        public IList<StringValueObject> CalendarExcludeParameters {
            get; private set;
        }

        /// <summary>
        ///   Time span to publish the next event to the output ports after the current event has begun.</summary>
        [Parameter(DisplayOrder = 6, IsDefaultShown = true)]
        public TimeSpanValueObject NextEventAfter {
            get; private set;
        }

        /// <summary>
        ///   Format string for <see cref="NextEventSummary"/> (0=DateTime, 1=Duration, 2=Subject, 3=Location).</summary>
        [Parameter(DisplayOrder = 7, IsDefaultShown = false)]
        public StringValueObject NextEventSummaryFormat {
            get; private set;
        }

        /// <summary>
        ///   Text for <see cref="NextEventSummary"/> if the event queue is empty.</summary>
        [Parameter(DisplayOrder = 8, IsDefaultShown = false)]
        public StringValueObject NextEventSummaryEmpty {
            get; private set;
        }

        /// <summary>
        ///   Start time of the next event.</summary>
        [Output(DisplayOrder = 2, IsDefaultShown = true)]
        public DateTimeValueObject NextEventBegin {
            get; private set;
        }

        /// <summary>
        ///   Subject of the next event.</summary>
        [Output(DisplayOrder = 3, IsDefaultShown = true)]
        public StringValueObject NextEventSummary {
            get; private set;
        }

        /// <summary>
        ///   A collection of the upcoming events.</summary>
        private IEnumerable<Event> Events;

        /// <summary>
        ///   A token for the scheduling service.</summary>
        private SchedulerToken NextEventToken;

        /// <summary>
        ///   Constructor to setup the ports and services.</summary>
        /// <param name="context">
        ///   Context of the node instance to connect to services.</param>
        public Calendar(INodeContext context) {

            // ensure context is set
            context.ThrowIfNull(nameof(context));

            // initializes services
            this.TypeService = context.GetService<ITypeService>();
            this.SchedulerService = context.GetService<ISchedulerService>();

            // initialize ports
            this.CalendarInput = this.TypeService.CreateString(PortTypes.String, nameof(this.CalendarInput));

            this.CalendarIncludeParamsCount = this.TypeService.CreateInt(PortTypes.Integer, nameof(this.CalendarIncludeParamsCount), 0);
            this.CalendarIncludeParamsCount.MinValue = 0;
            this.CalendarIncludeParamsCount.MaxValue = 10;

            this.CalendarIncludeParameters = new List<StringValueObject>();
            ListHelpers.ConnectListToCounter(this.CalendarIncludeParameters, this.CalendarIncludeParamsCount,
                this.TypeService.GetValueObjectCreator(PortTypes.String, nameof(this.CalendarIncludeParameters)), null);

            this.CalendarExcludeParamsCount = this.TypeService.CreateInt(PortTypes.Integer, nameof(this.CalendarExcludeParamsCount), 0);
            this.CalendarExcludeParamsCount.MinValue = 0;
            this.CalendarExcludeParamsCount.MaxValue = 10;

            this.CalendarExcludeParameters = new List<StringValueObject>();
            ListHelpers.ConnectListToCounter(this.CalendarExcludeParameters, this.CalendarExcludeParamsCount,
                this.TypeService.GetValueObjectCreator(PortTypes.String, nameof(this.CalendarExcludeParameters)), null);

            this.NextEventAfter = this.TypeService.CreateTimeSpan(PortTypes.TimeSpan, nameof(this.NextEventAfter), TimeSpan.FromMinutes(5));
            this.NextEventAfter.MinValue = TimeSpan.Zero;
            this.NextEventAfter.MaxValue = TimeSpan.FromMinutes(15);

            this.NextEventSummaryFormat = this.TypeService.CreateString(PortTypes.String, nameof(this.NextEventSummaryFormat), Calendar.DefaultSummaryFormatString);
            this.NextEventSummaryEmpty = this.TypeService.CreateString(PortTypes.String, nameof(this.NextEventSummaryEmpty), String.Empty);

            this.NextEventBegin = this.TypeService.CreateDateTime(PortTypes.DateTime, nameof(this.NextEventBegin));
            this.NextEventSummary = this.TypeService.CreateString(PortTypes.String, nameof(this.NextEventSummary));

        }

        /// <summary>
        ///   Localizes all options and labels.</summary>
        /// <param name="language">
        ///   The language / culture.</param>
        /// <param name="key">
        ///   The key for the option or label.</param>
        /// <returns>
        ///   A localized key or label.</returns>
        public override String Localize(String language, String key) {

            // ensure that parameters are set
            language.ThrowIfNull(nameof(language));
            key.ThrowIfNull(nameof(key));

            // load culture
            CultureInfo culture;
            try {
                culture = CultureInfo.GetCultureInfo(language);
            }
            catch (CultureNotFoundException) {
                culture = CultureInfo.InvariantCulture;
            }

            // filter and map keys
            if (key.StartsWith(nameof(this.CalendarIncludeParameters))) {

                String identifier = key.Substring(nameof(this.CalendarIncludeParameters).Length);
                return String.Format(ResourceManager.GetString(nameof(this.CalendarIncludeParameters), culture) ?? key, identifier);

            }
            else if (key.StartsWith(nameof(this.CalendarExcludeParameters))) {

                String identifier = key.Substring(nameof(this.CalendarExcludeParameters).Length);
                return String.Format(ResourceManager.GetString(nameof(this.CalendarExcludeParameters), culture) ?? key, identifier);

            }
            else {

                return ResourceManager.GetString(key, culture) ?? key;

            }

        }

        /// <summary>
        ///   Caluculates the output based on all available input variables.</summary>
        public override void Execute() {

            // unschedule upcoming updates
            if (this.NextEventToken != null)
                this.SchedulerService.Remove(this.NextEventToken);

            // collect filters
            IEnumerable<String> includeFilters = this.CalendarIncludeParameters
                .Select(p => p.HasValue ? p.Value : String.Empty)
                .Where(v => !String.IsNullOrWhiteSpace(v));

            IEnumerable<String> excludeFilters = this.CalendarExcludeParameters
                .Select(p => p.HasValue ? p.Value : String.Empty)
                .Where(v => !String.IsNullOrWhiteSpace(v));

            // parse calendar
            DateTime localNow = this.SchedulerService.Now;
            if (this.CalendarInput.HasValue)
                this.Events = this.Parse(this.CalendarInput.Value,
                    includeFilters, excludeFilters, localNow, localNow.Date.AddDays(8));

            // schedule next update
            this.Next();

        }

        /// <summary>
        ///   Parses an iCal/vCal and extracts all events between <paramref name="startDateTime"/>
        ///   and <paramref name="endDateTime"/> that meet the other filter criteria, too. Result 
        ///   does not include all-day events.</summary>
        /// <param name="source">
        ///   The calendar as iCal/vCal.</param>
        /// <param name="includeFilters">
        ///   An event must match each filter to be in the result set.</param>
        /// <param name="excludeFilters">
        ///   An event must not match any of these filters to be in the result set.</param>
        /// <param name="startDateTime">
        ///   Start date/time event filter.</param>
        /// <param name="endDateTime">
        ///   End date/time event filter.</param>
        /// <returns>
        ///   A list ov events for the given day - or - an empty list.</returns>
        private IEnumerable<Event> Parse(String source, IEnumerable<String> includeFilters, IEnumerable<String> excludeFilters, DateTime startDateTime, DateTime endDateTime) {

            // setup event list
            ICollection<Event> events = new Collection<Event>();

            // stop further processing if the calendar data is empty
            if (String.IsNullOrWhiteSpace(source))
                return events;

            // ensure that filters are set
            if (includeFilters == null)
                includeFilters = new Collection<String>();

            if (excludeFilters == null)
                excludeFilters = new Collection<String>();

            // parse calendar data
            CalendarParser parser = new CalendarParser(source);

            // load time zones and add #UTC and #LOCAL fallbacks
            IDictionary<String, TimeZoneInfo> timezones = parser.Timezones
                .Select(t => t.AsTimeZoneInfo)
                .Where(t => t != null && !String.IsNullOrWhiteSpace(t.Id))
                .ToDictionary(t => t.Id);

            timezones.Add("#UTC", TimeZoneInfo.Utc);
            timezones.Add("#LOCAL", TimeZoneInfo.Local);

            // load and filter events
            TimeSpan maxDuration = TimeSpan.FromDays(1);
            events = parser.Events
                .Where(e => this.MatchAll(e.Values, includeFilters)
                    && this.MatchNone(e.Values, excludeFilters))
                .Select(e => new Event() {
                    Subject = e.Subject,
                    Location = e.Location,
                    LocalStartDateTime = this.CalcLocalTime(e.StartDateTime, e.StartDateTimeTzId, timezones),
                    Duration = this.CalcDuration(e.StartDateTime, e.StartDateTimeTzId, e.EndDateTime, e.EndDateTimeTzId, timezones)
                })
                .Where(e => e.LocalStartDateTime > startDateTime
                    && e.LocalStartDateTime < endDateTime
                    && e.Duration < maxDuration)
                .ToList();

            return events.OrderBy(e => e.LocalStartDateTime);

        }

        /// <summary>
        ///   Tests whether all <paramref name="filters"/> match (<see cref="String.StartsWith(String)"/>)
        ///   <paramref name="lines"/>.</summary>
        /// <param name="lines">
        ///   The content lines.</param>
        /// <param name="filters"></param>
        /// <returns>
        ///   The filter lines.</returns>
        private Boolean MatchAll(ICollection<ContentLine> lines, IEnumerable<String> filters) {

            if (lines == null)
                throw new ArgumentNullException(nameof(lines));

            if (filters == null || filters.Count() == 0)
                return true;
            else
                return filters.All(f => lines.Any(c => c.Source.Trim().StartsWith(f, StringComparison.OrdinalIgnoreCase)));

        }

        /// <summary>
        ///   Tests whether none of the <paramref name="filters"/> match (<see cref="String.StartsWith(String)"/>)
        ///   <paramref name="lines"/>.</summary>
        /// <param name="lines">
        ///   The content lines.</param>
        /// <param name="filters"></param>
        /// <returns>
        ///   The filter lines.</returns>
        private Boolean MatchNone(ICollection<ContentLine> lines, IEnumerable<String> filters) {

            if (lines == null)
                throw new ArgumentNullException(nameof(lines));

            if (filters == null || filters.Count() == 0)
                return true;
            else
                return !filters.Any(f => lines.Any(c => c.Source.Trim().StartsWith(f, StringComparison.OrdinalIgnoreCase)));

        }

        /// <summary>
        ///   Calculate the local date/time based on <paramref name="dateTime"/> and <paramref name="tzId"/>.</summary>
        /// <param name="dateTime">
        ///   The date/time for a given <paramref name="tzId"/>.</param>
        /// <param name="tzId">
        ///   The time zone for <paramref name="dateTime"/>.</param>
        /// <param name="timezones">
        ///   Timezone definitions.</param>
        /// <returns>
        ///   The local date/time.</returns>
        private DateTime CalcLocalTime(DateTime dateTime, String tzId, IDictionary<String, TimeZoneInfo> timezones) {

            // stop futher processing if timezone(s) is/are not specified
            if (timezones == null || String.IsNullOrWhiteSpace(tzId) || !timezones.ContainsKey(tzId))
                return dateTime;

            // convert given date/time to UTC
            TimeZoneInfo timeZoneInfo = timezones[tzId];
            DateTime utcDateTime = TimeZoneInfo.ConvertTimeToUtc(dateTime, timeZoneInfo);

            // return local time
            return utcDateTime.ToLocalTime();

        }

        /// <summary>
        ///   Calculates the duration between two date/times.</summary>
        /// <param name="start">
        ///   Start date/time.</param>
        /// <param name="startTzId">
        ///   Start date/time timezone.</param>
        /// <param name="end">
        ///   End date/time.</param>
        /// <param name="endTzId">
        ///   End date/time timezones.</param>
        /// <param name="timezones">
        ///   Timezone definitions.</param>
        /// <returns>
        ///   The time span between start and end date.</returns>
        private TimeSpan CalcDuration(DateTime start, String startTzId, DateTime end, String endTzId, IDictionary<String, TimeZoneInfo> timezones) {

            return this.CalcLocalTime(end, endTzId, timezones).Subtract(this.CalcLocalTime(start, startTzId, timezones));

        }

        /// <summary>
        ///   Configures the scheduler to update the "Next" outputs.</summary>
        private void Next() {

            // stop processing if there aren't any events
            if (this.Events == null || this.Events.Count() == 0) {

                this.NextEventSummary.Value =
                    this.NextEventSummaryEmpty.HasValue ? this.NextEventSummaryEmpty.Value : String.Empty;

                this.NextEventBegin.Value = DateTime.MinValue;

                return;

            }

            // find the next event
            DateTime localNow = this.SchedulerService.Now;
            Event nextEvent = this.Events.Where(e => e.LocalStartDateTime > localNow).OrderBy(e => e.LocalStartDateTime).FirstOrDefault();

            // stop processing if there's no upcoming events
            if (nextEvent == null) {

                this.NextEventSummary.Value =
                    this.NextEventSummaryEmpty.HasValue ? this.NextEventSummaryEmpty.Value : String.Empty;

                this.NextEventBegin.Value = DateTime.MinValue;

            }

            // update the output ports
            else {

                String formatString = this.NextEventSummaryFormat.HasValue && !String.IsNullOrWhiteSpace(this.NextEventSummaryFormat.Value) ? this.NextEventSummaryFormat.Value : Calendar.DefaultSummaryFormatString;

                String nextEventSummary = String.Format(formatString,
                    nextEvent.LocalStartDateTime, nextEvent.Duration, nextEvent.Subject, nextEvent.Location);

                if (!this.NextEventSummary.HasValue || this.NextEventSummary.Value != nextEventSummary)
                    this.NextEventSummary.Value = nextEventSummary;

                DateTime nextEventStartDateTime = nextEvent.LocalStartDateTime;

                if (!this.NextEventBegin.HasValue || this.NextEventBegin.Value != nextEventStartDateTime)
                    this.NextEventBegin.Value = nextEvent.LocalStartDateTime;

                // calculate and schedule next update time
                TimeSpan updateWaitTime = this.NextEventAfter.HasValue ? this.NextEventAfter.Value : TimeSpan.Zero;
                DateTime utcUpdateTime = nextEvent.LocalStartDateTime.Add(updateWaitTime).ToUniversalTime();

                this.NextEventToken = this.SchedulerService.InvokeAt(utcUpdateTime, this.Next);

            }

        }

    }

}
