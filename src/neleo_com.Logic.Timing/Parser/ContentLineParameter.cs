using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text.RegularExpressions;

namespace neleo_com.Logic.Timing.Parser {

    /// <summary>
    ///   A parameter of a content line.</summary>
    public class ContentLineParameter : Collection<String> {

        private const String NameValuePattern = "(.+?)=(.+)";
        private const String ValueListPattern = "([^,]+)(?=,|$)";

        /// <summary>
        ///   Name of the parameter.</summary>
        public String Name {
            get; private set;
        }

        /// <summary>
        ///   Creates a new content line parameter by parsing a text line.</summary>
        /// <param name="source">
        ///   The source text line.</param>
        public ContentLineParameter(String source) {

            Match match = Regex.Match(source, ContentLineParameter.NameValuePattern);

            this.Name = match.Groups[1].ToString().Trim();
            String valueString = match.Groups[2].ToString();

            MatchCollection matches = Regex.Matches(valueString, ContentLineParameter.ValueListPattern);
            foreach (Match paramMatch in matches)
                this.Add(paramMatch.Groups[1].ToString().Trim());

        }

    }

}
