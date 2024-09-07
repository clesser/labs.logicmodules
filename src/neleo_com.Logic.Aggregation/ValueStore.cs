using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace neleo_com.Logic.Aggregation {

    /// <summary>
    ///   A value store to capture samples per day and hour.</summary>
    public class ValueStore {

        /// <summary>
        ///   The maximum number of days to capture historical data.</summary>
        public const Int32 MinDays = 1;

        /// <summary>
        ///   The maximum number of days to capture historical data.</summary>
        public const Int32 MaxDays = 14;

        /// <summary>
        ///   The value splitter used to serialize and deserialize the data store.</summary>
        private const Char Splitter = '|';

        /// <summary>
        ///   The internal value store.</summary>
        private readonly List<Double?> Store;

        /// <summary>
        ///   The number of days with historic values in the store.</summary>
        private readonly Int32 History;

        /// <summary>
        ///   Initializes the value store by providing 24+1 storage cells for <paramref name="days"/>.</summary>
        /// <param name="days">
        ///   The number of days with historical data.</param>
        /// <param name="serializedValues">
        ///   Serialized data to initialize the store.</param>
        public ValueStore(Int32 days, String serializedValues) {

            // verify min/max size of the store
            if (ValueStore.MinDays > days || ValueStore.MaxDays < days)
                throw new ArgumentOutOfRangeException(nameof(days));

            // remember the amount of historic days
            this.History = days;

            // calculate the number of storage cells
            // Note: the first cell stores the number of the current day (or -1)
            Int32 storeSize = 1 + ((1 + this.History) * 24);

            // initialize an empty store or deserialize the passed data
            if (String.IsNullOrWhiteSpace(serializedValues)) {

                this.Store = new List<Double?>(new Double?[storeSize]);

            }
            else {

                this.Store = new List<Double?>(serializedValues.Split(ValueStore.Splitter).Select(val =>
                    Double.TryParse(val, NumberStyles.Float, CultureInfo.InvariantCulture, out Double dVal) ? (Double?)dVal : null));

                if (this.Store.Count < storeSize)
                    this.Store.AddRange(new Double?[storeSize - this.Store.Count]);

            }

            // ensure that the day reference has been set (otherwise initialize it)
            if (!this.Store[0].HasValue)
                this.Store[0] = -1;

        }

        /// <summary>
        ///   Initializes the value store by providing 24 storage cells for <paramref name="days"/> (+1 for today)</summary>
        /// <param name="days">
        ///   The number of days with historical data.</param>
        public ValueStore(Int32 days) : this(days, null) { }

        /// <summary>
        ///   Sets a value for a given time (day and hour).</summary>
        /// <param name="time">
        ///   The time that is associated with the value.</param>
        /// <param name="value">
        ///   The value to be stored.</param>
        public void SetValue(DateTime time, Double value) {

            // ensure that values are in range
            if (time == null)
                throw new ArgumentNullException(nameof(time));

            // check if there's a new day
            if (!this.Store[0].Value.Equals((Double)time.Day)) {

                // insert new storage cells for the day (so that "today" will become "yesterday")
                // (note that trimming of the obsolete cells will be done during serialization (ToString())
                this.Store.InsertRange(1, new Double?[24]);

                // remember the number of the new day
                this.Store[0] = (Double)time.Day;

            }

            // set the value
            // (note that the first position is the number of the current day)
            this.Store[time.Hour + 1] = value;

        }

        /// <summary>
        ///   Gets the stored values for a given day (0 to 24 values).</summary>
        /// <param name="day">
        ///   A day (0 = today).</param>
        /// <returns>
        ///   The stored values (can be less then 24 per day).</returns>
        public IEnumerable<Double> GetValues(Int32 day) {

            // ensure the day is in range
            if (day < 0 || day > (this.History))
                throw new ArgumentOutOfRangeException(nameof(day));

            // return set values
            // (note to skip the first value as is indicates the number of the day)
            return this.Store.Skip(1).Skip(day * 24).Take(24).Where(val => val.HasValue).Select(val => val.Value);

        }

        /// <summary>
        ///   Gets all available historical data (it includes everything except today).</summary>
        /// <returns>
        ///   The stored values.</returns>
        public IEnumerable<Double> GetValues() {

            // return set values
            // (note to skip the first value as is indicates the number of the day)
            return this.Store.Skip(1).Skip(24).Take(this.History * 24).Where(val => val.HasValue).Select(val => val.Value);

        }

        /// <summary>
        ///   Serializes the stored values.</summary>
        /// <returns>
        ///   The serialized values.</returns>
        public String Serialize() {

            String valueSplitter = new String(ValueStore.Splitter, 1);
            Int32 storeSize = 1 + ((1 + this.History) * 24);

            return String.Join(valueSplitter,
                this.Store.Take(storeSize).Select(v => v.HasValue ? v.Value.ToString(CultureInfo.InvariantCulture) : "-"));

        }

    }

}
