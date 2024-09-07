using System;

using neleo_com.Logic.Bridges.Velux.Definitions;

namespace neleo_com.Logic.Bridges.Velux.Datagrams {

    /// <summary>
    ///   Base implementation for Velux Klf-200 gateway datagrams that return node information.</summary>
    public abstract class Klf200NodeInformationDatagram : Klf200Datagram, IKlf200DatagramTelegramEncoder {

        /// <summary>
        ///   Create a new command that exposes readable node information properties.</summary>
        /// <param name="id">
        ///   Gateway command id.</param>
        /// <param name="size">
        ///   Size of the data buffer.</param>
        protected Klf200NodeInformationDatagram(Klf200Command id, Int32 size) : base(id, size) { }

        /// <summary>
        ///   NodeID is an Actuator index in the system table, to get information from. 
        ///   It must be a value from 0 to 199.</summary>
        public Byte NodeId {
            get => this.Data.ReadByte(0);
        }

        /// <summary>
        ///   Order can be used to store a sort order. The sort order is used in client end, 
        ///   when presenting a list of nodes for the user.</summary>
        public UInt16 Order {
            get => this.Data.ReadUInt16(1);
        }

        /// <summary>
        ///   Placement can be used to store a room group index or house group index number.</summary>
        public Byte Placement {
            get => this.Data.ReadByte(3);
        }

        /// <summary>
        ///   This field Name holds the name of the actuator, ex. “Window 1”./// </summary>
        public String Name {
            get => this.Data.ReadString(4, 64);
        }

        /// <summary>
        ///   This field indicates what velocity the node is operating with.</summary>
        public GW_NodeVelocity Velocity {
            get => this.Data.ReadEnum<GW_NodeVelocity>(68, 1, GW_NodeVelocity.Undefined);
        }

        /// <summary>
        ///   This field indicates the node type, ex. Window, Roller shutter, Light etc.</summary>
        public GW_NodeType NodeSubType {
            get => this.Data.ReadEnum<GW_NodeType>(69, 2, GW_NodeType.None);
        }

        /// <summary>
        ///   ProductGroup is a single byte, containing the product group number for the gateway, 
        ///   this can be used to identify the gateway. KLF200 is members of remote control product group, 
        ///   therefore ProductGroup is always 14.</summary>
        public Byte ProductGroup {
            get => this.Data.ReadByte(71);
        }

        /// <summary>
        ///   ProductType is a single byte, containing the product type number for the gateway, this 
        ///   can be used to identify the gateway.ProductType is 3 for KLF200.</summary>
        public Byte ProductType {
            get => this.Data.ReadByte(72);
        }

        /// <summary>
        ///   The node variation.</summary>
        public GW_NodeVariation NodeVariation {
            get => this.Data.ReadEnum<GW_NodeVariation>(73, 1, GW_NodeVariation.NotSet);
        }

        /// <summary>
        ///   This field indicates the power mode of the node.</summary>
        public GW_PowerMode PowerMode {
            get => this.Data.ReadEnum<GW_PowerMode>(74, 1, GW_PowerMode.AlwaysAlive);
        }

        /// <summary>
        ///   Software Build number of actuator software.</summary>
        public Byte BuildNumber {
            get => this.Data.ReadByte(75);
        }

        /// <summary>
        ///   This field tells the serial number of the node.</summary>
        public UInt64 SerialNumber {
            get => this.Data.ReadUInt64(76);
        }

        /// <summary>
        ///   This field indicates the operating state of the node.</summary>
        public GW_NodeState NodeState {
            get => this.Data.ReadEnum<GW_NodeState>(84, 1, GW_NodeState.StateUnknown);
        }

        /// <summary>
        ///   This field indicates the current value of the node. This will be a relative value 
        ///   (0x0000 - 0xC800) or ‘No feed-back value known’ (0xF7FF) in case the current value is 
        ///   outside the relative value range or the current value is not known.</summary>
        public UInt16 CurrentValue {
            get => this.Data.ReadUInt16(85);
        }

        /// <summary>
        ///   This field indicates the target value of the current operation. This will be a relative 
        ///   value(0x0000 - 0xC800) or ‘No feed-back value known’ (0xF7FF) in case the target value 
        ///   is outside the relative value range or the target value is not known. </summary>
        public UInt16 TargetValue {
            get => this.Data.ReadUInt16(87);
        }

        /// <summary>
        ///   This field indicates the current value of functional parameter 1. This will be a relative 
        ///   value(0x0000 - 0xC800) or ‘No feed-back value known’ (0xF7FF) in case the FP1 current value
        ///   is outside the relative value range or the FP1 current value is not known.</summary>
        public UInt16 FP1CurrentValue {
            get => this.Data.ReadUInt16(89);
        }

        /// <summary>
        ///   This field indicates the current value of functional parameter 2. This will be a relative 
        ///   value(0x0000 - 0xC800) or ‘No feed-back value known’ (0xF7FF) in case the FP2 current value
        ///   is outside the relative value range or the FP1 current value is not known.</summary>
        public UInt16 FP2CurrentValue {
            get => this.Data.ReadUInt16(91);
        }

        /// <summary>
        ///   This field indicates the current value of functional parameter 3. This will be a relative 
        ///   value(0x0000 - 0xC800) or ‘No feed-back value known’ (0xF7FF) in case the FP3 current value
        ///   is outside the relative value range or the FP1 current value is not known.</summary>
        public UInt16 FP3CurrentValue {
            get => this.Data.ReadUInt16(93);
        }

        /// <summary>
        ///   This field indicates the current value of functional parameter 4. This will be a relative 
        ///   value(0x0000 - 0xC800) or ‘No feed-back value known’ (0xF7FF) in case the FP4 current value
        ///   is outside the relative value range or the FP1 current value is not known.</summary>
        public UInt16 FP4CurrentValue {
            get => this.Data.ReadUInt16(95);
        }

        /// <summary>
        ///   This field indicates the remaining time for a node activation in seconds. If 0 is returned 
        ///   remaining time is unknown or node has reached its target value.</summary>
        public UInt16 RemainingTime {
            get => this.Data.ReadUInt16(97);
        }

        /// <summary>
        ///   UTC time stamp for last known position.</summary>
        public DateTime UtcTimeStamp {
            get => this.Data.ReadDateTime(99);
        }

        // ignore aliases as we don't need them now
        // TODO: add aliases

        /// <summary>
        ///   Creates a telegram out of this datagram's properties.</summary>
        /// <param name="resolver">
        ///   Replace the identifier of a node/group/scene by it's name. Returns <see cref="String.Empty"/> if the identifier
        ///   can't be resolved.</param>
        /// <returns>
        ///   A telegram.</returns>
        public Klf200Telegram EncodeTelegram(Func<Klf200TelegramScope, Byte, String> resolver) {

            Klf200Telegram telegram = new Klf200Telegram() {
                Mode = Klf200TelegramMode.Response,
                Scope = Klf200TelegramScope.Node,
                Identifier = this.NodeId,
                Name = this.Name
            };

            telegram.SetParameter(Klf200TelegramParameter.Type, this.NodeSubType);
            telegram.SetParameter(Klf200TelegramParameter.State, this.NodeState);
            telegram.SetParameter(Klf200TelegramParameter.Current, this.CurrentValue);
            telegram.SetParameter(Klf200TelegramParameter.Target, this.TargetValue);
            telegram.SetParameter(Klf200TelegramParameter.Countdown, this.RemainingTime);

            return telegram;

        }

    }

}
