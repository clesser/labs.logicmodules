using System;

using Newtonsoft.Json;

namespace neleo_com.Logic.Bridges.Nuki.Datagrams {

    /// <summary>
    ///   Response for an state info request.</summary>
    [JsonObject]
    public class NukiDeviceState: NukiActionState {

        /// <summary>
        ///   Lock state of a Nuki device.</summary>
        [JsonProperty("state")]
        public Int32 LockState {
            get; set;
        }

        /// <summary>
        ///   State of the door sensor.</summary>
        [JsonProperty("doorsensorState")]
        public Int32 DoorState {
            get; set;
        }

        /// <summary>
        ///   Remaining battery life in percent.</summary>
        [JsonProperty("batteryChargeState")]
        public Int32 BatteryState {
            get; set;
        }

    }

}
