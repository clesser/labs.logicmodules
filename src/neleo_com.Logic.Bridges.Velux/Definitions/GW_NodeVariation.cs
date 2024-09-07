using System;

namespace neleo_com.Logic.Bridges.Velux.Definitions {

    /// <summary>
    ///   Information about node variations.</summary>
    public enum GW_NodeVariation : Byte {

        /// <summary>
        ///   Not set.</summary>
        NotSet = 0,

        /// <summary>
        ///   Window is a top hung window.</summary>
        TopHung = 1,

        /// <summary>
        ///   Window is a kip window.</summary>
        Kip = 2,

        /// <summary>
        ///   Window is a flat roof.</summary>
        FlatRoof = 3,

        /// <summary>
        ///   Window is a sky light.</summary>
        SkyLight = 4

    }

}
