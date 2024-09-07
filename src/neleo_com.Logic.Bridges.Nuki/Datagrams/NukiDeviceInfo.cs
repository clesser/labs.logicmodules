using System;

using Newtonsoft.Json;

namespace neleo_com.Logic.Bridges.Nuki.Datagrams {

    /// <summary>
    ///   Response for an devie info request.</summary>
    [JsonObject]
    public class NukiDeviceInfo {

        /// <summary>
        ///   The device identifier (= nukiId).</summary>
        [JsonProperty("nukiId")]
        public String DeviceId {
            get; set;
        }

        /// <summary>
        ///   The device type (Smart Lock == 0).</summary>
        [JsonProperty("deviceType")]
        public Int32 DeviceType {
            get; set;
        }

        /// <summary>
        ///   The device state (lock state, door state, battery state).</summary>
        [JsonProperty("lastKnownState")]
        public NukiDeviceState DeviceState {
            get; set;
        }

    }

}
