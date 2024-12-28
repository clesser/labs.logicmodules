using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace neleo_com.Logic.Timing.Parser {

    /// <summary>
    ///   A collection of content line parameters.</summary>
    public class ContentLineParameters : Dictionary<String, ContentLineParameter> {

        private const String ParameterPattern = "([^;]+)(?=;|$)";

        /// <summary>
        ///   Creates a new content line parameter collection by parsing the <paramref name="source"/>.</summary>
        /// <param name="source">
        ///   The parameter definition.</param>
        public ContentLineParameters(String source) {

            MatchCollection matches = Regex.Matches(source, ContentLineParameters.ParameterPattern);
            foreach (Match match in matches) {

                ContentLineParameter contentLineParameter = new ContentLineParameter(match.Groups[1].ToString());
                this[contentLineParameter.Name] = contentLineParameter;

            }

        }

        /// <summary>
        ///   Checks if the specified key-value-pair exists in the list of parameters.</summary>
        /// <param name="key">
        ///   A key.</param>
        /// <param name="value">
        ///   A value.</param>
        /// <returns>
        ///   <c>true</c> if the key-value-pair exists, otherwise <c>false</c>.</returns>
        public Boolean Contains(String key, String value) {

            return this.ContainsKey(key) && this[key].Contains(value);

        }

    }

}
