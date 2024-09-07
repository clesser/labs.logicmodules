using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;

using neleo_com.Logic.Bridges.Velux.Definitions;

namespace neleo_com.Logic.Bridges.Velux.Datagrams {

    /// <summary>
    ///   This event holds the information on a modified group.</summary>
    public sealed class GW_GROUP_INFORMATION_CHANGED_NTF : Klf200Datagram, IKlf200DatagramTelegramEncoder {

        /// <summary>
        ///   Initialize the command.</summary>
        public GW_GROUP_INFORMATION_CHANGED_NTF() : base(Klf200Command.GW_GROUP_INFORMATION_CHANGED_NTF, 69) { }

        /// <summary>
        ///   Indicates that a group has been deleted.</summary>
        public Boolean IsDeleted {
            get => this.Data.ReadByte(0) == 0;
        }

        /// <summary>
        ///   Indicates that the group information has been modified.</summary>
        public Boolean IsModified {
            get => this.Data.ReadByte(0) == 1;
        }

        /// <summary>
        ///   GroupID is the group index in the system table. It must be a value from 0 to 99.</summary>
        public Byte GroupId {
            get => this.Data.ReadByte(1);
        }

        /// <summary>
        ///   Order can be used to store a sort order. The sort order is used in client end, 
        ///   when presenting a list of nodes for the user.</summary>
        public UInt16 Order {
            get => this.Data.ReadUInt16(2);
        }

        /// <summary>
        ///   Placement can be used to store a room group index or house group index number.</summary>
        public Byte Placement {
            get => this.Data.ReadByte(4);
        }

        /// <summary>
        ///   This field Name holds the name of the actuator, ex. “Window 1”./// </summary>
        public String Name {
            get => this.Data.ReadString(6, 64);
        }

        /// <summary>
        ///   This field indicates what velocity the group's nodes are operating with.</summary>
        public GW_NodeVelocity Velocity {
            get => this.Data.ReadEnum<GW_NodeVelocity>(69, 1, GW_NodeVelocity.Undefined);
        }

        /// <summary>
        ///   The node variation.</summary>
        public GW_NodeVariation NodeVariation {
            get => this.Data.ReadEnum<GW_NodeVariation>(70, 1, GW_NodeVariation.NotSet);
        }

        /// <summary>
        ///   The group type.</summary>
        public GW_GroupType GroupType {
            get => this.Data.ReadEnum<GW_GroupType>(71, 1, GW_GroupType.User);
        }

        /// <summary>
        ///   This field indicates the number of objects the group contains.</summary>
        public Byte NodesCount {
            get => this.Data.ReadByte(72);
        }

        /// <summary>
        ///   Array (0..199) that indicates (true/false) if a node belongs to a group.</summary>
        public BitArray NodesMap {
            get => this.Data.ReadBits(73, 25);
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
                Scope = Klf200TelegramScope.Group,
                Identifier = this.GroupId,
                Name = this.Name
            };

            ICollection<Byte> nodes = new Collection<Byte>();
            var nodesMap = this.NodesMap;
            for (Byte i = 0; i < NodesMap.Length; i++)
                if (nodesMap[i])
                    nodes.Add(i);

            telegram.SetParameter(Klf200TelegramParameter.Type, this.GroupType);
            telegram.SetParameter(Klf200TelegramParameter.Nodes, String.Join("-", nodes));

            return telegram;

        }

    }

}
