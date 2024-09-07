using System;
using System.Reflection;

using neleo_com.Logic.Bridges.Velux.Datagrams;

namespace neleo_com.Logic.Bridges.Velux {

    /// <summary>
    ///   This service creates datagrams based on identifiers or reserved names.</summary>
    public static class Klf200DatagramService {

        /// <summary>
        ///   Mapping of the namespace for all datagram implementations.</summary>
        private static readonly String DatagramNamespace = (typeof(GW_ERROR_NTF)).Namespace;

        /// <summary>
        ///   Creates a request command for a given telegram scope.</summary>
        /// <param name="scope">
        ///   The telegram scope.</param>
        /// <param name="action">
        ///   The action (defaults to "default").</param>
        /// <returns>
        ///   A command or <c>null</c>.</returns>
        public static Klf200Datagram CreateRequest(Klf200TelegramScope scope, String action) {

            // ensure 'action' is set properly
            if (String.IsNullOrWhiteSpace(action))
                action = "default";
            else
                action = action.ToLowerInvariant();

            // return null if scope (and/or action) cannot be mapped (see switch statement)
            switch (scope) {

                case Klf200TelegramScope.Node:
                    return Klf200DatagramService.Create(Klf200Command.GW_COMMAND_SEND_REQ);

                case Klf200TelegramScope.Group:
                    return Klf200DatagramService.Create(Klf200Command.GW_ACTIVATE_PRODUCTGROUP_REQ);

                case Klf200TelegramScope.Scene:
                    switch (action) {
                        case "start":
                            return Klf200DatagramService.Create(Klf200Command.GW_ACTIVATE_SCENE_REQ);
                        case "stop":
                            return Klf200DatagramService.Create(Klf200Command.GW_STOP_SCENE_REQ);
                        default:
                            return null;
                    };
                default:
                    return null;

            }

        }

        /// <summary>
        ///   Creates the command for a given command id.</summary>
        /// <param name="commandId">
        ///   The command identifier.</param>
        /// <returns>
        ///   A command or <c>null</c>.</returns>
        public static Klf200Datagram Create(UInt16 commandId) {

            // process command identifier if it's valid
            if (Enum.IsDefined(typeof(Klf200Command), commandId))
                return Klf200DatagramService.Create((Klf200Command)commandId);
            else
                return null;

        }

        /// <summary>
        ///   Creates the command for a given command id.</summary>
        /// <param name="commandId">
        ///   The command identifier.</param>
        /// <returns>
        ///   A command or <c>null</c>.</returns>
        public static Klf200Datagram Create(Klf200Command commandId) {

            String datagramName = String.Format("{0}.{1}", DatagramNamespace, commandId);
            Type datagramType = Assembly.GetExecutingAssembly().GetType(datagramName, false);

            if (datagramType != null)
                return (Klf200Datagram)Activator.CreateInstance(datagramType);
            else
                return null;

        }

    }

}
