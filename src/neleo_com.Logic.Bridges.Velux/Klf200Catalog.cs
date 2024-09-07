using System;
using System.Collections.Generic;
using System.Linq;

namespace neleo_com.Logic.Bridges.Velux {

    /// <summary>
    ///   Mapping table to convert node/group/scene names to identifiers and vice versa.</summary>
    public class Klf200Catalog {

        /// <summary>
        ///   Internal mapping table.</summary>
        private readonly IDictionary<String, String> Catalog;

        /// <summary>
        ///   Initializes a new catalog.</summary>
        public Klf200Catalog() {

            this.Catalog = new Dictionary<String, String>();

        }

        /// <summary>
        ///   Registers or updates an entry in the catalog.</summary>
        /// <param name="scope">
        ///   The scope of the <paramref name="name"/> and <paramref name="identifier"/>.</param>
        /// <param name="name">
        ///   The user friednly name.</param>
        /// <param name="identifier">
        ///   The internal identifier.</param>
        public void Collect(Klf200TelegramScope scope, String name, Byte identifier) {

            if (String.IsNullOrWhiteSpace(name))
                name = identifier.ToString();

            this.Set(this.Key(scope, name), identifier);
            this.Set(this.Key(scope, identifier), name);

        }

        /// <summary>
        ///   Generates a key from <paramref name="scope"/> and <paramref name="key"/>.</summary>
        /// <param name="scope">
        ///   The scope.</param>
        /// <param name="key">
        ///   The key.</param>
        /// <returns>
        ///   A key.</returns>
        private String Key(Klf200TelegramScope scope, String key) {

            return String.Format("{0}-{1}", scope, key.Replace(' ', '-')).ToUpperInvariant();

        }

        /// <summary>
        ///   Generates a key from <paramref name="scope"/> and <paramref name="key"/>.</summary>
        /// <param name="scope">
        ///   The scope.</param>
        /// <param name="key">
        ///   The key.</param>
        /// <returns>
        ///   A key.</returns>
        private String Key(Klf200TelegramScope scope, Byte key) {

            return String.Format("{0}#{1}", scope, key).ToUpperInvariant();

        }

        /// <summary>
        ///   Sets (add or replace) a key-value-pair in the catalog.</summary>
        /// </summary>
        /// <param name="key">
        ///   The key.</param>
        /// <param name="value">
        ///   The value.</param>
        private void Set(String key, String value) {

            if (this.Catalog.ContainsKey(key))
                this.Catalog[key] = value;
            else
                this.Catalog.Add(key, value);

        }

        /// <summary>
        ///   Sets (add or replace) a key-value-pair in the catalog.</summary>
        /// </summary>
        /// <param name="key">
        ///   The key.</param>
        /// <param name="value">
        ///   The value.</param>
        private void Set(String key, Byte value) {

            if (this.Catalog.ContainsKey(key))
                this.Catalog[key] = value.ToString();
            else
                this.Catalog.Add(key, value.ToString());

        }

        /// <summary>
        ///   Resolves the name for the <paramref name="scope"/> and <paramref name="identifier"/> value pair.</summary>
        /// <param name="scope">
        ///   The scope.</param>
        /// <param name="identifier">
        ///   The identifier.</param>
        /// <returns>
        ///   A name or an <see cref="String.Empty"/>.</returns>
        public String ResolveName(Klf200TelegramScope scope, Byte identifier) {

            String key = this.Key(scope, identifier);
            if (this.Catalog.ContainsKey(key))
                return this.Catalog[key];
            else
                return String.Empty;

        }

        /// <summary>
        ///   Resolves the identifier for the <paramref name="scope"/> and <paramref name="name"/> value pair.</summary>
        /// <param name="scope">
        ///   The scope.</param>
        /// <param name="name">
        ///   The Name.</param>
        /// <returns>
        ///   An identifier or an <see cref="Byte.MaxValue"/>.</returns>
        public Byte ResolveIdentifier(Klf200TelegramScope scope, String name) {

            String key = this.Key(scope, name);
            if (this.Catalog.ContainsKey(key))
                return Byte.Parse(this.Catalog[key]);
            else
                return Byte.MaxValue;

        }

        /// <summary>
        ///   Returns a list of all items in the catalog for the specified <paramref name="scope"/>.</summary>
        /// <param name="scope">
        ///   The scope.</param>
        /// <returns>
        ///   A list of items for the corresponding <paramref name="scope"/> - or - <see cref="String.Empty"/>.</returns>
        internal String GetCatalogToc(Klf200TelegramScope scope) {

            String scopeItemPrefix = String.Format("{0}#", scope).ToUpperInvariant();
            var scopeItems = this.Catalog
                .Where((kvp) => kvp.Key.StartsWith(scopeItemPrefix, StringComparison.OrdinalIgnoreCase))
                .Select((kvp) => String.Format("{0}={1}", kvp.Key.Substring(scopeItemPrefix.Length), kvp.Value));

            return String.Join(", ", scopeItems);

        }

    }

}
