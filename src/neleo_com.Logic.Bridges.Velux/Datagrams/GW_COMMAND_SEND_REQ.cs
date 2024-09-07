using System;

using neleo_com.Logic.Bridges.Velux.Definitions;

namespace neleo_com.Logic.Bridges.Velux.Datagrams {

    /// <summary>
    ///   When the Velux KLF-200 gateway receives a GW_COMMAND_SEND_REQ frame, 
    ///   it will set a new actuator position in one or more actuators.</summary>
    public sealed class GW_COMMAND_SEND_REQ : Klf200Datagram, IKlf200DatagramTelegramDecoder, IKlf200DatagramSessionRequest {

        /// <summary>
        ///   Initialize the command.</summary>
        public GW_COMMAND_SEND_REQ() : base(Klf200Command.GW_COMMAND_SEND_REQ, 66) { }

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
        ///   <see cref="GW_COMMAND_RUN_STATUS_NTF"/> frame carries the current value of one parameter. 
        ///   The ParameterActive parameter in <see cref="GW_COMMAND_SEND_REQ"/> frame is used to indicate
        ///   which parameter status is requested for. Default let ParameterActive = MainParameter (). </summary>
        public GW_ParameterType ParameterActive {
            get => this.Data.ReadEnum<GW_ParameterType>(4, 1, GW_ParameterType.MainParameter);
            set => this.Data.WriteEnum<GW_ParameterType>(value, 4, 1);
        }

        /// <summary>
        ///   Value for the main parameter (depends on actuator type).</summary>
        public UInt16 MainParameter {
            get => this.Data.ReadUInt16(7);
            set => this.Data.WriteUInt16(value, 7);
        }



        // TODO add functional parameters 1..16



        /// <summary>
        ///   Number of used indexes in 'IndexArray' parameter. 'IndexArrayCount' must be a number 
        ///   from 1 to 20, both included. If 'IndexArrayCount' is below 20 then the last byte(s) of 
        ///   'IndexArray' parameter is ignored.</summary>
        public Byte NodeArrayCount {
            get => this.Data.ReadByte(41);
        }

        /// <summary>
        ///   Byte array indicating nodes in the system table. One byte for each node, each byte in 
        ///   array can have value[0;199]. 'IndexArray' is always 20 bytes long, even if 'IndexArrayCount' 
        ///   parameter is below 20. If for example 'IndexArrayCount' parameter is 5, only first 5 bytes 
        ///   of 'IndexArray' is relevant.</summary>
        public Byte[] NodeArray {

            get => this.Data.ReadBytes(42, this.NodeArrayCount);

            set {

                if (value == null)
                    throw new ArgumentNullException(nameof(value));

                if (value.Length < 1 || value.Length > 20)
                    throw new ArgumentOutOfRangeException(nameof(value));

                this.Data.WriteByte((Byte)value.Length, 41);
                this.Data.WriteBytes(value, 42, 20);

            }

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
                    telegram.Identifier = resolver.Invoke(Klf200TelegramScope.Node, telegram.Name);

                this.NodeArray = new Byte[] { telegram.Identifier };

                switch (telegram.Action.ToLowerInvariant()) {

                    case "min":
                        this.MainParameter = telegram.ParseUInt16Parameter(Klf200TelegramParameter.Target, 0x0000); // 0x0000 = min
                        this.PriorityLevel = telegram.ParseEnumParameter<GW_CommandPriority>(Klf200TelegramParameter.Priority, GW_CommandPriority.UserLevel1);
                        break;

                    case "max":
                        this.MainParameter = telegram.ParseUInt16Parameter(Klf200TelegramParameter.Target, 0xC800); // 0xC800 = max
                        this.PriorityLevel = telegram.ParseEnumParameter<GW_CommandPriority>(Klf200TelegramParameter.Priority, GW_CommandPriority.UserLevel1);
                        break;

                    case "stop":
                        this.MainParameter = telegram.ParseUInt16Parameter(Klf200TelegramParameter.Target, 0xD200); // 0xD200 = freeze
                        this.PriorityLevel = telegram.ParseEnumParameter<GW_CommandPriority>(Klf200TelegramParameter.Priority, GW_CommandPriority.UserLevel1);
                        break;

                    case "start":
                        this.MainParameter = telegram.ParseUInt16Parameter(Klf200TelegramParameter.Target, 0xD300); // 0xD300 = default
                        this.PriorityLevel = telegram.ParseEnumParameter<GW_CommandPriority>(Klf200TelegramParameter.Priority, GW_CommandPriority.UserLevel1);
                        break;

                    default:
                        this.MainParameter = telegram.ParseUInt16Parameter(Klf200TelegramParameter.Target, 0xD400); // 0xD400 = ignore
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
