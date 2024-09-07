using System;

namespace neleo_com.Logic.Bridges.Velux.Datagrams {

    /// <summary>
    ///   Request information about a specific scene.</summary>
    public sealed class GW_GET_SCENE_INFORMATION_REQ : Klf200Datagram {

        /// <summary>
        ///   Initialize the command.</summary>
        /// <param name="sceneId">
        ///   The scene identifier.</param>
        public GW_GET_SCENE_INFORMATION_REQ(Byte sceneId) : base(Klf200Command.GW_GET_SCENE_INFORMATION_REQ, 1) {

            this.Data.WriteByte(sceneId, 0);

        }

    }

}