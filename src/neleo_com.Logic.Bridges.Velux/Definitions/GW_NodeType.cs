using System;

namespace neleo_com.Logic.Bridges.Velux.Definitions {

    /// <summary>
    ///   Definition of all actuator types and sub types.</summary>
    public enum GW_NodeType : UInt16 {

        None = 0x0000,

        InteriorVenetianBlind = 0x0040,

        RollerShutter = 0x0080,

        RollerShutter_AdjustableSlats = 0x0081,

        RollerShutter_WithProjection = 0x0082,

        VerticalExteriorAwning = 0x00CD,

        WindowOpener = 0x0100,

        WindowOpener_WithRainSensor = 0x0101,

        GarageDoorOpener = 0x0140,

        Light = 0x0180,

        Light_SwitchOnOff = 0x01BA,

        GateOpener = 0x01CD,

        DoorLock = 0x0240,

        WindoowLock = 0x0241,

        VerticalInteriorBlinds = 0x0280,

        DualRollerShutter = 0x0340,

        SwitchOnOff = 0x03C0,

        HorizontalAwning = 0x0400,

        ExteriorVenetianBlind = 0x0440,

        LouverBlind = 0x0480,

        CurtainTrack = 0x04C0,

        VentilationPoint = 0x0500,

        VentilationPoint_AirInlet = 0x0501,

        VentilationPoint_AirTransfer = 0x0502,

        VentilationPoint_AirOutlet = 0x0503,

        ExteriorHeating = 0x0540,

        SwingingShutters = 0x0600,

        SwingingShutters_IndependentLeaves = 0x0601

    }

}
