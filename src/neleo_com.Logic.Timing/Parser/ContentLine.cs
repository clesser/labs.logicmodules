using System;
using System.Text.RegularExpressions;

namespace neleo_com.Logic.Timing.Parser {

    /// <summary>
    ///   A content line in a calendar definition.</summary>
    public class ContentLine {

        public const String ContentPattern = "(.+?):(.+?)(?=\\r\\n[A-Z]|$)";
        public const RegexOptions ContentOptions = RegexOptions.Singleline;

        private const String ComponentsPattern = "(.+?)((;.+?)*):(.+)";
        private const RegexOptions ComponentsOptions = RegexOptions.Singleline;

        /// <summary>
        ///   The raw line (source).</summary>
        public String Source {
            get; private set;
        }

        /// <summary>
        ///   Name of the content line.</summary>
        public String Name {
            get; private set;
        }

        /// <summary>
        ///   Parameters of the content line.</summary>
        public ContentLineParameters Parameters {
            get; private set;
        }

        /// <summary>
        ///   value of the content line.</summary>
        public String Value {
            get; private set;
        }

        /// <summary>
        ///   Creates a new content line by parsing a text input.</summary>
        /// <param name="source">
        ///   The input source.</param>
        public ContentLine(String source) {

            this.Source = source.UnfoldAndUnescape();

            Match match = Regex.Match(this.Source, ContentLine.ComponentsPattern, ContentLine.ComponentsOptions);

            this.Name = match.Groups[1].ToString().Trim();

            this.Parameters = new ContentLineParameters(match.Groups[2].ToString());

            // values can be empty; in this case the next line is captured by the RegEx; catch next content line
            String value = match.Groups[4].ToString();
            if (value.StartsWith("\r\n"))
                this.Value = String.Empty;
            else
                this.Value = value.Trim();

        }

    }

}
