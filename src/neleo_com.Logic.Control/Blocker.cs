using System;
using System.Collections.Generic;
using System.Linq;

using LogicModule.ObjectModel;
using LogicModule.ObjectModel.TypeSystem;
using LogicModule.Nodes.Helpers;

namespace neleo_com.Logic.Control {

    /// <summary>
    ///   Options to control sending a value telegram when stopping telegram routing.</summary>
    public static class NoneDefined {

        public const String None = nameof(NoneDefined.None);
        public const String Defined = nameof(NoneDefined.Defined);

    }

    /// <summary>
    ///   Options to control sending a value telegram when starting telegram routing.</summary>
    public static class NoneDefinedCached {

        public const String None = nameof(NoneDefinedCached.None);
        public const String Defined = nameof(NoneDefinedCached.Defined);
        public const String Cached = nameof(NoneDefinedCached.Cached);

    }

    /// <summary>
    ///   A blocker node that can send the last incoming value when disabling it.</summary>
    public class Blocker : LogicNodeBase {

        /// <summary>
        ///   The Type Service manages incoming and outgoing ports.</summary>
        private readonly ITypeService TypeService;

        /// <summary>
        ///   The Editor Service ensures that incoming and outgoing ports are in sync.</summary>
        private readonly IEditorService EditorService;

        /// <summary>
        ///   The value input port (either a parameter or value).</summary>
        [Input(DisplayOrder = 1)]
        public AnyValueObject Input {
            get; private set;
        }

        /// <summary>
        ///   Saves the state of <see cref="Enabled"/> to allow newValue-oldValue-comparison.</summary>
        private Boolean IsEnabled {
            get; set;
        }

        /// <summary>
        ///   Switch to control if the logic module routes the value telegrams through.</summary>
        [Input(DisplayOrder = 2)]
        public BoolValueObject Enabled {
            get; private set;
        }

        /// <summary>
        ///   Switch to send a pre-defined value when stop routing value telegrams.</summary>
        [Parameter(IsDefaultShown = false, DisplayOrder = 3)]
        public EnumValueObject SendOnActivation {
            get; private set;
        }

        /// <summary>
        ///   The value that should be send when stop routing value telegrams.</summary>
        [Parameter(IsDefaultShown = false, DisplayOrder = 4)]
        public AnyValueObject ValueOnActivation {
            get; private set;
        }

        /// <summary>
        ///   Switch to send a cached or pre-defined value when start routing value telegrams.</summary>
        [Parameter(IsDefaultShown = false, DisplayOrder = 5)]
        public EnumValueObject SendOnDeactivation {
            get; private set;
        }

        /// <summary>
        ///   The value that should be send when start routing value telegrams.</summary>
        [Parameter(IsDefaultShown = false, DisplayOrder = 6)]
        public AnyValueObject ValueOnDeactivation {
            get; private set;
        }

        /// <summary>
        ///   The value output port.</summary>
        [Output(IsRequired = true)]
        public AnyValueObject Output {
            get; private set;
        }

        /// <summary>
        ///   Constructor to setup the ports and services.</summary>
        /// <param name="context">
        ///   Context of the node instance to connect to services.</param>
        public Blocker(INodeContext context) : base(context) {

            context.ThrowIfNull(nameof(context));

            this.TypeService = context.GetService<ITypeService>();
            this.EditorService = context.GetService<IEditorService>();

            this.Input = this.TypeService.CreateAny(PortTypes.Any, nameof(this.Input));
            this.Output = this.TypeService.CreateAny(PortTypes.Any, nameof(this.Output));

            this.IsEnabled = false;
            this.Enabled = this.TypeService.CreateBool(PortTypes.Binary, nameof(this.Enabled), this.IsEnabled);

            this.SendOnActivation = this.TypeService.CreateEnum(nameof(NoneDefined), nameof(this.SendOnActivation),
                new String[] { NoneDefined.None, NoneDefined.Defined }, NoneDefined.None);
            this.SendOnActivation.ValueSet += this.SendOnActivation_ValueSet;

            this.SendOnDeactivation = this.TypeService.CreateEnum(nameof(NoneDefinedCached), nameof(this.SendOnDeactivation),
                new String[] { NoneDefinedCached.None, NoneDefinedCached.Defined, NoneDefinedCached.Cached }, NoneDefinedCached.None);
            this.SendOnDeactivation.ValueSet += this.SendOnDeactivation_ValueSet;

            this.SyncPortTypes();

        }

        /// <summary>
        ///   Handle configuration changes for blocker activation value telegrams.</summary>
        /// <param name="sender">
        ///   The sender.</param>
        /// <param name="args">
        ///   Information about changed properties.</param>
        private void SendOnActivation_ValueSet(Object sender, ValueChangedEventArgs args) {

            if (!args.NewValue.Equals(args.OldValue)) {

                if (args.NewValue.Equals(NoneDefined.Defined))
                    this.ValueOnActivation = this.TypeService.CreateAny(PortTypes.Any, nameof(this.ValueOnActivation));
                else
                    this.ValueOnActivation = null;

                this.SyncPortTypes();

            }

        }

        /// <summary>
        ///   Handle configuration changes for blocker activation value telegrams.</summary>
        /// <param name="sender">
        ///   The sender.</param>
        /// <param name="args">
        ///   Information about changed properties.</param>
        private void SendOnDeactivation_ValueSet(Object sender, ValueChangedEventArgs args) {

            if (!args.NewValue.Equals(args.OldValue)) {

                if (args.NewValue.Equals(NoneDefinedCached.Defined))
                    this.ValueOnDeactivation = this.TypeService.CreateAny(PortTypes.Any, nameof(this.ValueOnDeactivation));
                else
                    this.ValueOnDeactivation = null;

                this.SyncPortTypes();

            }

        }

        /// <summary>
        ///   Synchronizes the ports to share the same value type.</summary>
        private void SyncPortTypes() {

            ICollection<IValueObject> ports = new List<IValueObject> { this.Output, this.Input };

            if (this.ValueOnActivation != null)
                ports.Add(this.ValueOnActivation);

            if (this.ValueOnDeactivation != null)
                ports.Add(this.ValueOnDeactivation);

            this.EditorService.ClearAllSharedTypeRegistration(this);
            this.EditorService.RegisterSharedType(this, ports.ToArray());

        }

        /// <summary>
        ///   This method implements the routing logic. If routing is enabled, it will pass the incoming value 
        ///   to the output port. 
        ///   If the routing gets either enabled or disabled, predefined or cached values will be send.</summary>
        public override void Execute() {

            if (this.Enabled.WasSet) {

                if (this.Enabled.Value && !this.IsEnabled) {

                    if (NoneDefined.Defined.Equals(this.SendOnActivation.Value))
                        this.Output.Value = this.ValueOnActivation.Value;

                }
                else if (!this.Enabled.Value && this.IsEnabled) {

                    if (NoneDefinedCached.Defined.Equals(this.SendOnDeactivation.Value))
                        this.Output.Value = this.ValueOnDeactivation.Value;

                    if (NoneDefinedCached.Cached.Equals(this.SendOnDeactivation.Value))
                        this.Output.Value = this.Input.Value;

                }

                this.IsEnabled = this.Enabled.Value;

            }

            if (this.Input.WasSet && !this.Enabled.Value)
                this.Output.Value = this.Input.Value;

        }

    }

}
