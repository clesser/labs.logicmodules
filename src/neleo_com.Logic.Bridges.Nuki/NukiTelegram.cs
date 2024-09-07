using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using neleo_com.Logic.Bridges.Nuki.Definitions;

namespace neleo_com.Logic.Bridges.Nuki {

    /// <summary>
    ///   Telegrams are used to communicate between logic nodes and represented as URIs.</summary>
    public class NukiTelegram {

        /// <summary>
        ///   Defines the telegram mode (request / response).</summary>
        public NukiTelegramMode Mode {
            get; set;
        } = NukiTelegramMode.None;

        /// <summary>
        ///   Defines the device type.</summary>
        public Int32 DeviceType {
            get; set;
        } = 0;

        /// <summary>
        ///   Defines the device identifier.</summary>
        public String DeviceId {
            get; set;
        } = String.Empty;

        /// <summary>
        ///   Defines the action of a request or the corresponding response.</summary>
        public NukiActionType Action {
            get; set;
        } = NukiActionType.None;

        /// <summary>
        ///   Parameters of the telegram.</summary>
        public IDictionary<NukiTelegramParameter, String> Parameters {
            get;
        } = new Dictionary<NukiTelegramParameter, String>();

        /// <summary>
        ///  Checks if a specific parameter has been set.</summary>
        /// <param name="key">
        ///   The key.</param>
        /// <returns>
        ///   <c>true</c> if the parameter has been set and initialized.</returns>
        public Boolean HasParameter(NukiTelegramParameter key) {

            return this.Parameters.ContainsKey(key) && !String.IsNullOrWhiteSpace(this.Parameters[key]);

        }

        /// <summary>
        ///   Adds or overwrites a parameter. Key-value-pairs with empty values 
        ///   won't be added or removed.</summary>
        /// <param name="key">
        ///   The key.</param>
        /// <param name="value">
        ///   The value.</param>
        public void SetParameter(NukiTelegramParameter key, String value) {

            // remove key-value-pair if value is empty - or just don't add it
            if (String.IsNullOrWhiteSpace(value)) {

                if (this.Parameters.ContainsKey(key))
                    this.Parameters.Remove(key);

            }
            else {

                // add or overwrite the value for a specific key.
                if (this.Parameters.ContainsKey(key))
                    this.Parameters[key] = value;
                else
                    this.Parameters.Add(key, value);

            }

        }

        /// <summary>
        ///   Adds or overwrites a parameter. Key-value-pairs with <c>null</c> values 
        ///   won't be added or removed.</summary>
        /// <param name="key">
        ///   The key.</param>
        /// <param name="value">
        ///   The value.</param>
        public void SetParameter(NukiTelegramParameter key, Object value) {

            this.SetParameter(key, (value == null) ? String.Empty : value.ToString());

        }

        /// <summary>
        ///   Parses an URI-style telegram and extracts all properties.</summary>
        /// <param name="value">
        ///   An encoded telegram.</param>
        /// <returns>
        ///   A decoded telegram.</returns>
        public static NukiTelegram Parse(String value) {

            if (String.IsNullOrWhiteSpace(value))
                return null;

            NukiTelegram telegram = new NukiTelegram();
            Uri parsedValue = new Uri(value);

            if (Enum.TryParse<NukiTelegramMode>(parsedValue.Scheme, true, out NukiTelegramMode mode))
                telegram.Mode = mode;

            telegram.DeviceId = parsedValue.Host;

            telegram.DeviceType = parsedValue.Port;

            // System.Uri does eliminate port = 0 and return port = -1 instead --> fix this manually
            if (value.StartsWith(String.Format("{0}://{1}:0/", telegram.Mode, telegram.DeviceId), StringComparison.OrdinalIgnoreCase))
                telegram.DeviceType = 0;

            if (parsedValue.LocalPath.Length > 1)
                if (Enum.TryParse<NukiActionType>(parsedValue.LocalPath.Substring(1), true, out NukiActionType action))
                    telegram.Action = action;

            String parameters = parsedValue.Query;
            if (parameters.Length > 1)
                foreach (String parameter in parameters.Substring(1).Split(new char[] { '&' })) {
                    String[] keyValuePair = parameter.Split(new Char[] { '=' });
                    if (keyValuePair.Length == 2)
                        if (Enum.TryParse<NukiTelegramParameter>(keyValuePair[0], true, out NukiTelegramParameter parameterKey))
                            telegram.Parameters.Add(parameterKey, keyValuePair[1]);
                }

            return telegram;

        }

        /// <summary>
        ///   Encodes all properties into an URI-style telegram.</summary>
        /// <returns>
        ///   An encoded telegram.</returns>
        public override String ToString() {

            StringBuilder telegram = new StringBuilder();
            telegram.AppendFormat("{0}://{1}:{2}/{3}", this.Mode.ToString().ToLowerInvariant(),
                this.DeviceId, this.DeviceType, this.Action.ToString().ToLowerInvariant());

            if (this.Parameters.Count() > 0)
                telegram.AppendFormat("?{0}", String.Join("&",
                    this.Parameters.Select(p => String.Join("=", p.Key.ToString().ToLowerInvariant(), p.Value))));

            return telegram.ToString();

        }

    }

}
