using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace neleo_com.Logic.Bridges.Velux.Definitions {

    /// <summary>
    ///   Definition of node states (error codes).</summary>
    public enum GW_RunStateError : Byte {

        /// <summary>
        ///   Used to indicate unknown reply.</summary>
        UnknownStatusReply = 0x00,

        /// <summary>
        ///   Indicates no errors detected.</summary>
        CommandCompletedOk = 0x01,

        /// <summary>
        ///   Indicates no communication to node.</summary>
        NoContact = 0x02,

        /// <summary>
        ///   Indicates manually operated by a user.</summary>
        ManuallyOperated = 0x03,

        /// <summary>
        ///   Indicates node has been blocked by an object.</summary>
        Blocked = 0x04,

        /// <summary>
        ///   Indicates the node contains a wrong system key.</summary>
        WrongSystemkey = 0x05,

        /// <summary>
        ///   Indicates the node is locked on this priority level.</summary>
        PriorityLevelLocked = 0x06,

        /// <summary>
        ///   Indicates node has stopped in another position than expected.</summary>
        ReachedWrongPosition = 0x07,

        /// <summary>
        ///   Indicates an error has occurred during execution of command.</summary>
        ErrorDuringExecution = 0x08,

        /// <summary>
        ///   Indicates no movement of the node parameter.</summary>
        NoExecution = 0x09,

        /// <summary>
        ///   Indicates the node is calibrating the parameters.</summary>
        Calibrating = 0x0A,

        /// <summary>
        ///   Indicates the node power consumption is too high.</summary>
        PowerConsumptionTooHigh = 0x0B,

        /// <summary>
        ///   Indicates the node power consumption is too low.</summary>
        PowerConsumptionTooLow = 0x0C,

        /// <summary>
        ///   Indicates door lock errors. (Door open during lock command)</summary>
        LockPositionOpen = 0x0D,

        /// <summary>
        ///   Indicates the target was not reached in time.</summary>
        MotionTimeTooLongCommunicationEnded = 0x0E,

        /// <summary>
        ///   Indicates the node has gone into thermal protection mode.</summary>
        ThermalProtection = 0x0F,

        /// <summary>
        ///   Indicates the node is not currently operational.</summary>
        ProductNotOperational = 0x10,

        /// <summary>
        ///   Indicates the filter needs maintenance.</summary>
        FilterMaintenanceNeeded = 0x11,

        /// <summary>
        ///   Indicates the battery level is low.</summary>
        BatteryLevel = 0x12,

        /// <summary>
        ///   Indicates the node has modified the target value of the command.</summary>
        TargetModified = 0x13,

        /// <summary>
        ///   Indicates this node does not support the mode received.</summary>
        ModeNotImplemented = 0x14,

        /// <summary>
        ///   Indicates the node is unable to move in the right direction.</summary>
        CommandIncompatibleToMovement = 0x15,

        /// <summary>
        ///   Indicates dead bolt is manually locked during unlock command.</summary>
        UserAction = 0x16,

        /// <summary>
        ///   Indicates dead bolt error.</summary>
        DeadBoltError = 0x17,

        /// <summary>
        ///   Indicates the node has gone into automatic cycle mode.</summary>
        AutomaticCycleEngaged = 0x18,

        /// <summary>
        ///   Indicates wrong load on node.</summary>
        WrongLoadConnected = 0x19,

        /// <summary>
        ///   Indicates that node is unable to reach received colour code.</summary>
        ColourNotReachable = 0x1A,

        /// <summary>
        ///   Indicates the node is unable to reach received target position.</summary>
        TargetNotReachable = 0x1B,

        /// <summary>
        ///   Indicates io-protocol has received an invalid index.</summary>
        BadIndexReceived = 0x1C,

        /// <summary>
        ///   Indicates that the command was overruled by a new command.</summary>
        CommandOverruled = 0x1D,

        /// <summary>
        ///   Indicates that the node reported waiting for power.</summary>
        NodeWaitingForPower = 0x1E,

        /// <summary>
        ///   Indicates an unknown error code received. (Hex code is shown on display)</summary>
        InformationCode = 0xDF,

        /// <summary>
        ///   Indicates the parameter was limited by an unknown device. (Same as LIMITATIONByUNKNOWN_DEVICE)</summary>
        ParameterLimited = 0xE0,

        /// <summary>
        ///   Indicates the parameter was limited by local button.</summary>
        LimitationByLocalUser = 0xE1,

        /// <summary>
        ///   Indicates the parameter was limited by a remote control.</summary>
        LimitationByUser = 0xE2,

        /// <summary>
        ///   Indicates the parameter was limited by a rain sensor.</summary>
        LimitationByRain = 0xE3,

        /// <summary>
        ///   Indicates the parameter was limited by a timer.</summary>
        LimitationByTimer = 0xE4,

        /// <summary>
        ///   Indicates the parameter was limited by a power supply.</summary>
        LimitationByUps = 0xE6,

        /// <summary>
        ///   Indicates the parameter was limited by an unknown device. (Same as PARAMETER_LIMITED)</summary>
        LimitationByUnknownDevice = 0xE7,

        /// <summary>
        ///   Indicates the parameter was limited by a standalone automatic controller.</summary>
        LimitationBySaac = 0xEA,

        /// <summary>
        ///   Indicates the parameter was limited by a wind sensor.</summary>
        LimitationByWind = 0xEB,

        /// <summary>
        ///   Indicates the parameter was limited by the node itself.</summary>
        LimitationByMyself = 0xEC,

        /// <summary>
        ///   Indicates the parameter was limited by an automatic cycle.</summary>
        LimitationByAutomaticCycle = 0xED,

        /// <summary>
        ///   Indicates the parameter was limited by an emergency.</summary>
        LimitationByEmergency = 0xEE

    }

}
