using System;
using System.Collections.Generic;

namespace neleo_com.Logic.Bridges.Velux.Datagrams {

    /// <summary>
    ///   Information about 0..3 scenes.</summary>
    public sealed class GW_GET_SCENE_LIST_NTF : Klf200Datagram {

        /// <summary>
        ///   Initialize the command.</summary>
        public GW_GET_SCENE_LIST_NTF() : base(Klf200Command.GW_GET_SCENE_LIST_NTF, 197) { }

        /// <summary>
        ///   List of all scenes that are defined in this datagram.</summary>
        public IReadOnlyDictionary<Byte, String> Scenes {

            get {

                // create a list of scenes
                Dictionary<Byte, String> scenes = new Dictionary<Byte, String>();

                // iterate through the datagram to find scenes
                for (Int32 sceneIndex = 0; sceneIndex < this.Data.ReadByte(0); sceneIndex++) {

                    Int32 pos = 1 + (sceneIndex * 65);
                    scenes.Add(this.Data.ReadByte(pos), this.Data.ReadString(pos + 1, 64));

                }

                // return list of scenes
                return scenes;

            }

        }

    }

}
