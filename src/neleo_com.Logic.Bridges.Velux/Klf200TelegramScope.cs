using System;

namespace neleo_com.Logic.Bridges.Velux {

    /// <summary>
    ///   Definition of the telegram scopes.</summary>
    public enum Klf200TelegramScope {

        /// <summary>
        ///   Scope is not defined.</summary>
        None,

        /// <summary>
        ///   Individual devices (nodes).</summary>
        Node,

        /// <summary>
        ///   Grouped devices (nodes).</summary>
        Group,

        /// <summary>
        ///   Scenes.</summary>
        Scene

    }

}
