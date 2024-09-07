using System;

using neleo_com.Logic.Bridges.Velux.Definitions;

namespace neleo_com.Logic.Bridges.Velux.Datagrams {

    /// <summary>
    ///   Confirmation for <see cref="GW_ACTIVATE_SCENE_REQ"/>.</summary>
    public sealed class GW_ACTIVATE_SCENE_CFM : Klf200Datagram, IKlf200DatagramSessionResponse {

        /// <summary>
        ///   Initialize the command.</summary>
        public GW_ACTIVATE_SCENE_CFM() : base(Klf200Command.GW_ACTIVATE_SCENE_CFM, 3) { }

        /// <summary>
        ///   Gets a session idenfier.</summary>
        public UInt16 SessionId {
            get => this.Data.ReadUInt16(0);
        }

        /// <summary>
        ///   Gets an indicator whether this is the final datagram for this session.</summary>
        public Boolean IsFinal {
            get => false;
        }

        /// <summary>
        ///   Indicates if the corresponding <see cref="GW_ACTIVATE_SCENE_REQ"/> command is accepted 
        ///   or rejected by the Command Handler.</summary>
        public GW_SceneCommandRequestState State {
            get => this.Data.ReadEnum<GW_SceneCommandRequestState>(2, 1, GW_SceneCommandRequestState.RequestRejected);
        }

    }

}
