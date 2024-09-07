using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace neleo_com.Logic.Bridges.Velux {

    // request://node/name?param1=value1
    // request://node:1?param1=value1
    // request://node:1#stop

    // --> URI scheme map: ":1" (=Port) to identifier and "/name" (=AbsolutePath) to name

    /// <summary>
    ///   Telegrams are used to communicate between logic nodes and represented as URIs.</summary>
    public class Klf200Telegram {

        /// <summary>
        ///   Defines the telegram mode (request / response).</summary>
        public Klf200TelegramMode Mode {
            get; set;
        } = Klf200TelegramMode.None;

        /// <summary>
        ///   Defines the scope of a telegram (node / group / scene).</summary>
        public Klf200TelegramScope Scope {
            get; set;
        } = Klf200TelegramScope.None;

        /// <summary>
        ///   Identifier of a node / group / scene.</summary>
        public Byte Identifier {
            get; set;
        } = byte.MaxValue;

        /// <summary>
        ///   Name of a node / group / scene.</summary>
        public String Name {
            get; set;
        } = String.Empty;

        /// <summary>
        ///   Parameters for the node / group / scene.</summary>
        public IDictionary<Klf200TelegramParameter, String> Parameters {
            get;
        } = new Dictionary<Klf200TelegramParameter, String>();

        /// <summary>
        ///   Action for the node / group / scene.</summary>
        public String Action {
            get; set;
        } = String.Empty;

        /// <summary>
        ///   Adds or overwrites a parameter. Key-value-pairs with empty values 
        ///   won't be added or removed.</summary>
        /// <param name="key">
        ///   The key.</param>
        /// <param name="value">
        ///   The value.</param>
        public void SetParameter(Klf200TelegramParameter key, String value) {

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
        public void SetParameter(Klf200TelegramParameter key, Object value) {

            this.SetParameter(key, (value == null) ? String.Empty : value.ToString());

        }

        /// <summary>
        ///   Parses an URI-style telegram and extracts all properties.</summary>
        /// <param name="value">
        ///   An encoded telegram.</param>
        /// <returns>
        ///   A decoded telegram.</returns>
        public static Klf200Telegram Parse(String value) {

            if (String.IsNullOrWhiteSpace(value))
                return null;

            Klf200Telegram telegram = new Klf200Telegram();
            Uri parsedValue = new Uri(value);

            if (Enum.TryParse<Klf200TelegramMode>(parsedValue.Scheme, true, out Klf200TelegramMode mode))
                telegram.Mode = mode;

            if (Enum.TryParse<Klf200TelegramScope>(parsedValue.Host, true, out Klf200TelegramScope scope))
                telegram.Scope = scope;

            Int32 identifier = parsedValue.Port;
            if (identifier >= Byte.MinValue && identifier <= Byte.MaxValue)
                telegram.Identifier = (Byte)identifier;

            String name = parsedValue.LocalPath;
            if (name.Length > 1)
                telegram.Name = name.Substring(1);

            // System.Uri does eliminate port = 0 and return port = -1 instead --> fix this manually
            if (value.StartsWith(String.Format("{0}://{1}:0", telegram.Mode, telegram.Scope), StringComparison.OrdinalIgnoreCase))
                telegram.Identifier = 0;

            String parameters = parsedValue.Query;
            if (parameters.Length > 1)
                foreach (String parameter in parameters.Substring(1).Split(new char[] { '&' })) {
                    String[] keyValuePair = parameter.Split(new Char[] { '=' });
                    if (keyValuePair.Length == 2)
                        if (Enum.TryParse<Klf200TelegramParameter>(keyValuePair[0], true, out Klf200TelegramParameter parameterKey))
                            telegram.Parameters.Add(parameterKey, keyValuePair[1]);
                }

            String action = parsedValue.Fragment;
            if (action.Length > 1)
                telegram.Action = action.Substring(1);

            return telegram;

        }

        /// <summary>
        ///   Encodes all properties into an URI-style telegram.</summary>
        /// <returns>
        ///   An encoded telegram.</returns>
        public override String ToString() {

            StringBuilder telegram = new StringBuilder();
            telegram.AppendFormat("{0}://{1}", this.Mode.ToString().ToLowerInvariant(),
                this.Scope.ToString().ToLowerInvariant());

            if (!String.IsNullOrWhiteSpace(this.Name))
                telegram.AppendFormat("/{0}", this.Name);
            else if (this.Identifier != byte.MaxValue)
                telegram.AppendFormat(":{0}", this.Identifier);

            if (this.Parameters.Count() > 0)
                telegram.AppendFormat("?{0}", String.Join("&",
                    this.Parameters.Select(p => String.Join("=", p.Key.ToString().ToCamelCase(), p.Value))));

            if (!String.IsNullOrWhiteSpace(this.Action))
                telegram.AppendFormat("#{0}", this.Action);

            return telegram.ToString();

        }

    }

}
