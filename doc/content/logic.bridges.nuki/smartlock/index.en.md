---
title: SmartLock
pageLevel: module
moduleLogo: /content/logic.bridges.nuki/img/smartlock--icons8-lock-44.png
---

## Description

This module allows locking, unlocking, and unlatching a door lock via Nuki SmartLock and provides information about lock state, door sensor, and battery level.


To use this module, a Nuki Bridge reachable in the same network is required. For configuration, parameters "token" of Nuki Bridge and "nukiId" (called "Device ID" below) of the Nuki SmartLock are required. Bridge setup and parameter lookup are documented here:


1. [Determine Nuki Bridge IP address](https://developer.nuki.io/page/nuki-bridge-http-api-1-13/4/#heading--bridge-discovery) --> The Nuki Bridge IP should be static.
2. [Generate access token](https://developer.nuki.io/page/nuki-bridge-http-api-1-13/4/#heading--token) --> Generation must be confirmed on the Nuki Bridge within 30 sec.
3. [Determine Nuki SmartLock device number](https://developer.nuki.io/page/nuki-bridge-http-api-1-13/4/#heading--list) --> If multiple devices are listed, the number of Nuki SmartLock (deviceType = 0 or 4) is relevant.


After each action (lock, unlock, unlatch) that the Nuki Bridge confirms as "successful", "Lock State" is updated accordingly without querying the actual hardware state.


In practice, this enables quick feedback (visualization) that may be corrected during the next update cycle. This is especially relevant for unlatching (state = 5), which switches to "unlocked" (state = 3) after lock timeout, but is only reflected in X1 at the next status update.


**Note**: This module is ideal for controlling a **single** SmartLock connected to one bridge. If **multiple** devices are connected to the same bridge, prefer the combination of [Nuki Bridge](/en/logic.bridges.nuki/gatewayconnector), [Nuki Request Sender](/en/logic.bridges.nuki/gatewayrequest), and [Nuki Response Filter](/en/logic.bridges.nuki/gatewayresponse).

## Properties

### Inputs


| Name | Port type | Description |
| --- | --- | --- |
| Lock | Binary | Requests door lock when value 1 is sent. |
| Unlock | Binary | Requests door unlock when value 1 is sent. Note: depending on SmartLock configuration, "Unlock" may rotate the key as far as "Unlatch" does. |
| Unlatch | Binary | Requests unlatching of the door bolt when value 1 is sent. If door was locked, it is unlocked first. |
| Info (direct) | Binary | Requests SmartLock and door sensor status directly from the device. This has a negative effect on battery life. |
| Info (cached) | Binary | Requests SmartLock and door sensor status from bridge cache. This has a positive effect on battery life. |



### Parameters


| Name | Port type | Description |
| --- | --- | --- |
| Bridge IP | Text | Defines the IPv4 address of the Nuki Bridge (e.g., "0.0.0.0"). The logic module internally appends port "8080". |
| Bridge Token | Text | Access token input. |
| Device Identifier | Text | Defines which device receives commands (lock, unlock, unlatch, status update). |
| Device Type | Integer | Nuki device type (see [Device Types](https://developer.nuki.io/page/nuki-bridge-http-api-1-13/4/#heading--device-types)). |



### Outputs


| Name | Port type | Description |
| --- | --- | --- |
| Lock State | Integer | Returns Nuki SmartLock lock state (see [Lock State](https://developer.nuki.io/page/nuki-bridge-http-api-1-13/4#heading--lock-states)). |
| Door State | Integer | Returns Nuki DoorSensor state (see [DoorSensor State](https://developer.nuki.io/page/nuki-bridge-http-api-1-13/4#heading--doorsensor-states)). |
| Battery State | Percent | Returns Nuki SmartLock battery level (0..100%). |
| Error | Text | Provides network errors from communication with Nuki Bridge. |

## Usage

### Example: Door lock


Controlling an apartment door using Nuki SmartLock can look like this:


![Nuki SmartLock logic module](/content/logic.bridges.nuki/img/smartlock--example01.png)
*Application example of Nuki SmartLock logic module.*


The lock is controlled using three inputs, so it can be locked, unlocked, or unlatched e.g. by buttons or scenes. A few recommendations for data-point configuration:


- Group address data points. Two group addresses and two data points are needed. On data point 1, put group address 1 (send) and group address 2 (status); on data point 2, put group address 2 (send). Data point 1 is used to switch Nuki SmartLock (using edge detector/filter), and data point 2 is used behind "Comparer+" to detect state. A switch can be used for visualization.
- Variable data points. It is recommended to create three data points: data point 1 for lock, data point 2 for unlock, and data point 3 for status display after "Comparer+". Visualization can currently only be implemented using on/off buttons and a binary indicator. If the lock is controlled only via scenes (no switch visualization needed), one data point with edge detector/filter (analogous to group-address data points) can be used (see figure).
- For unlatch, any data point visualized with an on/off button can be used.


Telegram generation ensures that status (including battery level) is read from Nuki Bridge regularly. Two inputs are available:


- *Info (direct)* reads status directly from the device. Frequent direct queries can reduce battery/accumulator lifetime.
- *Info (cached)* reads cached status from the bridge. In this case, device battery is preserved.


On output side, Nuki SmartLock states are provided. With "Comparer+", values are evaluated (locked/unlocked/error and open/closed/error). Battery level is passed through directly. A comparer can trigger an alarm at critical battery level.


A classic door sensor detects when the door opens and sets lock state to "Open" (filter module in the last row). This provides the status immediately, even outside an update cycle.

## License information

- {{sectionLink path="/en/logic.bridges.nuki" section="license" label="License terms and legal notice for this module"}}
- [Lock](https://icons8.com/icon/15437/lock) icon by [Icons8](https://icons8.com)
