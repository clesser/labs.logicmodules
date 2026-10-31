---
title: Help
pageLevel: module
moduleLogo: /content/logic.bridges.velux/img/gatewayrequest--icons8-input-44.png
---

## Description

This module simulates control inputs of a device (Node), a device group (Group), or scene call (Scene). It converts control signals into telegrams that Velux KLF 200 Gateway can process.


A URL-like [telegram syntax](/en/logic.bridges.velux/doc/syntax) is used for telegrams.

## Properties

### Inputs


| Name | Port type | Scopes | Description |
| --- | --- | --- | --- |
| Target Value | Integer | NodeGroup | Receives target value for a device or group according to configured input range. |
| Target Value (%) | Percent | NodeGroup | Receives target value for a device or group according to configured input range. |
| Min / Max | Binary | NodeGroup | Instructs device/group to set maximum (1 = 100%) or minimum (0 = 0%) value for standard parameter. For shutters, 1 fully closes, 0 fully opens. |
| Stop / Start | Binary | NodeGroupScene | Starts (1) or interrupts (0) a scene. For devices/groups, (1) sets default value and (0) aborts a value change. For shutters, (1) usually has no action, while (0) stops motor movement. |
| Up / Down (Long) | Binary | NodeGroup | Instructs device/group to set maximum (1 = 100%) or minimum (0 = 0%) value for standard parameter. For shutters, 1 fully closes, 0 fully opens. |
| Up / Down (Short) | Binary | NodeGroup | Value changes happen in configured step size (e.g., 10%). For shutters, 1 moves down by configured step size, 0 moves up by step size. If a value change is already running (e.g., moving shutter), "Up/Down (Short)" is interpreted as "Stop" for duration configured in "Up/Down (Timespan)". |



### Parameters


| Name | Port type | Scopes | Description |
| --- | --- | --- | --- |
| Command Type | Selection | NodeGroupScene | Selects whether request sender represents a device, a device group, or a scene. |
| Input Value Range | Selection | NodeGroup | Defines how "Target Value" is interpreted:*Default (0..51200)* expects raw values,*Percent (0..100)* expects percent values,*Percent (100..0)* expects percent values and inverts internally. Default value: *Percent (0..100)*. |
| Up / Down (Timespan) | Timespan | NodeGroup | Defines duration of a full value change (e.g., full shutter up run). During this duration, "Up / Down (Short)" commands act as "Stop". After that duration, short commands act as step commands again. Also, repeated sending of identical value is blocked during this duration (different values can always be sent). After the duration ends, identical values can be sent again. For shutters, configure full up run time (+5–10% buffer). |
| Up / Down (Short %) | Integer | NodeGroup | Defines step size for value change via "Up / Down (Short)" input in range 0..100%. |
| Name or :Identifier | Text | NodeGroup | Name or identifier of device, group, or scene. Identifier can be prefixed with "/". Internal numeric identifier can also be used if prefixed with ":" (e.g., ":3"). |
| Execution Velocity | Selection | NodeGroupScene | Some devices support different execution speeds. Select between:*Default* normal speed,*Silent* reduced speed,*Fast* increased speed. Default value: *Default* |
| Command Source | Selection | NodeGroupScene | Defines telegram source. Default value *User - Prio 2* is used for human interactions. Value *Comfort - Prio 2* is used for automation. |
| Command Priority | Selection | NodeGroupScene | Defines telegram priority. Default *Human* is used for human interactions. *Logic / Automation* is used for automatic control. |



### Outputs


| Name | Port type | Description |
| --- | --- | --- |
| Requests | Text | Sends request telegrams according to [telegram syntax](/en/logic.bridges.velux/doc/syntax). |

## Usage

Usually one gateway module is placed on a logic sheet, surrounded by various request senders (shutter: kitchen, shutter: dining room, scene: vacation) and evaluation modules (shutter: kitchen, shutter: dining room).


![Overview of Velux KLF-200 logic components](/content/logic.bridges.velux/img/gatewayconnector--example01.png)
*One gateway logic module connected to request senders and response filters.*


Logic sheets can be organized by function. For example, shutter controls can be created per shutter or per group. The [Velux Gateway](/en/logic.bridges.velux/gatewayconnector) page shows the distributed architecture used in the following example.


![Request sender module for group control](/content/logic.bridges.velux/img/gatewayrequest--example01.png)
*In this example, request sender modules are used to control shutters of two window fronts as one group. Note: a group can only be controlled as a whole - position feedback is still provided per individual device.*


More practical examples are collected on the [Setup notes](/en/logic.bridges.velux/doc/setup) page.

## License information

{{moduleLicenseLink path="/en/logic.bridges.velux#license"}}
- [Input](https://icons8.com/icon/82745/input) icon by [Icons8](https://icons8.com)
