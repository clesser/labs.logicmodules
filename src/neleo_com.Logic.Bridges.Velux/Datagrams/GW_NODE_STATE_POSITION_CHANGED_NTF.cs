using System;

using neleo_com.Logic.Bridges.Velux.Definitions;

namespace neleo_com.Logic.Bridges.Velux.Datagrams {

    /// <summary>
    ///   This event holds the information on a modified node.</summary>
    public sealed class GW_NODE_STATE_POSITION_CHANGED_NTF : Klf200Datagram, IKlf200DatagramTelegramEncoder {

        /// <summary>
        ///   Initialize the command.</summary>
        public GW_NODE_STATE_POSITION_CHANGED_NTF() : base(Klf200Command.GW_NODE_STATE_POSITION_CHANGED_NTF, 20) { }

        /// <summary>
        ///   NodeID is an Actuator index in the system table, to get information from. 
        ///   It must be a value from 0 to 199.</summary>
        public Byte NodeId {
            get => this.Data.ReadByte(0);
        }

        /// <summary>
        ///   This field indicates the operating state of the node.</summary>
        public GW_NodeState NodeState {
            get => this.Data.ReadEnum<GW_NodeState>(1, 1, GW_NodeState.StateUnknown);
        }

        /// <summary>
        ///   This field indicates the current value of the node. This will be a relative value 
        ///   (0x0000 - 0xC800) or ‘No feed-back value known’ (0xF7FF) in case the current value is 
        ///   outside the relative value range or the current value is not known.</summary>
        public UInt16 CurrentValue {
            get => this.Data.ReadUInt16(2);
        }

        /// <summary>
        ///   This field indicates the target value of the current operation. This will be a relative 
        ///   value(0x0000 - 0xC800) or ‘No feed-back value known’ (0xF7FF) in case the target value 
        ///   is outside the relative value range or the target value is not known. </summary>
        public UInt16 TargetValue {
            get => this.Data.ReadUInt16(4);
        }

        /// <summary>
        ///   This field indicates the current value of functional parameter 1. This will be a relative 
        ///   value(0x0000 - 0xC800) or ‘No feed-back value known’ (0xF7FF) in case the FP1 current value
        ///   is outside the relative value range or the FP1 current value is not known.</summary>
        public UInt16 FP1CurrentValue {
            get => this.Data.ReadUInt16(6);
        }

        /// <summary>
        ///   This field indicates the current value of functional parameter 2. This will be a relative 
        ///   value(0x0000 - 0xC800) or ‘No feed-back value known’ (0xF7FF) in case the FP2 current value
        ///   is outside the relative value range or the FP1 current value is not known.</summary>
        public UInt16 FP2CurrentValue {
            get => this.Data.ReadUInt16(8);
        }

        /// <summary>
        ///   This field indicates the current value of functional parameter 3. This will be a relative 
        ///   value(0x0000 - 0xC800) or ‘No feed-back value known’ (0xF7FF) in case the FP3 current value
        ///   is outside the relative value range or the FP1 current value is not known.</summary>
        public UInt16 FP3CurrentValue {
            get => this.Data.ReadUInt16(10);
        }

        /// <summary>
        ///   This field indicates the current value of functional parameter 4. This will be a relative 
        ///   value(0x0000 - 0xC800) or ‘No feed-back value known’ (0xF7FF) in case the FP4 current value
        ///   is outside the relative value range or the FP1 current value is not known.</summary>
        public UInt16 FP4CurrentValue {
            get => this.Data.ReadUInt16(12);
        }

        /// <summary>
        ///   This field indicates the remaining time for a node activation in seconds. If 0 is returned 
        ///   remaining time is unknown or node has reached its target value.</summary>
        public UInt16 RemainingTime {
            get => this.Data.ReadUInt16(14);
        }

        /// <summary>
        ///   UTC time stamp for last known position.</summary>
        public DateTime UtcTimeStamp {
            get => this.Data.ReadDateTime(16);
        }

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
                Identifier = this.NodeId
            };

            if (resolver != null)
                telegram.Name = resolver.Invoke(Klf200TelegramScope.Node, this.NodeId);

            telegram.SetParameter(Klf200TelegramParameter.State, this.NodeState);
            telegram.SetParameter(Klf200TelegramParameter.Current, this.CurrentValue);
            telegram.SetParameter(Klf200TelegramParameter.Target, this.TargetValue);
            telegram.SetParameter(Klf200TelegramParameter.Countdown, this.RemainingTime);

            return telegram;

        }

    }

}
