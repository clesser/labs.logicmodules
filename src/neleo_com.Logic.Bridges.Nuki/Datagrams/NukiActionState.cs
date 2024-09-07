using System;

using Newtonsoft.Json;

namespace neleo_com.Logic.Bridges.Nuki.Datagrams {

    /// <summary>
    ///   Response for an action request.</summary>
    [JsonObject]
    public class NukiActionState {

        /// <summary>
        ///   Flag indicating if the action has been executed successfully.</summary>
        [JsonProperty("success")]
        public Boolean Success {
            get; set;
        }

    }

}
