using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

using LogicModule.Nodes.Helpers;
using LogicModule.ObjectModel;
using LogicModule.ObjectModel.TypeSystem;

namespace neleo_com.Logic.Aggregation {

    /// <summary>
    ///   The available statistical operations for evaluation.</summary>
    public static class StatisticOperation {

        public const String Average = nameof(StatisticOperation.Average);
        public const String Median = nameof(StatisticOperation.Median);
        public const String Mode = nameof(StatisticOperation.Mode);

        /// <summary>
        ///   Performs the statistical calculation operations that are available.</summary>
        /// <param name="operation">
        ///   An operation selector.</param>
        /// <param name="values">
        ///   The list of values.</param>
        /// <returns>
        ///   The result of the <paramref name="operation"/> over the <paramref name="values"/>.</returns>
        public static Double Calc(String operation, IEnumerable<Double> values) {

            if (values == null || values.Count() == 0 || String.IsNullOrWhiteSpace(operation))
                throw new ArgumentNullException();

            switch (operation) {

                case Mode:
                    // round all values to avoid getting no relevant results
                    // then, group all values and sort them by the amount of samples
                    // then, take the value that has been stored the most
                    return values.Select(val => (Int32)Math.Round(val, 0)).GroupBy(val => val).OrderBy(val => val.Count()).Last().Key;

                case Median:
                    // sort all values
                    // then, take the middle value
                    return values.OrderBy(val => val).ElementAt((Int32)(values.Count() / 2));

                default:
                    return values.Average();

            }

        }

    }

    /// <summary>
    ///   An aggregator that stores a value sample for every hour for a period of days and returns an aggregated value based on the selected <see cref="StatisticOperation"/> method.</summary>
    public class Aggregator : LogicNodeBase {

        /// <summary>
        ///   The Type Service manages incoming and outgoing ports.</summary>
        private readonly ITypeService TypeService;

        /// <summary>
        ///   The Scheduler Service provides access to the (real | virtual) date and time.</summary>
        private readonly ISchedulerService SchedulerService;

        /// <summary>
        ///   The Persistance Service stores the sampled data for evaluation.</summary>
        private readonly IPersistenceService PersistanceService;

        /// <summary>
        ///   Key to store and retrieve samples in the <see cref="PersistanceService"/>.</summary>
        private const String StoreKey = "Store";

        /// <summary>
        ///   Tracks the hour (0..23) when the last sample has been captured to reduce workload.</summary>
        private Int32 LastSample = -1;

        /// <summary>
        ///   The value input port (either a parameter or value).</summary>
        [Input(DisplayOrder = 1, IsRequired = true)]
        public DoubleValueObject Input {
            get; private set;
        }

        /// <summary>
        ///   defines the evaluation method / operator.</summary>
        [Parameter(DisplayOrder = 2, IsDefaultShown = true)]
        public EnumValueObject EvaluationMethod {
            get; private set;
        }

        /// <summary>
        ///   Defines the rolling evaluation period in days (1..x).</summary>
        [Parameter(DisplayOrder = 3, IsDefaultShown = true)]
        public IntValueObject EvaluationPeriod {
            get; private set;
        }

        /// <summary>
        ///   Returns the maximum value of the rolling evaluation period.</summary>
        [Output(DisplayOrder = 1, IsDefaultShown = true)]
        public DoubleValueObject OverallMaxValue {
            get; private set;
        }

        /// <summary>
        ///   Returns the evaluated value (<see cref="EvaluationMethod"/>) of the rolling evaluation period.</summary>
        [Output(DisplayOrder = 2, IsDefaultShown = true)]
        public DoubleValueObject OverallEvalValue {
            get; private set;
        }

        /// <summary>
        ///   Returns the evaluated value (<see cref="EvaluationMethod"/>) for each day of the rolling evaluation period.</summary>
        [Output(DisplayOrder = 2, IsDefaultShown = false)]
        public IList<DoubleValueObject> DailyEvalValue {
            get; private set;
        }

        /// <summary>
        ///   Returns the minimum value of the rolling evaluation period.</summary>
        [Output(DisplayOrder = 4, IsDefaultShown = true)]
        public DoubleValueObject OverallMinValue {
            get; private set;
        }

        /// <summary>
        ///   Constructor to setup the ports and services.</summary>
        /// <param name="context">
        ///   Context of the node instance to connect to services.</param>
        public Aggregator(INodeContext context) : base(context) {

            context.ThrowIfNull(nameof(context));

            this.TypeService = context.GetService<ITypeService>();
            this.SchedulerService = context.GetService<ISchedulerService>();
            this.PersistanceService = context.GetService<IPersistenceService>();

            this.Input = this.TypeService.CreateDouble(PortTypes.Number, nameof(this.Input));

            this.EvaluationPeriod = this.TypeService.CreateInt(PortTypes.Integer, nameof(this.EvaluationPeriod), 5);
            this.EvaluationPeriod.MinValue = ValueStore.MinDays;
            this.EvaluationPeriod.MaxValue = ValueStore.MaxDays;

            this.EvaluationMethod = this.TypeService.CreateEnum(nameof(StatisticOperation), nameof(this.EvaluationMethod),
                new String[] { StatisticOperation.Average, StatisticOperation.Median, StatisticOperation.Mode }, StatisticOperation.Average);

            this.OverallMaxValue = this.TypeService.CreateDouble(PortTypes.Number, nameof(this.OverallMaxValue));
            this.OverallEvalValue = this.TypeService.CreateDouble(PortTypes.Number, nameof(this.OverallEvalValue));
            this.OverallMinValue = this.TypeService.CreateDouble(PortTypes.Number, nameof(this.OverallMinValue));

            this.DailyEvalValue = new List<DoubleValueObject>();
            ListHelpers.ConnectListToCounter(this.DailyEvalValue, this.EvaluationPeriod,
                this.TypeService.GetValueObjectCreator(PortTypes.Number, nameof(this.DailyEvalValue)), null);

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
            if (key.StartsWith(nameof(this.DailyEvalValue))) {

                String identifier = key.Substring(nameof(this.DailyEvalValue).Length);
                String newKey = String.Format("Daily{0}Value", this.EvaluationMethod.Value);
                return String.Format(ResourceManager.GetString(newKey, culture) ?? key, identifier);

            }
            else if (key.Equals(nameof(this.OverallEvalValue))) {

                String newKey = String.Format("Overall{0}Value", this.EvaluationMethod.Value);
                return ResourceManager.GetString(newKey, culture) ?? newKey;

            }
            else {

                return ResourceManager.GetString(key, culture) ?? key;

            }

        }

        /// <summary>
        ///   Sets the output port values if there're stored values.</summary>
        public override void Startup() {

            // read the captured samples (or have an empty store)
            ValueStore store = new ValueStore(this.EvaluationPeriod.Value,
                this.PersistanceService.GetValue(this, Aggregator.StoreKey));

            // update the calculated values
            IEnumerable<Double> allValues = store.GetValues();
            if (allValues != null && allValues.Count() > 0) {

                this.OverallMinValue.Value = allValues.Min();
                this.OverallMaxValue.Value = allValues.Max();
                this.OverallEvalValue.Value = StatisticOperation.Calc(this.EvaluationMethod.Value, allValues);

            }

            for (Int32 day = 0; day < this.EvaluationPeriod.Value; day++) {

                IEnumerable<Double> dayValues = store.GetValues(day + 1);
                if (dayValues != null && dayValues.Count() > 0)
                    this.DailyEvalValue[day].Value = StatisticOperation.Calc(this.EvaluationMethod.Value, dayValues);

            }

        }

        /// <summary>
        ///   Captures the incoming data and calculates the output on demand.</summary>
        public override void Execute() {

            // sample only the first incoming value of the hour, update the calculated values
            // and ignore everything else
            DateTime timestamp = this.SchedulerService.Now;
            if (!timestamp.Hour.Equals(this.LastSample)) {

                // read the captured samples (or have an empty store)
                ValueStore store = new ValueStore(this.EvaluationPeriod.Value,
                    this.PersistanceService.GetValue(this, Aggregator.StoreKey));

                // save the value and the historical data
                store.SetValue(timestamp, this.Input.Value);
                this.PersistanceService.SetValue(this, Aggregator.StoreKey, store.Serialize());

                // remember that we saved a value for this hour of the day
                this.LastSample = timestamp.Hour;

                // update the calculated values
                IEnumerable<Double> allValues = store.GetValues();
                if (allValues != null && allValues.Count() > 0) {

                    this.OverallMinValue.Value = allValues.Min();
                    this.OverallMaxValue.Value = allValues.Max();
                    this.OverallEvalValue.Value = StatisticOperation.Calc(this.EvaluationMethod.Value, allValues);

                }

                for (Int32 day = 0; day < this.EvaluationPeriod.Value; day++) {

                    IEnumerable<Double> dayValues = store.GetValues(day + 1);
                    if (dayValues != null && dayValues.Count() > 0)
                        this.DailyEvalValue[day].Value = StatisticOperation.Calc(this.EvaluationMethod.Value, dayValues);

                }

            }

        }

    }

}
