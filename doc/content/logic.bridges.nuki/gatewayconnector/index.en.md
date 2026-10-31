---
title: Bridge
pageLevel: module
moduleLogo: /content/logic.bridges.nuki/img/gatewayconnector--icons8-router-44.png
---

## Description

This module communicates with the Nuki Bridge. Using [Nuki Request Sender](/en/logic.bridges.nuki/gatewayrequest) and [Nuki Response Filter](/en/logic.bridges.nuki/gatewayresponse), actions can be triggered on connected devices and status can be read.


This component provides one input (*Info (cached)*) that can query all connected devices centrally.


To use this module, a Nuki Bridge reachable in the same network is required. For configuration, the Nuki Bridge parameters "token" and IP address are required. Setup and how to read these parameters is documented here:


1. [Determine Nuki Bridge IP address](https://developer.nuki.io/page/nuki-bridge-http-api-1-13/4/#heading--bridge-discovery) --> The Nuki Bridge IP should be static.
2. [Generate access token](https://developer.nuki.io/page/nuki-bridge-http-api-1-13/4/#heading--token) --> Generation must be confirmed on the Nuki Bridge within 30 sec.


**Note**: If only one SmartLock should be controlled, there is a combined module ([Nuki SmartLock](/en/logic.bridges.nuki/smartlock)) that includes all three components (Bridge, Request Sender, Response Filter).

## Properties

### Inputs


| Name | Port type | Description |
| --- | --- | --- |
| Requests | Text | Receives request telegrams according to [telegram syntax](/en/logic.bridges.nuki/doc/syntax). |
| Info (cached) | Binary | Queries the cached status of all devices connected to the bridge at once. This has a positive effect on battery life of devices. |



### Parameters


| Name | Port type | Description |
| --- | --- | --- |
| Bridge IP | Text | Defines the IPv4 address of the Nuki Bridge (e.g., "0.0.0.0"). The logic module internally appends port "8080". |
| Bridge Token | Text | Access token input. |



### Outputs


| Name | Port type | Description |
| --- | --- | --- |
| Responses | Text | Sends response telegrams according to [telegram syntax](/en/logic.bridges.nuki/doc/syntax). |
| Error | Text | Provides network errors from communication with Nuki Bridge. |

## Usage

### Example: Door lock


Control of an apartment door using Nuki modules can look like this:


![Nuki Bridge logic module](/content/logic.bridges.nuki/img/gatewayconnector--example01.png)
*Application example of Nuki logic modules.*


In the example, one SmartLock is implemented with all three components. The following example shows two SmartLocks (e.g., front door, basement door). Both SmartLocks are connected to the same bridge.


![Nuki Bridge logic modules for two SmartLocks](/content/logic.bridges.nuki/img/gatewayconnector--example02.png)
*Application example of Nuki logic modules for two SmartLocks.*


:::warning
Since Nuki Bridge cannot process multiple requests simultaneously or in very quick succession, it is recommended to delay incoming telegrams step by step (module: telegram delay; about 2–3 sec between two devices) when they should be switched at the same time (e.g., "Lock all doors"). Nuki modules themselves do not delay messages, because otherwise no functional prioritization of individual devices/commands would be possible.
:::

## License information

- {{sectionLink path="/en/logic.bridges.nuki" section="license" label="License terms and legal notice for this module"}}
- [Router](https://icons8.com/icon/0ruxkz6Qtla5/router) icon by [Icons8](https://icons8.com)
