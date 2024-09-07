using System;

using neleo_com.Logic.Bridges.Velux.Definitions;

namespace neleo_com.Logic.Bridges.Velux.Datagrams {

    /// <summary>
    ///   The gateway can handle activation of all actuators in user defined product group.</summary>
    public sealed class GW_ACTIVATE_PRODUCTGROUP_REQ : Klf200Datagram, IKlf200DatagramTelegramDecoder, IKlf200DatagramSessionRequest {

        /// <summary>
        ///   Initialize the command.</summary>
        public GW_ACTIVATE_PRODUCTGROUP_REQ() : base(Klf200Command.GW_ACTIVATE_PRODUCTGROUP_REQ, 13) { }

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
        ///   GroupID is the group index in the system table. It must be a value from 0 to 99.</summary>
        public Byte GroupId {
            get => this.Data.ReadByte(4);
            set => this.Data.WriteByte(value, 4);
        }

        /// <summary>
        ///   <see cref="GW_COMMAND_RUN_STATUS_NTF"/> frame carries the current value of one parameter. 
        ///   The ParameterActive parameter in <see cref="GW_COMMAND_SEND_REQ"/> frame is used to indicate
        ///   which parameter status is requested for. Default let ParameterActive = ParameterValue (). </summary>
        public GW_ParameterType ParameterActive {
            get => this.Data.ReadEnum<GW_ParameterType>(5, 1, GW_ParameterType.MainParameter);
            set => this.Data.WriteEnum<GW_ParameterType>(value, 5, 1);
        }

        /// <summary>
        ///   Value for the parameter (<see cref="ParameterActive"/>).</summary>
        public UInt16 ParameterValue {
            get => this.Data.ReadUInt16(6);
            set => this.Data.WriteUInt16(value, 6);
        }

        /// <summary>
        ///   This field indicates what velocity the nodes are operating with.</summary>
        public GW_NodeVelocity Velocity {
            get => this.Data.ReadEnum<GW_NodeVelocity>(8, 1, GW_NodeVelocity.Undefined);
            set => this.Data.WriteEnum<GW_NodeVelocity>(value, 8, 1);
        }



        // TODO add priority and lock levels



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
                this.ParameterActive = GW_ParameterType.MainParameter;

                if (Byte.MaxValue.Equals(telegram.Identifier) && !String.IsNullOrEmpty(telegram.Name))
                    telegram.Identifier = resolver.Invoke(Klf200TelegramScope.Group, telegram.Name);

                this.GroupId= telegram.Identifier;

                this.Velocity = telegram.ParseEnumParameter<GW_NodeVelocity>(Klf200TelegramParameter.Velocity, GW_NodeVelocity.Default);

                switch (telegram.Action.ToLowerInvariant()) {

                    case "min":
                        this.ParameterValue = telegram.ParseUInt16Parameter(Klf200TelegramParameter.Target, 0x0000); // 0x0000 = min
                        this.PriorityLevel = telegram.ParseEnumParameter<GW_CommandPriority>(Klf200TelegramParameter.Priority, GW_CommandPriority.UserLevel1);
                        break;

                    case "max":
                        this.ParameterValue = telegram.ParseUInt16Parameter(Klf200TelegramParameter.Target, 0xC800); // 0xC800 = max
                        this.PriorityLevel = telegram.ParseEnumParameter<GW_CommandPriority>(Klf200TelegramParameter.Priority, GW_CommandPriority.UserLevel1);
                        break;

                    case "stop":
                        this.ParameterValue = telegram.ParseUInt16Parameter(Klf200TelegramParameter.Target, 0xD200); // 0xD200 = freeze
                        this.PriorityLevel = telegram.ParseEnumParameter<GW_CommandPriority>(Klf200TelegramParameter.Priority, GW_CommandPriority.UserLevel1);
                        break;

                    case "start":
                        this.ParameterValue = telegram.ParseUInt16Parameter(Klf200TelegramParameter.Target, 0xD300); // 0xD300 = default
                        this.PriorityLevel = telegram.ParseEnumParameter<GW_CommandPriority>(Klf200TelegramParameter.Priority, GW_CommandPriority.UserLevel1);
                        break;

                    default:
                        this.ParameterValue = telegram.ParseUInt16Parameter(Klf200TelegramParameter.Target, 0xD400); // 0xD400 = ignore
                        this.PriorityLevel = telegram.ParseEnumParameter<GW_CommandPriority>(Klf200TelegramParameter.Priority, GW_CommandPriority.UserLevel2);
                        break;

                }

                return true;

            }
            catch {

                return false;

            }

        }

    }

}
