using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

using LogicModule.Nodes.Helpers;
using LogicModule.ObjectModel;
using LogicModule.ObjectModel.TypeSystem;

namespace neleo_com.Logic.Control {

    /// <summary>
    ///   Definition of the value type and comparison mode for proper comparison.</summary>
    public static class ComparisonMode {

        public const String Number = nameof(ComparisonMode.Number);
        public const String Text = nameof(ComparisonMode.Text);
        public const String TextIgnoreCase = nameof(ComparisonMode.TextIgnoreCase);

    }

    /// <summary>
    ///   A switch-case logic module to avoid multiple 
    /// </summary>
    public class Switch : LogicNodeBase {

        /// <summary>
        ///   The Type Service manages incoming and outgoing ports.</summary>
        private readonly ITypeService TypeService;

        /// <summary>
        ///   Minimal numer of case-statements.</summary>
        private const Int32 MinCases = 1;

        /// <summary>
        ///   Maximum numer of case-statements.</summary>
        private const Int32 MaxCases = 15;

        /// <summary>
        ///   A set of options to configure the logic component's input value type.</summary>
        [Parameter(DisplayOrder = 1, IsDefaultShown = false)]
        public EnumValueObject Comparison {
            get; private set;
        }

        /// <summary>
        ///   The value input port (either a parameter or value).</summary>
        [Input(DisplayOrder = 2, IsInput = true)]
        public DoubleValueObject InputNumber {
            get; private set;
        }

        /// <summary>
        ///   The value input port (either a parameter or value).</summary>
        [Input(DisplayOrder = 3, IsInput = true)]
        public StringValueObject InputText {
            get; private set;
        }

        /// <summary>
        ///   Defines the number of cases / statements.</summary>
        [Parameter(DisplayOrder = 4, IsDefaultShown = false)]
        public IntValueObject Cases {
            get; private set;
        }

        /// <summary>
        ///   Each case defines a value which be validated for equality.</summary>
        [Parameter(DisplayOrder = 5, IsDefaultShown = true)]
        public IList<DoubleValueObject> CaseNumberFilter {
            get; private set;
        }

        /// <summary>
        ///   Each case defines a value which be validated for equality.</summary>
        [Parameter(DisplayOrder = 6, IsDefaultShown = true)]
        public IList<StringValueObject> CaseTextFilter {
            get; private set;
        }

        /// <summary>
        ///   If an incoming telegram doesn't match any case filter, 
        ///   the corresponding trigger will return "1", otherwise "0".</summary>
        [Output(DisplayOrder = 1, IsDefaultShown = true)]
        public BoolValueObject CaseUndefinedTrigger {
            get; private set;
        }

        /// <summary>
        ///   If the incoming telegram matches a case filter, 
        ///   the corresponding trigger will return "1", otherwise "0".</summary>
        [Output(DisplayOrder = 2, IsDefaultShown = true)]
        public IList<BoolValueObject> CaseTrigger {
            get; private set;
        }

        /// <summary>
        ///   Constructor to setup the ports and services.</summary>
        /// <param name="context">
        ///   Context of the node instance to connect to services.</param>
        public Switch(INodeContext context) {

            context.ThrowIfNull(nameof(context));

            this.TypeService = context.GetService<ITypeService>();

            this.Comparison = this.TypeService.CreateEnum(nameof(ComparisonMode), nameof(this.Comparison),
                new String[] { ComparisonMode.Number, ComparisonMode.Text, ComparisonMode.TextIgnoreCase }, ComparisonMode.Number);
            this.Comparison.ValueSet += this.Comparison_ValueSet;

            this.Cases = this.TypeService.CreateInt(PortTypes.Integer, nameof(this.Cases), 3);
            this.Cases.MinValue = Switch.MinCases;
            this.Cases.MaxValue = Switch.MaxCases;

            this.InputNumber = this.TypeService.CreateDouble(PortTypes.Number, nameof(this.InputNumber));
            this.CaseNumberFilter = new List<DoubleValueObject>();
            ListHelpers.ConnectListToCounter(this.CaseNumberFilter, this.Cases,
                this.TypeService.GetValueObjectCreator(PortTypes.Number, nameof(this.CaseNumberFilter)), null);

            this.CaseUndefinedTrigger = this.TypeService.CreateBool(PortTypes.Binary, nameof(this.CaseUndefinedTrigger), false);

            this.CaseTrigger = new List<BoolValueObject>();
            ListHelpers.ConnectListToCounter(this.CaseTrigger, this.Cases,
                this.TypeService.GetValueObjectCreator(PortTypes.Binary, nameof(this.CaseTrigger), false), null);

        }

        /// <summary>
        ///   Handle configuration changes for value or comparison type.</summary>
        /// <param name="sender">
        ///   The sender.</param>
        /// <param name="args">
        ///   Information about changed properties.</param>
        private void Comparison_ValueSet(Object sender, ValueChangedEventArgs args) {

            switch (args.NewValue) {

                case ComparisonMode.Number:

                    if (this.InputNumber == null) {

                        this.InputNumber = this.TypeService.CreateDouble(PortTypes.Number, nameof(this.InputNumber));
                        this.CaseNumberFilter = new List<DoubleValueObject>();
                        ListHelpers.ConnectListToCounter(this.CaseNumberFilter, this.Cases,
                            this.TypeService.GetValueObjectCreator(PortTypes.Number, nameof(this.CaseNumberFilter)), null);
                        this.InputText = null;
                        this.CaseTextFilter = null;
                    }
                    break;

                case ComparisonMode.Text:
                case ComparisonMode.TextIgnoreCase:

                    if (this.InputText == null) {

                        this.InputText = this.TypeService.CreateString(PortTypes.String, nameof(this.InputText));
                        this.CaseTextFilter = new List<StringValueObject>();
                        ListHelpers.ConnectListToCounter(this.CaseTextFilter, this.Cases,
                            this.TypeService.GetValueObjectCreator(PortTypes.String, nameof(this.CaseTextFilter)), null);
                        this.InputNumber = null;
                        this.CaseNumberFilter = null;
                    }
                    break;

            }

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
            if (key.StartsWith(nameof(this.CaseNumberFilter))) {

                String identifier = key.Substring(nameof(this.CaseNumberFilter).Length);
                return String.Format(ResourceManager.GetString(nameof(this.CaseNumberFilter), culture) ?? key, identifier);

            }
            else if (key.StartsWith(nameof(this.CaseTextFilter))) {

                String identifier = key.Substring(nameof(this.CaseTextFilter).Length);
                return String.Format(ResourceManager.GetString(nameof(this.CaseTextFilter), culture) ?? key, identifier);

            }
            else if (key.StartsWith(nameof(this.CaseTrigger))) {

                String identifier = key.Substring(nameof(this.CaseTrigger).Length);
                return String.Format(ResourceManager.GetString(nameof(this.CaseTrigger), culture) ?? key, identifier);

            }
            else {

                return ResourceManager.GetString(key, culture) ?? key;

            }

        }

        /// <summary>
        ///   Evaluates incoming telegrams and triggers the output ports based on the case filter definitions.</summary>
        public override void Execute() {

            if (this.InputNumber != null && this.InputNumber.HasValue && this.InputNumber.WasSet) {

                Boolean matched = false;
                for (Int32 caseIndex = 0; caseIndex < this.CaseNumberFilter.Count(); caseIndex++) {

                    DoubleValueObject caseFilter = this.CaseNumberFilter[caseIndex];
                    Boolean match = caseFilter.Value.Equals(this.InputNumber.Value);
                    matched |= match;
                    this.TriggerOnChange(this.CaseTrigger[caseIndex], match);

                }

                this.TriggerOnChange(this.CaseUndefinedTrigger, !matched);

            }
            else if (this.InputText != null && this.InputText.HasValue && this.InputText.WasSet) {

                Boolean matched = false;
                StringComparison comparison = this.Comparison.Value.Equals(ComparisonMode.TextIgnoreCase) ? StringComparison.OrdinalIgnoreCase : StringComparison.CurrentCulture;

                for (Int32 caseIndex = 0; caseIndex < this.CaseTextFilter.Count(); caseIndex++) {

                    StringValueObject caseFilter = this.CaseTextFilter[caseIndex];
                    Boolean match = caseFilter.Value.Equals(this.InputText.Value, comparison);
                    matched |= match;
                    this.TriggerOnChange(this.CaseTrigger[caseIndex], match);

                }

                this.TriggerOnChange(this.CaseUndefinedTrigger, !matched);

            }

        }

        /// <summary>
        ///   Sends a telegram if the value differs from the currently set value.</summary>
        /// <param name="output">
        ///   The output port.</param>
        /// <param name="value">
        ///   The value.</param>
        private void TriggerOnChange(BoolValueObject output, Boolean value) {

            if (output != null)
                if (!output.HasValue || (output.HasValue && output.Value != value))
                    output.Value = value;

        }

    }

}
