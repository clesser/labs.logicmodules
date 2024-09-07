using System;

using neleo_com.Logic.Bridges.Velux.Definitions;

namespace neleo_com.Logic.Bridges.Velux.Datagrams {

    /// <summary>
    ///   The gateway can handle activation of all actuators in user defined scene.</summary>
    public sealed class GW_ACTIVATE_SCENE_REQ : Klf200Datagram, IKlf200DatagramTelegramDecoder, IKlf200DatagramSessionRequest {

        /// <summary>
        ///   Initialize the command.</summary>
        public GW_ACTIVATE_SCENE_REQ() : base(Klf200Command.GW_ACTIVATE_SCENE_REQ, 6) { }

        /// <summary>
        ///   Sets or gets a session idenfier.</summary>
        /// <remarks>
        ///   SessionId will be set in <see cref="Klf200Socket.SendRequestAsync()"/>.</remarks>
        public UInt16 SessionId {
            get => this.Data.ReadUInt16(0);
            set => this.Data.WriteUInt16(value, 0);
        }

        /// <summary>
        ///   Specifies the command originator type (USER/TIMER/SECURITY etc.) 
        ///   Typically, only USER or SAAC are used./summary>
        public GW_CommandSource CommandOriginator {
            get => this.Data.ReadEnum<GW_CommandSource>(2, 1, GW_CommandSource.User);
            set => this.Data.WriteEnum<GW_CommandSource>(value, 2, 1);
        }

        /// <summary>
        ///   PriorityLevel defines the priority level, of the activating command.
        ///   Typically, PriorityLevel will be set to ‘3’ for user level 2 or ‘5’ for Comfort Level 2.</summary>
        public GW_CommandPriority PriorityLevel {
            get => this.Data.ReadEnum<GW_CommandPriority>(3, 1, GW_CommandPriority.UserLevel2);
            set => this.Data.WriteEnum<GW_CommandPriority>(value, 3, 1);
        }

        /// <summary>
        ///   SceneID is the index in the system table, to get information from. 
        ///   It must be a value from 0 to 31.</summary>
        public Byte SceneId {
            get => this.Data.ReadByte(4);
            set => this.Data.WriteByte(value, 4);
        }

        /// <summary>
        ///   This field indicates what velocity the nodes are operating with.</summary>
        public GW_NodeVelocity Velocity {
            get => this.Data.ReadEnum<GW_NodeVelocity>(5, 1, GW_NodeVelocity.Undefined);
            set => this.Data.WriteEnum<GW_NodeVelocity>(value, 5, 1);
        }

        /// <summary>
        ///   Tries to decompose the telegram's parameters to set the datagram's properties.</summary>
        /// <param name="telegram">
        ///   A telegram.</param>
        /// <param name="resolver">
        ///   Replace the name of a node/group/scene by it's identifier. Returns <see cref="Byte.MaxValue"/> if the name
        ///   can't be resolved.</param>
        /// <returns>
        ///   <c>true</c> if decomposition was successful (all required properties were set); otherwise <c>false</c>.</returns>
        public Boolean DecodeTelegram(Klf200Telegram telegram, Func<Klf200TelegramScope, String, Byte> resolver) {

            try {

                // SessionId will be configured in Klf200Socket
                this.CommandOriginator = telegram.ParseEnumParameter<GW_CommandSource>(Klf200TelegramParameter.Source, GW_CommandSource.User);
                this.PriorityLevel = telegram.ParseEnumParameter<GW_CommandPriority>(Klf200TelegramParameter.Priority, GW_CommandPriority.UserLevel1);

                if (Byte.MaxValue.Equals(telegram.Identifier) && !String.IsNullOrEmpty(telegram.Name))
                    telegram.Identifier = resolver.Invoke(Klf200TelegramScope.Scene, telegram.Name);

                this.SceneId = telegram.Identifier;

                this.Velocity = telegram.ParseEnumParameter<GW_NodeVelocity>(Klf200TelegramParameter.Velocity, GW_NodeVelocity.Default);

                return true;

            }
            catch {

                return false;

            }

        }

    }

}
