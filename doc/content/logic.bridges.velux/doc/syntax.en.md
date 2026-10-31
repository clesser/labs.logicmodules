---
title: [neleo.labs] X1 Velux KLF modules
---

## Description

Communication between logic modules uses telegrams that follow a URL-like syntax:


- request://\{scene|group|node\}\{:id|/name\}?param1=value1&param2=value2#action
- response://\{scene|group|node\}\{:id|/name\}?param1=value1
- info://\{scene|group|node\}


[Velux Request Sender](/en/logic.bridges.velux/gatewayrequest) modules generate "request://" telegrams that are sent to input of [Velux Gateway](/en/logic.bridges.velux/gatewayconnector). It responds with "response://" telegrams and sends them from its output to inputs of [Velux Response Filter](/en/logic.bridges.velux/gatewayresponse) modules ("broadcast").


*scene*, *group*, or *node* defines whether following parameters refer to scenes, groups, or individual devices.


Scenes, groups, or devices can be identified either by internal number (*:id*) or by name (*/name*) (e.g., "request://scene/Aufwachen#start" or "request://node:3?position=25").


Then follows the optional parameter list, starting with question mark ("?") and first key-value pair. Additional key-value pairs are appended with ampersand ("&").


Quick commands can also be defined. They are introduced by hash ("#action"). For example, stop commands can be sent (e.g., "request://node:2#stop").


Possible key-value pairs and actions depend on request type (scene/group/device) and capabilities (e.g., up/down/stop, on/off). Currently, only setting the standard parameter (target=) is supported.


It is also possible to output lists of all scenes, groups, and devices (number and name). For this, send request "info://" with class ("scene", "group", or "node") to [Velux Gateway](/en/logic.bridges.velux/gatewayconnector) in Gira Project Assistant simulation. Requested information is output via gateway module output "Responses" in format "0=Name1, 1=Name2, ..", where number is internal ID of device/group/scene.


In most cases, manually creating telegrams is not required - but this allows integration of devices not officially tested yet.

## Properties

### Protocol


| Name | Description |
| --- | --- |
| request:// | Telegram addressed to input of [Velux Gateway](/en/logic.bridges.velux/gatewayconnector). Used to set properties in devices/groups or start/stop scenes. |
| response:// | Telegram sent from output of [Velux Gateway](/en/logic.bridges.velux/gatewayconnector). Shows changes of device properties. |
| info:// | Telegram addressed to input of [Velux Gateway](/en/logic.bridges.velux/gatewayconnector). Used to fetch list of devices (nodes), groups, or scenes. List is sent to output "Responses" of gateway logic module and intended to simplify setup. Alternatively, [KLF-200 Tester](/en/logic.bridges.velux/doc/tester) can read lists of devices/groups/scenes. |



### Scope


| Name | Description |
| --- | --- |
| node | Indicates that following parameters apply to a single device (node). |
| group | Indicates that following parameters apply to a group of devices. |
| scene | Indicates that following actions apply to a scene. |



### Identifiers


| Name | Protocols | Scopes | Description |
| --- | --- | --- | --- |
| :id | request | node{{br}}group{{br}}scene | Identifier of a device, group, or scene in form of internal index in Velux KLF-200 Gateway. Supported ranges: devices 0..199, groups 0..99, scenes 0..31. Note: device/group/scene number can only be used in "request://" telegrams / request sender. In "response://" telegrams / response filter, names are always used (see next row). |
| /name | requestresponse | node{{br}}group{{br}}scene | Identifier of a device, group, or scene by name assigned in Velux KLF-200 Gateway (e.g., "Esszimmer"). Max. length is 64 characters. |



### Actions


| Name | Protocols | Scopes | Description |
| --- | --- | --- | --- |
| #start | request | node{{br}}group{{br}}scene | Sets standard parameter (e.g., position for shutters) of a device/group to default value ("0xD300"). This value is not defined for all devices and may be ignored. Starts scene execution. |
| #stop | request | node{{br}}group{{br}}scene | Sets standard parameter (e.g., position for shutters) of a device/group to current value ("0xD200"). This stops running actions of a device/group. If target value is already reached, telegram is ignored. Stops a running scene. If scene already ended, telegram is ignored. |
| #min | request | node{{br}}group | Sets standard parameter (e.g., position for shutters) of a device/group to minimum value ("0x000" = fully up for shutters). See chapter 14.1 of Velux KLF-200 documentation. |
| #max | request | node{{br}}group | Sets standard parameter (e.g., position for shutters) of a device/group to maximum value ("0xC800" = fully down for shutters). See chapter 14.1 of Velux KLF-200 documentation. |



### Parameters


| Name | Protocols | Scopes | Actions | Description |
| --- | --- | --- | --- | --- |
| source | request | node{{br}}group{{br}}scene | minmaxstartstop | Defines who initiated telegram: *User* *Rain* *Timer* *UPS* *SAAC* *Wind* *LoadShedding* *LocalLight* *UnspecificEnvironmentSensor* *Emergency*. Default: *User* |
| priority | request | node{{br}}group{{br}}scene | minmaxstartstop | Defines priority handling: *HumanProtection* *EnvironmentProtection* *UserLevel1* *UserLevel2* *ComfortLevel1* *ComfortLevel2* *ComfortLevel3* *ComfortLevel4*. Default: *UserLevel2*, for stop telegrams: *UserLevel1* |
| velocity | request | node{{br}}group{{br}}scene | minmaxstart | Defines execution speed where supported: *Default* *Silent* *Fast*. Default: *Default* |
| target | request | node{{br}}group | (overwritten by #start, #stop, #min, #max) | Target value can be defined in different ways: *Absolute target value* 0x0000 = 0 = 0%, 0xC800 = 51200 = 100%. *Relative target value* 0xC900 = 51456 = -100%, 0xCCE8 = 52456 = +/-0%, 0xD0D0 = 53456 = +100%. *Default* equals #start. *Current* equals #stop. Absolute and relative values must be provided in decimal notation. See chapter 13 of Velux KLF-200 documentation. |
| target | response | node | - | Absolute target value (0x000 .. 0xC800 / 0..51200) of a device. If relative target value (+/-%) was set, gateway returns undefined value (0xF7FF = 63487). Values are always decimal. For shutters, this corresponds to requested position. |
| current | response | node | - | Absolute value (0x000 .. 0xC800 / 0..51200) of standard parameter of a device. Values are always decimal. For shutters, this corresponds to current position. |
| countdown | response | node | - | Time in seconds required by device to reach target value of standard parameter. For shutters, this is travel time from current to target position. |
| state | response | node | - | Defines execution state: *NonExecuting* *ErrorWhileExecution* *NotUsed* *WaitingForPower* *Executing* *Done* *StateUnknown*. Typical values: *NotUsed*, *Executing*, *Done* |

## Usage

The following examples assume a shutter named "Esszimmer" with internal gateway number "1":


request://node:1#min ► Moves shutter up to end position (internally used defaults: source=User, priority=UserLevel2, velocity=Default).


request://node:1?priority=UserLevel1#min ► Moves shutter up to end position with higher priority (internally used defaults: source=User, velocity=Default).


request://node/Esszimmer?target=25600 ► Moves shutter to 50% (50% of 51200 equals 25600; internally used defaults: source=User, priority=UserLevel2, velocity=Default). The 50% target value refers to travel time, not physical shutter height. Some calibration per shutter is usually needed to find correct "50%".
