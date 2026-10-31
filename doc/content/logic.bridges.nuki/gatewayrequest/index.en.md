---
title: Request Sender
pageLevel: module
moduleLogo: /content/logic.bridges.nuki/img/gatewayrequest--icons8-input-44.png
---

## Description

This module simulates the input side of a Nuki device and communicates with the [Nuki Bridge](/en/logic.bridges.nuki/gatewayconnector). Device status is then provided by the [Nuki Response Filter](/en/logic.bridges.nuki/gatewayresponse).


To use this module, a Nuki Bridge reachable in the same network is required. For configuration, parameters "nukiID" and "deviceType" of the managed device are required. How to read these parameters is documented here:


[Determine Nuki SmartLock device number](https://developer.nuki.io/page/nuki-bridge-http-api-1-13/4/#heading--list) --> If multiple devices are listed, the number of Nuki SmartLock (deviceType = 0 or 4) is relevant.


**Note**: If only one SmartLock should be controlled, there is a combined module ([Nuki SmartLock](/en/logic.bridges.nuki/smartlock)) that includes all three components (Bridge, Request Sender, Response Filter).

## Properties

### Inputs


| Name | Port type | Description |
| --- | --- | --- |
| Lock | Binary | Requests door lock when value 1 is sent. |
| Unlock | Binary | Requests door unlock when value 1 is sent. Note: depending on SmartLock configuration, "Unlock" may rotate the key as far as "Unlatch" does. |
| Unlatch | Binary | Requests unlatching of the door bolt when value 1 is sent. If door was locked, it is unlocked first. |
| Info (direct) | Binary | Requests SmartLock and door sensor status directly from the device. This has a negative effect on battery life. |



### Parameters


| Name | Port type | Description |
| --- | --- | --- |
| Device Identifier | Text | Defines which device receives commands (lock, unlock, unlatch, status update). |
| Device Type | Integer | Nuki device type (see [Device Types](https://developer.nuki.io/page/nuki-bridge-http-api-1-13/4/#heading--device-types)). |



### Outputs


| Name | Port type | Description |
| --- | --- | --- |
| Requests | Text | Sends request telegrams according to [telegram syntax](/en/logic.bridges.nuki/doc/syntax). |

## Usage

### Example: Door lock


Control of an apartment door using Nuki modules can look like this:


![Nuki Bridge logic module](/content/logic.bridges.nuki/img/gatewayconnector--example01.png)
*Application example of Nuki logic modules.*


More details are summarized on [this page](/en/logic.bridges.nuki/gatewayconnector).

## License information

- {{sectionLink path="/en/logic.bridges.nuki" section="license" label="License terms and legal notice for this module"}}
- [Input](https://icons8.com/icon/82745/input) icon by [Icons8](https://icons8.com)
