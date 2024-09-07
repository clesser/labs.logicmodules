using System;

namespace neleo_com.Logic.Bridges.Velux {

    /// <summary>
    ///   Implementation of a base Velux Klf-200 gateway datagram 
    ///   that will be endcoded/decoded in <see cref="Klf200Socket"/>.</summary>
    public abstract class Klf200Datagram {

        /// <summary>
        ///   Captures the value for <see cref="Id"/>.</summary>
        protected readonly Klf200Command _Id;

        /// <summary>
        ///   Captures the value for <see cref="Data"/>.</summary>
        protected readonly Byte[] _Data;

        /// <summary>
        ///   Create a new command.</summary>
        /// <param name="id">
        ///   Gateway command id.</param>
        /// <param name="size">
        ///   Size of the data buffer.</param>
        protected Klf200Datagram(Klf200Command id, Int32 size) {

            this._Id = id;
            this._Data = new Byte[size];

        }

        /// <summary>
        ///   Velux KLF-200 gateway command identifier.</summary>
        public Klf200Command Id {
            get => this._Id;
        }

        /// <summary>
        ///   Access to the command data.</summary>
        public Byte[] Data {
            get => this._Data;
        }

    }

}
