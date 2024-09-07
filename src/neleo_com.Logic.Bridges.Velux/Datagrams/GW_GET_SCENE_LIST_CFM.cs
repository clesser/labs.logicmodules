using System;

namespace neleo_com.Logic.Bridges.Velux.Datagrams {

    /// <summary>
    ///   Confirmation for <see cref="GW_GET_SCENE_LIST_REQ"/>.</summary>
    public sealed class GW_GET_SCENE_LIST_CFM : Klf200Datagram {

        /// <summary>
        ///   Initialize the command.</summary>
        public GW_GET_SCENE_LIST_CFM() : base(Klf200Command.GW_GET_SCENE_LIST_CFM, 1) { }

        /// <summary>
        ///   Reads the number of scenes.</summary>
        public Byte SceneCount {
            get => this.Data.ReadByte(0);
        }

    }

}
