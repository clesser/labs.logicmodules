using System;

namespace neleo_com.Logic.Bridges.Velux {

    ///   <summary>
    ///     Collection of all command identifiers.</summary>
    public enum Klf200Command : UInt16 {

        /// <summary>
        ///   Provides information on what triggered the error.</summary>
        GW_ERROR_NTF = 0x0000,

        /// <summary>
        ///   Request gateway to reboot.</summary>
        GW_REBOOT_REQ = 0x0001,

        /// <summary>
        ///   Acknowledge to GW_REBOOT_REQ command.</summary>
        GW_REBOOT_CFM = 0x0002,

        /// <summary>
        ///   Request gateway to clear system table, scene table 
        ///   and set Ethernet settings to factory default. Gateway will reboot.</summary>
        GW_SET_FACTORY_DEFAULT_REQ = 0x0003,

        /// <summary>
        ///   Acknowledge to GW_SET_FACTORY_DEFAULT_REQ command.</summary>
        GW_SET_FACTORY_DEFAULT_CFM = 0x0004,

        /// <summary>
        ///   Request version information.</summary>
        GW_GET_VERSION_REQ = 0x0008,

        /// <summary>
        ///   Acknowledge to GW_GET_VERSION_REQ command.</summary>
        GW_GET_VERSION_CFM = 0x0009,

        /// <summary>
        ///   Request KLF 200 API protocol version.</summary>
        GW_GET_PROTOCOL_VERSION_REQ = 0x000A,

        /// <summary>
        ///   Acknowledge to GW_GET_PROTOCOL_VERSION_REQ command.</summary>
        GW_GET_PROTOCOL_VERSION_CFM = 0x000B,

        /// <summary>
        ///   Request the state of the gateway</summary>
        GW_GET_STATE_REQ = 0x000C,

        /// <summary>
        ///   Acknowledge to GW_GET_STATE_REQ command.</summary>
        GW_GET_STATE_CFM = 0x000D,

        /// <summary>
        ///   Request gateway to leave learn state.</summary>
        GW_LEAVE_LEARN_STATE_REQ = 0x000E,

        /// <summary>
        ///   Acknowledge to GW_LEAVE_LEARN_STATE_REQ command.</summary>
        GW_LEAVE_LEARN_STATE_CFM = 0x000F,

        /// <summary>
        ///   Request network parameters.</summary>
        GW_GET_NETWORK_SETUP_REQ = 0x00E0,

        /// <summary>
        ///   Acknowledge to GW_GET_NETWORK_SETUP_REQ.</summary>
        GW_GET_NETWORK_SETUP_CFM = 0x00E1,

        /// <summary>
        ///   Set network parameters.</summary>
        GW_SET_NETWORK_SETUP_REQ = 0x00E2,

        /// <summary>
        ///   Acknowledge to GW_SET_NETWORK_SETUP_REQ.</summary>
        GW_SET_NETWORK_SETUP_CFM = 0x00E3,

        /// <summary>
        ///   Request a list of nodes in the gateways system table.</summary>
        GW_CS_GET_SYSTEMTABLE_DATA_REQ = 0x0100,

        /// <summary>
        ///   Acknowledge to GW_CS_GET_SYSTEMTABLE_DATA_REQ</summary>
        GW_CS_GET_SYSTEMTABLE_DATA_CFM = 0x0101,

        /// <summary>
        ///   Acknowledge to GW_CS_GET_SYSTEM_TABLE_DATA_REQList of nodes in the gateways systemtable.</summary>
        GW_CS_GET_SYSTEMTABLE_DATA_NTF = 0x0102,

        /// <summary>
        ///   Start CS DiscoverNodes macro in KLF200.</summary>
        GW_CS_DISCOVER_NODES_REQ = 0x0103,

        /// <summary>
        ///   Acknowledge to GW_CS_DISCOVER_NODES_REQ command.</summary>
        GW_CS_DISCOVER_NODES_CFM = 0x0104,

        /// <summary>
        ///   Acknowledge to GW_CS_DISCOVER_NODES_REQ command.</summary>
        GW_CS_DISCOVER_NODES_NTF = 0x0105,

        /// <summary>
        ///   Remove one or more nodes in the systemtable.</summary>
        GW_CS_REMOVE_NODES_REQ = 0x0106,

        /// <summary>
        ///   Acknowledge to GW_CS_REMOVE_NODES_REQ.</summary>
        GW_CS_REMOVE_NODES_CFM = 0x0107,

        /// <summary>
        ///   Clear systemtable and delete system key.</summary>
        GW_CS_VIRGIN_STATE_REQ = 0x0108,

        /// <summary>
        ///   Acknowledge to GW_CS_VIRGIN_STATE_REQ.</summary>
        GW_CS_VIRGIN_STATE_CFM = 0x0109,

        /// <summary>
        ///   Setup KLF200 to get or give a system to or from another io-homecontrol® remote control.
        ///   By a system means all nodes in the systemtable and the system key.</summary>
        GW_CS_CONTROLLER_COPY_REQ = 0x010A,

        /// <summary>
        ///   Acknowledge to GW_CS_CONTROLLER_COPY_REQ.</summary>
        GW_CS_CONTROLLER_COPY_CFM = 0x010B,

        /// <summary>
        ///   Acknowledge to GW_CS_CONTROLLER_COPY_REQ.</summary>
        GW_CS_CONTROLLER_COPY_NTF = 0x010C,

        /// <summary>
        ///   Cancellation of system copy to other controllers.</summary>
        GW_CS_CONTROLLER_COPY_CANCEL_NTF = 0x010D,

        /// <summary>
        ///   Receive system key from another controller.</summary>
        GW_CS_RECEIVE_KEY_REQ = 0x010E,

        /// <summary>
        ///   Acknowledge to GW_CS_RECEIVE_KEY_REQ.</summary>
        GW_CS_RECEIVE_KEY_CFM = 0x010F,

        /// <summary>
        ///   Acknowledge to GW_CS_RECEIVE_KEY_REQ with status.</summary>
        GW_CS_RECEIVE_KEY_NTF = 0x0110,

        /// <summary>
        ///   Information on Product Generic Configuration job initiated 
        ///   by press on PGC button.</summary>
        GW_CS_PGC_JOB_NTF = 0x0111,

        /// <summary>
        ///   Broadcasted to all clients and gives information about 
        ///   added and removed actuator nodes in system table.</summary>
        GW_CS_SYSTEM_TABLE_UPDATE_NTF = 0x0112,

        /// <summary>
        ///   Generate new system key and update actuators in systemtable.</summary>
        GW_CS_GENERATE_NEW_KEY_REQ = 0x0113,

        /// <summary>
        ///   Acknowledge to GW_CS_GENERATE_NEW_KEY_REQ.</summary>
        GW_CS_GENERATE_NEW_KEY_CFM = 0x0114,

        /// <summary>
        ///   Acknowledge to GW_CS_GENERATE_NEW_KEY_REQ with status.</summary>
        GW_CS_GENERATE_NEW_KEY_NTF = 0x0115,

        /// <summary>
        ///   Update key in actuators holding an old key.</summary>
        GW_CS_REPAIR_KEY_REQ = 0x0116,

        /// <summary>
        ///   Acknowledge to GW_CS_REPAIR_KEY_REQ.</summary>
        GW_CS_REPAIR_KEY_CFM = 0x0117,

        /// <summary>
        ///   Acknowledge to GW_CS_REPAIR_KEY_REQ with status.</summary>
        GW_CS_REPAIR_KEY_NTF = 0x0118,

        /// <summary>
        ///   Request one or more actuator to open for configuration.</summary>
        GW_CS_ACTIVATE_CONFIGURATION_MODE_REQ = 0x0119,

        /// <summary>
        ///   Acknowledge to GW_CS_ACTIVATE_CONFIGURATION_MODE_REQ.</summary>
        GW_CS_ACTIVATE_CONFIGURATION_MODE_CFM = 0x011A,

        /// <summary>
        ///   Request extended information of one specific actuator node.</summary>
        GW_GET_NODE_INFORMATION_REQ = 0x0200,

        /// <summary>
        ///   Acknowledge to GW_GET_NODE_INFORMATION_REQ.</summary>
        GW_GET_NODE_INFORMATION_CFM = 0x0201,

        /// <summary>
        ///   Acknowledge to GW_GET_NODE_INFORMATION_REQ.</summary>
        GW_GET_NODE_INFORMATION_NTF = 0x0210,

        /// <summary>
        ///   Request extended information of all nodes.</summary>
        GW_GET_ALL_NODES_INFORMATION_REQ = 0x0202,

        /// <summary>
        ///   Acknowledge to GW_GET_ALL_NODES_INFORMATION_REQ</summary>
        GW_GET_ALL_NODES_INFORMATION_CFM = 0x0203,

        /// <summary>
        ///   Acknowledge to GW_GET_ALL_NODES_INFORMATION_REQ. Holds node information</summary>
        GW_GET_ALL_NODES_INFORMATION_NTF = 0x0204,

        /// <summary>
        ///   Acknowledge to GW_GET_ALL_NODES_INFORMATION_REQ. No more nodes.</summary>
        GW_GET_ALL_NODES_INFORMATION_FINISHED_NTF = 0x0205,

        /// <summary>
        ///   Set node variation.</summary>
        GW_SET_NODE_VARIATION_REQ = 0x0206,

        /// <summary>
        ///   Acknowledge to GW_SET_NODE_VARIATION_REQ.</summary>
        GW_SET_NODE_VARIATION_CFM = 0x0207,

        /// <summary>
        ///   Set node name.</summary>
        GW_SET_NODE_NAME_REQ = 0x0208,

        /// <summary>
        ///   Acknowledge to GW_SET_NODE_NAME_REQ.</summary>
        GW_SET_NODE_NAME_CFM = 0x0209,

        /// <summary>
        ///   Set node velocity.</summary>
        GW_SET_NODE_VELOCITY_REQ = 0x020A,

        /// <summary>
        ///   Acknowledge to GW_SET_NODE_VELOCITY_REQ.</summary>
        GW_SET_NODE_VELOCITY_CFM = 0x020B,

        /// <summary>
        ///   Information has been updated.</summary>
        GW_NODE_INFORMATION_CHANGED_NTF = 0x020C,

        /// <summary>
        ///   Information has been updated.</summary>
        GW_NODE_STATE_POSITION_CHANGED_NTF = 0x0211,

        /// <summary>
        ///   Set search order and room placement.</summary>
        GW_SET_NODE_ORDER_AND_PLACEMENT_REQ = 0x020D,

        /// <summary>
        ///   Acknowledge to GW_SET_NODE_ORDER_AND_PLACEMENT_REQ.</summary>
        GW_SET_NODE_ORDER_AND_PLACEMENT_CFM = 0x020E,

        /// <summary>
        ///   Request information about all defined groups.</summary>
        GW_GET_GROUP_INFORMATION_REQ = 0x0220,

        /// <summary>
        ///   Acknowledge to GW_GET_GROUP_INFORMATION_REQ.</summary>
        GW_GET_GROUP_INFORMATION_CFM = 0x0221,

        /// <summary>
        ///   Acknowledge to GW_GET_NODE_INFORMATION_REQ.</summary>
        GW_GET_GROUP_INFORMATION_NTF = 0x0230,

        /// <summary>
        ///   Change an existing group.</summary>
        GW_SET_GROUP_INFORMATION_REQ = 0x0222,

        /// <summary>
        ///   Acknowledge to GW_SET_GROUP_INFORMATION_REQ.</summary>
        GW_SET_GROUP_INFORMATION_CFM = 0x0223,

        /// <summary>
        ///   Broadcast to all, about group information of a group has been changed.</summary>
        GW_GROUP_INFORMATION_CHANGED_NTF = 0x0224,

        /// <summary>
        ///   Delete a group.</summary>
        GW_DELETE_GROUP_REQ = 0x0225,

        /// <summary>
        ///   Acknowledge to GW_DELETE_GROUP_INFORMATION_REQ.</summary>
        GW_DELETE_GROUP_CFM = 0x0226,

        /// <summary>
        ///   Request new group to be created.</summary>
        GW_NEW_GROUP_REQ = 0x0227,

        /// <summary>
        ///   Acknowledge to GW_NEW_GROUP_REQ.</summary>
        GW_NEW_GROUP_CFM = 0x0228,

        /// <summary>
        ///   Request information about all defined groups.</summary>
        GW_GET_ALL_GROUPS_INFORMATION_REQ = 0x0229,

        /// <summary>
        ///   Acknowledge to GW_GET_ALL_GROUPS_INFORMATION_REQ.</summary>
        GW_GET_ALL_GROUPS_INFORMATION_CFM = 0x022A,

        /// <summary>
        ///   Acknowledge to GW_GET_ALL_GROUPS_INFORMATION_REQ.</summary>
        GW_GET_ALL_GROUPS_INFORMATION_NTF = 0x022B,

        /// <summary>
        ///   Acknowledge to GW_GET_ALL_GROUPS_INFORMATION_REQ.</summary>
        GW_GET_ALL_GROUPS_INFORMATION_FINISHED_NTF = 0x022C,

        /// <summary>
        ///   GW_GROUP_DELETED_NTF is broadcasted to all, when a group has been removed.</summary>
        GW_GROUP_DELETED_NTF = 0x022D,

        /// <summary>
        ///   Enable house status monitor.</summary>
        GW_HOUSE_STATUS_MONITOR_ENABLE_REQ = 0x0240,

        /// <summary>
        ///   Acknowledge to GW_HOUSE_STATUS_MONITOR_ENABLE_REQ.</summary>
        GW_HOUSE_STATUS_MONITOR_ENABLE_CFM = 0x0241,

        /// <summary>
        ///   Disable house status monitor.</summary>
        GW_HOUSE_STATUS_MONITOR_DISABLE_REQ = 0x0242,

        /// <summary>
        ///   Acknowledge to GW_HOUSE_STATUS_MONITOR_DISABLE_REQ.</summary>
        GW_HOUSE_STATUS_MONITOR_DISABLE_CFM = 0x0243,

        /// <summary>
        ///   Send activating command direct to one or more io-homecontrol® nodes.</summary>
        GW_COMMAND_SEND_REQ = 0x0300,

        /// <summary>
        ///   Acknowledge to GW_COMMAND_SEND_REQ.</summary>
        GW_COMMAND_SEND_CFM = 0x0301,

        /// <summary>
        ///   Gives run status for io-homecontrol® node.</summary>
        GW_COMMAND_RUN_STATUS_NTF = 0x0302,

        /// <summary>
        ///   Gives remaining time before io-homecontrol® node enter target position.</summary>
        GW_COMMAND_REMAINING_TIME_NTF = 0x0303,

        /// <summary>
        ///   Command send, Status request, Wink, Mode or Stop session is finished.</summary>
        GW_SESSION_FINISHED_NTF = 0x0304,

        /// <summary>
        ///   Get status request from one or more io-homecontrol® nodes.</summary>
        GW_STATUS_REQUEST_REQ = 0x0305,

        /// <summary>
        ///   Acknowledge to GW_STATUS_REQUEST_REQ.</summary>
        GW_STATUS_REQUEST_CFM = 0x0306,

        /// <summary>
        ///   Acknowledge to GW_STATUS_REQUEST_REQ. 
        ///   Status request from one or more io-homecontrol® nodes.</summary>
        GW_STATUS_REQUEST_NTF = 0x0307,

        /// <summary>
        ///   Request from one or more io-homecontrol® nodes to Wink.</summary>
        GW_WINK_SEND_REQ = 0x0308,

        /// <summary>
        ///   Acknowledge to GW_WINK_SEND_REQ</summary>
        GW_WINK_SEND_CFM = 0x0309,

        /// <summary>
        ///   Status info for performed wink request.</summary>
        GW_WINK_SEND_NTF = 0x030A,

        /// <summary>
        ///   Set a parameter limitation in an actuator.</summary>
        GW_SET_LIMITATION_REQ = 0x0310,

        /// <summary>
        ///   Acknowledge to GW_SET_LIMITATION_REQ.</summary>
        GW_SET_LIMITATION_CFM = 0x0311,

        /// <summary>
        ///   Get parameter limitation in an actuator.</summary>
        GW_GET_LIMITATION_STATUS_REQ = 0x0312,

        /// <summary>
        ///   Acknowledge to GW_GET_LIMITATION_STATUS_REQ.</summary>
        GW_GET_LIMITATION_STATUS_CFM = 0x0313,

        /// <summary>
        ///   Hold information about limitation.</summary>
        GW_LIMITATION_STATUS_NTF = 0x0314,

        /// <summary>
        ///   Send Activate Mode to one or more io-homecontrol® nodes.</summary>
        GW_MODE_SEND_REQ = 0x0320,

        /// <summary>
        ///   Acknowledge to GW_MODE_SEND_REQ</summary>
        GW_MODE_SEND_CFM = 0x0321,

        /// <summary>
        ///   Notify with Mode activation info.</summary>
        GW_MODE_SEND_NTF = 0x0322,

        /// <summary>
        ///   Prepare gateway to record a scene.</summary>
        GW_INITIALIZE_SCENE_REQ = 0x0400,

        /// <summary>
        ///   Acknowledge to GW_INITIALIZE_SCENE_REQ.</summary>
        GW_INITIALIZE_SCENE_CFM = 0x0401,

        /// <summary>
        ///   Acknowledge to GW_INITIALIZE_SCENE_REQ.</summary>
        GW_INITIALIZE_SCENE_NTF = 0x0402,

        /// <summary>
        ///   Cancel record scene process.</summary>
        GW_INITIALIZE_SCENE_CANCEL_REQ = 0x0403,

        /// <summary>
        ///   Acknowledge to GW_INITIALIZE_SCENE_CANCEL_REQ command.</summary>
        GW_INITIALIZE_SCENE_CANCEL_CFM = 0x0404,

        /// <summary>
        ///   Store actuator positions changes since GW_INITIALIZE_SCENE, as a scene.</summary>
        GW_RECORD_SCENE_REQ = 0x0405,

        /// <summary>
        ///   Acknowledge to GW_RECORD_SCENE_REQ.</summary>
        GW_RECORD_SCENE_CFM = 0x0406,

        /// <summary>
        ///   Acknowledge to GW_RECORD_SCENE_REQ.</summary>
        GW_RECORD_SCENE_NTF = 0x0407,

        /// <summary>
        ///   Delete a recorded scene.</summary>
        GW_DELETE_SCENE_REQ = 0x0408,

        /// <summary>
        ///   Acknowledge to GW_DELETE_SCENE_REQ.</summary>
        GW_DELETE_SCENE_CFM = 0x0409,

        /// <summary>
        ///   Request a scene to be renamed.</summary>
        GW_RENAME_SCENE_REQ = 0x040A,

        /// <summary>
        ///   Acknowledge to GW_RENAME_SCENE_REQ.</summary>
        GW_RENAME_SCENE_CFM = 0x040B,

        /// <summary>
        ///   Request a list of scenes.</summary>
        GW_GET_SCENE_LIST_REQ = 0x040C,

        /// <summary>
        ///   Acknowledge to GW_GET_SCENE_LIST.</summary>
        GW_GET_SCENE_LIST_CFM = 0x040D,

        /// <summary>
        ///   Acknowledge to GW_GET_SCENE_LIST.</summary>
        GW_GET_SCENE_LIST_NTF = 0x040E,

        /// <summary>
        ///   Request extended information for one given scene.</summary>
        GW_GET_SCENE_INFORMATION_REQ = 0x040F,

        /// <summary>
        ///   Acknowledge to GW_GET_SCENE_INFOAMATION_REQ.</summary>
        GW_GET_SCENE_INFORMATION_CFM = 0x0410,

        /// <summary>
        ///   Acknowledge to GW_GET_SCENE_INFOAMATION_REQ.</summary>
        GW_GET_SCENE_INFORMATION_NTF = 0x0411,

        /// <summary>
        ///   Request gateway to enter a scene.</summary>
        GW_ACTIVATE_SCENE_REQ = 0x0412,

        /// <summary>
        ///   Acknowledge to GW_ACTIVATE_SCENE_REQ.</summary>
        GW_ACTIVATE_SCENE_CFM = 0x0413,

        /// <summary>
        ///   Request all nodes in a given scene to stop at their current position.</summary>
        GW_STOP_SCENE_REQ = 0x0415,

        /// <summary>
        ///   Acknowledge to GW_STOP_SCENE_REQ.</summary>
        GW_STOP_SCENE_CFM = 0x0416,

        /// <summary>
        ///   A scene has either been changed or removed.</summary>
        GW_SCENE_INFORMATION_CHANGED_NTF = 0x0419,

        /// <summary>
        ///   Activate a product group in a given direction.</summary>
        GW_ACTIVATE_PRODUCTGROUP_REQ = 0x0447,

        /// <summary>
        ///   Acknowledge to GW_ACTIVATE_PRODUCTGROUP_REQ.</summary>
        GW_ACTIVATE_PRODUCTGROUP_CFM = 0x0448,

        /// <summary>
        ///   Acknowledge to GW_ACTIVATE_PRODUCTGROUP_REQ.</summary>
        GW_ACTIVATE_PRODUCTGROUP_NTF = 0x0449,

        /// <summary>
        ///   Get list of assignments to all Contact Input to scene or product group.</summary>
        GW_GET_CONTACT_INPUT_LINK_LIST_REQ = 0x0460,

        /// <summary>
        ///   Acknowledge to GW_GET_CONTACT_INPUT_LINK_LIST_REQ.</summary>
        GW_GET_CONTACT_INPUT_LINK_LIST_CFM = 0x0461,

        /// <summary>
        ///   Set a link from a Contact Input to a scene or product group.</summary>
        GW_SET_CONTACT_INPUT_LINK_REQ = 0x0462,

        /// <summary>
        ///   Acknowledge to GW_SET_CONTACT_INPUT_LINK_REQ.</summary>
        GW_SET_CONTACT_INPUT_LINK_CFM = 0x0463,

        /// <summary>
        ///   Remove a link from a Contact Input to a scene.</summary>
        GW_REMOVE_CONTACT_INPUT_LINK_REQ = 0x0464,

        /// <summary>
        ///   Acknowledge to GW_REMOVE_CONTACT_INPUT_LINK_REQ.</summary>
        GW_REMOVE_CONTACT_INPUT_LINK_CFM = 0x0465,

        /// <summary>
        ///   Request header from activation log.</summary>
        GW_GET_ACTIVATION_LOG_HEADER_REQ = 0x0500,

        /// <summary>
        ///   Confirm header from activation log.</summary>
        GW_GET_ACTIVATION_LOG_HEADER_CFM = 0x0501,

        /// <summary>
        ///   Request clear all data in activation log.</summary>
        GW_CLEAR_ACTIVATION_LOG_REQ = 0x0502,

        /// <summary>
        ///   Confirm clear all data in activation log.</summary>
        GW_CLEAR_ACTIVATION_LOG_CFM = 0x0503,

        /// <summary>
        ///   Request line from activation log.</summary>
        GW_GET_ACTIVATION_LOG_LINE_REQ = 0x0504,

        /// <summary>
        ///   Confirm line from activation log.</summary>
        GW_GET_ACTIVATION_LOG_LINE_CFM = 0x0505,

        /// <summary>
        ///   Confirm line from activation log.</summary>
        GW_ACTIVATION_LOG_UPDATED_NTF = 0x0506,

        /// <summary>
        ///   Request lines from activation log.</summary>
        GW_GET_MULTIPLE_ACTIVATION_LOG_LINES_REQ = 0x0507,

        /// <summary>
        ///   Error log data from activation log.</summary>
        GW_GET_MULTIPLE_ACTIVATION_LOG_LINES_NTF = 0x0508,

        /// <summary>
        ///   Confirm lines from activation log.</summary>
        GW_GET_MULTIPLE_ACTIVATION_LOG_LINES_CFM = 0x0509,

        /// <summary>
        ///   Request to set UTC time.</summary>
        GW_SET_UTC_REQ = 0x2000,

        /// <summary>
        ///   Acknowledge to GW_SET_UTC_REQ.</summary>
        GW_SET_UTC_CFM = 0x2001,

        /// <summary>
        ///   Set time zone and daylight savings rules.</summary>
        GW_RTC_SET_TIME_ZONE_REQ = 0x2002,

        /// <summary>
        ///   Acknowledge to GW_RTC_SET_TIME_ZONE_REQ.</summary>
        GW_RTC_SET_TIME_ZONE_CFM = 0x2003,

        /// <summary>
        ///   Request the local time based on current time zone and daylight savings rules.</summary>
        GW_GET_LOCAL_TIME_REQ = 0x2004,

        /// <summary>
        ///   Acknowledge to GW_RTC_SET_TIME_ZONE_REQ.</summary>
        GW_GET_LOCAL_TIME_CFM = 0x2005,

        /// <summary>
        ///   Enter password to authenticate request</summary>
        GW_PASSWORD_ENTER_REQ = 0x3000,

        /// <summary>
        ///   Acknowledge to GW_PASSWORD_ENTER_REQ</summary>
        GW_PASSWORD_ENTER_CFM = 0x3001,

        /// <summary>
        ///   Request password change.</summary>
        GW_PASSWORD_CHANGE_REQ = 0x3002,

        /// <summary>
        ///   Acknowledge to GW_PASSWORD_CHANGE_REQ.</summary>
        GW_PASSWORD_CHANGE_CFM = 0x3003,

        /// <summary>
        ///   Acknowledge to GW_PASSWORD_CHANGE_REQ. Broadcasted to all connected clients.</summary>
        GW_PASSWORD_CHANGE_NTF = 0x3004

    }

}
