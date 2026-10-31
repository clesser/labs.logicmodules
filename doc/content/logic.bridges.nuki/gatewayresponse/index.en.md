---
title: Response Filter
pageLevel: module
moduleLogo: /content/logic.bridges.nuki/img/gatewayresponse--icons8-output-44.png
---

## Description

This module simulates the output side of a Nuki device and communicates with the [Nuki Bridge](/en/logic.bridges.nuki/gatewayconnector). Device status is provided via the [Nuki Response Filter](/en/logic.bridges.nuki/gatewayresponse).


To use this module, a Nuki Bridge reachable in the same network is required. For configuration, parameters "nukiID" and "deviceType" of the managed device are required. How to read these parameters is documented here:


[Determine Nuki SmartLock device number](https://developer.nuki.io/page/nuki-bridge-http-api-1-13/4/#heading--list) --> If multiple devices are listed, the number of Nuki SmartLock (deviceType = 0 or 4) is relevant.


**Note**: If only one SmartLock should be controlled, there is a combined module ([Nuki SmartLock](/en/logic.bridges.nuki/smartlock)) that includes all three components (Bridge, Request Sender, Response Filter).

## Properties

### Inputs


| Name | Port type | Description |
| --- | --- | --- |
| Responses | Text | Receives response telegrams according to [telegram syntax](/en/logic.bridges.nuki/doc/syntax). |



### Parameters


| Name | Port type | Description |
| --- | --- | --- |
| Device Identifier | Text | Defines the device identifier to filter for. |
| Device Type | Integer | Nuki device type (see [Device Types](https://developer.nuki.io/page/nuki-bridge-http-api-1-13/4/#heading--device-types)). |



### Outputs


| Name | Port type | Description |
| --- | --- | --- |
| Lock State | Integer | Returns Nuki SmartLock lock state (see [Lock State](https://developer.nuki.io/page/nuki-bridge-http-api-1-13/4#heading--lock-states)). |
| Door State | Integer | Returns Nuki DoorSensor state (see [DoorSensor State](https://developer.nuki.io/page/nuki-bridge-http-api-1-13/4#heading--doorsensor-states)). |
| Battery State | Percent | Returns Nuki SmartLock battery level (0..100%). |

## Usage

### Example: Door lock


Control of an apartment door using Nuki modules can look like this:


![Nuki Bridge logic module](/content/logic.bridges.nuki/img/gatewayconnector--example01.png)
*Application example of Nuki logic modules.*


More details are summarized on [this page](/en/logic.bridges.nuki/gatewayconnector).

## License information

- {{sectionLink path="/en/logic.bridges.nuki" section="license" label="License terms and legal notice for this module"}}
- [Output](https://icons8.com/icon/90318/output) icon by [Icons8](https://icons8.com)
