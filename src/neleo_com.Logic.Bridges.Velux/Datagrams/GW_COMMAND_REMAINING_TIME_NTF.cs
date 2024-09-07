using System;

using neleo_com.Logic.Bridges.Velux.Definitions;

namespace neleo_com.Logic.Bridges.Velux.Datagrams {

    /// <summary>
    ///   This command tells how long it takes until the actuator has reached the desired position.</summary>
    public sealed class GW_COMMAND_REMAINING_TIME_NTF : Klf200Datagram, IKlf200DatagramSessionResponse, IKlf200DatagramTelegramEncoder {

        /// <summary>
        ///   Initialize the command.</summary>
        public GW_COMMAND_REMAINING_TIME_NTF() : base(Klf200Command.GW_COMMAND_REMAINING_TIME_NTF, 6) { }

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
        ///   NodeID is an Actuator index in the system table, to get information from. 
        ///   It must be a value from 0 to 199.</summary>
        public Byte NodeId {
            get => this.Data.ReadByte(2);
        }

        /// <summary>
        ///   Identifies the parameter that ParameterValue carry information about.</summary>
        public GW_ParameterType ParameterId {
            get => this.Data.ReadEnum<GW_ParameterType>(3, 1, GW_ParameterType.NotUsed);
        }

        /// <summary>
        ///   This field indicates the remaining time for a node parameter activation in seconds. If 0 is returned 
        ///   remaining time is unknown or node parameter has reached its target value.</summary>
        public UInt16 RemainingTime {
            get => this.Data.ReadUInt16(4);
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

            // resolve node name
            if (resolver != null)
                telegram.Name = resolver.Invoke(Klf200TelegramScope.Node, this.NodeId);

            // export parameter
            // identifiers of GW_ParameterType and Klf200TelegramParameters have an 0x40 offset
            // Main parameter = 0x40 .. Functional Parameter 16 = 0x50
            if ((Byte)this.ParameterId <= 0x10) {

                Byte parameterId = (Byte)(0x40 + (Byte)this.ParameterId);
                telegram.SetParameter((Klf200TelegramParameter)parameterId, this.RemainingTime);

            }

            return telegram;

        }

    }

}
