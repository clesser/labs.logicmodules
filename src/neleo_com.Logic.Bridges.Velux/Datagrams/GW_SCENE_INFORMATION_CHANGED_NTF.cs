using System;

namespace neleo_com.Logic.Bridges.Velux.Datagrams {

    /// <summary>
    ///   This event references a modified scene.</summary>
    public sealed class GW_SCENE_INFORMATION_CHANGED_NTF : Klf200Datagram {

        /// <summary>
        ///   Initialize the command.</summary>
        public GW_SCENE_INFORMATION_CHANGED_NTF() : base(Klf200Command.GW_SCENE_INFORMATION_CHANGED_NTF, 2) { }

        /// <summary>
        ///   Indicates that a scene has been deleted.</summary>
        public Boolean IsDeleted {
            get => this.Data.ReadByte(0) == 0;
        }

        /// <summary>
        ///   Indicates that the scene information has been modified.</summary>
        public Boolean IsModified {
            get => this.Data.ReadByte(0) == 1;
        }

        /// <summary>
        ///   Reference to the scene that has been modified.</summary>
        public Byte SceneId {
            get => this.Data.ReadByte(1);
        }

    }

}
