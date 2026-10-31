---
title: Gateway
pageLevel: module
moduleLogo: /content/logic.bridges.velux/img/gatewayconnector--icons8-router-44.png
---

## Description

This module establishes the connection to the Velux KLF-200 Gateway. It receives request telegrams and forwards them as datagrams to the gateway. Gateway responses are output as response telegrams.


A URL-like [telegram syntax](/en/logic.bridges.velux/doc/syntax) is used for telegrams.

## Properties

### Inputs


| Name | Port type | Description |
| --- | --- | --- |
| Requests | Text | Receives request telegrams according to [telegram syntax](/en/logic.bridges.velux/doc/syntax). |
| Request State | Binary | Requests Velux KLF-200 to query status of all devices. Normally this is **not required** - the module configures KLF-200 internally so it receives and forwards device updates automatically. On restart of X1 or this logic module, status of all devices is also queried automatically (if devices report status back to KLF-200). |
| Request Reconnect | Binary | Establishes a new connection to Velux KLF-200 when "1" is sent. Manual reconnect is usually not required because connection starts automatically with X1 logic module startup. If connection is interrupted, the module attempts reconnect automatically in a seven-minute window. |
| Request Reboot | Binary | Sends request to Velux KLF-200 to reboot itself. The logic module automatically tries to reconnect after one minute. For reboot command to reach KLF-200, an active gateway connection must exist first. |



### Parameters


| Name | Port type | Description |
| --- | --- | --- |
| IPv4 Address | Text | IPv4 address of Velux KLF-200 Gateway (e.g., "192.168.2.100"). Ensure gateway is reachable on port 51200. |
| Password | Text | Password for Velux KLF-200 Gateway. If unchanged, it is the WiFi password printed on the bottom of the gateway. |



### Outputs


| Name | Port type | Description |
| --- | --- | --- |
| Responses | Text | Sends response telegrams according to [telegram syntax](/en/logic.bridges.velux/doc/syntax). |
| State | Text | Indicates connection state of logic module to Velux KLF-200 Gateway. *Disconnecting* state after reboot request or reconnect request. *Disconnected* no communication with KLF-200 Gateway. *Connecting* module starts connection setup. *Connected* module has active connection. Default value: *Disconnected*. Every seven minutes it is checked whether connection is still active. If connection drops (e.g., power/network outage of KLF-200), state may still show *Connected* during that interval before switching to *Disconnected* or before an automatic reconnect attempt starts. If automatic reconnect fails (exactly one attempt), connection must be restored manually. |
| Errors | Text | Reports errors from telegram evaluation or communication with Velux KLF-200 Gateway. |

## Usage

### Concept


Usually one gateway module is placed on a logic sheet, surrounded by various request senders (shutter: kitchen, shutter: dining room, scene: vacation) and evaluation modules (shutter: kitchen, shutter: dining room).


![Overview of Velux KLF-200 logic components](/content/logic.bridges.velux/img/gatewayconnector--example01.png)
*One gateway logic module connected to request senders and response filters.*


Because one Velux KLF-200 Gateway can hold at most two connections, **only one matching gateway logic module should be used per physical gateway**.


For better clarity, telegrams can be generated and evaluated across different logic sheets. Request telegrams are collected via one local variable and forwarded to the gateway. Conversely, outgoing telegrams are distributed via another variable and then filtered on other logic sheets.


![Central Velux KLF-200](/content/logic.bridges.velux/img/doc--example02-03.png)
*In this case, one logic sheet connects the gateway with two internal text variables used to submit requests and distribute responses.*


![Logic sheet with one shutter actuator.](/content/logic.bridges.velux/img/doc--example02-04.png)
*Example of awning control. The response filter extracts telegrams for device "Balkon" and updates "Position" and "Movement"; the request sender receives KNX telegrams and forwards transformed telegrams to the Velux KLF-200 Gateway logic module.*


This also makes it possible to configure different request senders for one and the same device with different priorities (e.g., automation, sensors, user interaction).


![Logic sheet with advanced options to control the logic module.](/content/logic.bridges.velux/img/doc--example04-01.png)
*This logic sheet shows how to request a reconnect or restart of Velux KLF-200. It also shows how to output connection state or signal faults (using [Comparer+](/en/logic.control/switch) in "Text" comparison mode).* 


The page [Setup notes](/en/logic.bridges.velux/doc/setup) provides additional information.

## License information

{{moduleLicenseLink path="/en/logic.bridges.velux#license"}}
- [Router](https://icons8.com/icon/0ruxkz6Qtla5/router) icon by [Icons8](https://icons8.com)
