using System;

namespace neleo_com.Logic.Bridges.Velux.Datagrams {

    /// <summary>
    ///   Request a list of all scenes.</summary>
    public sealed class GW_GET_SCENE_LIST_REQ : Klf200Datagram {

        /// <summary>
        ///   Initialize the command.</summary>
        public GW_GET_SCENE_LIST_REQ() : base(Klf200Command.GW_GET_SCENE_LIST_REQ, 0) { }

    }

}
