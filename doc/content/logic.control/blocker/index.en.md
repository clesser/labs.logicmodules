---
title: Blocker+
pageLevel: module
moduleLogo: /content/logic.control/img/blocker--GiraIconLogicNodesBlocker.png
---

## Description

This module is an extension of the "Blocker" module. It adds an option to send a value when a block is released.


Logic often changes values automatically, and those values should not always reach the target while blocked. But when the block is released, the last value should be sent to the target. This module supports not only sending fixed values on activation/deactivation, but also sending the last received value when the block is lifted.

## Properties

### Inputs


| Name | Port type | Description |
| --- | --- | --- |
| Input | (Value) | Input of the blocker |
| Active | Binary | Controls module behavior: 1 = all telegrams are blocked; 0 = all telegrams are forwarded. Default value = 0 |



### Parameters


| Name | Port type | Description |
| --- | --- | --- |
| Send telegram upon activation | Selection | Defines whether a telegram is sent when the module is activated. *No telegram* = sends nothing. *Defined telegram* = sends a fixed value. Default value = No telegram |
| Telegram value upon activation | (Value) | Defines which telegram value is sent when the module is activated. |
| Send telegram upon deactivation | Selection | Defines whether a telegram is sent when the module is deactivated. *No telegram* = sends nothing. *Defined telegram* = sends a fixed value. *Cached telegram* = sends the last received value. Default value = No telegram |
| Telegram value upon deactivation | (Value) | Defines which telegram value is sent when the module is deactivated. |



### Outputs


| Name | Port type | Description |
| --- | --- | --- |
| Output | (Value) | Output of the blocker |

## Usage

### Example: Simple shutter control


A simple example is shutter control. In principle, shutters run automatically based on brightness (day/night). But sometimes you may want to pause this automation when guests are present (block = on). When guests leave, automation should continue (block = off).


If it is not known in advance whether the shutter should go up or down after releasing the block (guests may stay overnight or not :-) ), fixed values on deactivation are not practical. If the last received value (up/down) is sent, the state defined by the automation is restored.


![Blocker+ - Simple shutter control 1](/content/logic.control/img/blocker--example01.png)
*In this case, the shutter is lowered when night starts and if the automation blocker allows it. If automation is enabled again (e.g., after guests have left), the shutter is set to the desired position defined by automation.*


![Blocker+ - Simple shutter control 2](/content/logic.control/img/blocker--example02.png)
*This flow is similar to the previous one, but additionally monitors a window. If the window is opened, the shutter moves up (send "move up" on activation). When the window is closed, the block is deactivated and the value from automation is sent if needed.*



### Example: Color-temperature tracking


Sometimes color temperatures are centrally defined for multiple luminaires over the day. This automation can be controlled via a blocker. In some cases, however, LED drivers ignore color temperature data while the light is switched off.


To set the currently valid color temperature correctly, the blocker can also be linked to the light's switching state. If the light is off, no information is sent (it would be ignored anyway). If the light is turned on, the last stored color temperature is sent with a delay, and the light is adjusted correctly.


![Blocker+ - Color-temperature tracking](/content/logic.control/img/blocker--example03.png)
*Color-temperature tracking with two blockers. The first blocker prevents automatic color-temperature tracking when not desired. In combination with on/off delay, the second blocker sends the color temperature when the light is switched on. Both dimmers are driven by two sun-position-based timers (sunrise/sunset).*


:::warning
The example uses Sönke Jacobs' DimCurve logic module ([download link on knx-user-forum.de](http://service.knx-user-forum.de/?comm=download&id=20000030)).
:::

## License information

{{moduleLicenseLink path="/en/logic.control#license"}}
- The icon was taken from the "Blocker" component in Gira Project Assistant.
