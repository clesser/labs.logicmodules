using System;

namespace neleo_com.Logic.Bridges.Velux.Definitions {

    /// <summary>
    ///   PriorityLevel defines the priority level, of the activating command. 
    ///   The 8 priority levels are divided into 3 different groups: Protection(PL0-1), 
    ///   User(PL2-3) and Comfort(PL4-7). Typically, PriorityLevel will be set to ‘3’ 
    ///   for user level 2 or ‘5’ for Comfort Level 2.</summary>
    public enum GW_CommandPriority : Byte {

        /// <summary>
        ///   Provide the most secured level. Since consequences of misusing this level can 
        ///   deeply impact the system behaviour, and therefore the io-homecontrol image, 
        ///   it is mandatory for the manufacturer that wants to use this level of priority 
        ///   to receive an agreement from io-homecontrol® In any case the reception of such 
        ///   a command will disable all categories (Level 0 to 7).</summary>
        HumanProtection = 0,

        /// <summary>
        ///   Used by local sensors that are relative to goods protection: endproduct protection,
        ///   house goods protection. 
        ///   Examples: wind sensor on a terrace awning, rain sensor on a roof window, etc.</summary>
        EnvironmentProtection = 1,

        /// <summary>
        ///   Used by controller to send one (or a set of one shot) immediate action commands
        ///   when user manually requested for this. Controllers prescribed as having a higher 
        ///   level of priority than others use this level.
        ///   For example, this level can be used in combination with a lock command on other 
        ///   levels of priority, for providing an exclusive access to actuators control. e.g 
        ///   Parents/Children different access rights, … </summary>
        UserLevel1 = 2,

        /// <summary>
        ///   Used by controller to send one (or a set of one shot) immediate 
        ///   action commands when user manually requested for this. 
        ///   This level is the default level used by controllers. </summary>
        UserLevel2 = 3,

        /// <summary>
        ///   Not used.</summary>
        ComfortLevel1 = 4,

        /// <summary>
        ///   For <see cref="GW_CommandSource.SAAC"/> commands.</summary>
        ComfortLevel2 = 5,

        /// <summary>
        ///   Not used.</summary>
        ComfortLevel3 = 6,

        /// <summary>
        ///   Not used.</summary>
        ComfortLevel4 = 7

    }

}
