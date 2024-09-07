using System;

namespace neleo_com.Logic.Bridges.Velux.Datagrams {

    /// <summary>
    ///   This event holds the information on a scene.</summary>
    public sealed class GW_GET_SCENE_INFORMATION_NTF : Klf200Datagram {

        /// <summary>
        ///   Initialize the command.</summary>
        public GW_GET_SCENE_INFORMATION_NTF() : base(Klf200Command.GW_GET_SCENE_INFORMATION_NTF, 247) { }

        /// <summary>
        ///   SceneID is the index in the system table, to get information from. 
        ///   It must be a value from 0 to 31.</summary>
        public Byte SceneId {
            get => this.Data.ReadByte(0);
        }

        /// <summary>
        ///   This field Name holds the name of the scene./// </summary>
        public String Name {
            get => this.Data.ReadString(1, 64);
        }

        // ignore all other parameters

    }

}
