---
title: [neleo.labs] X1 Nuki Bridge modules
---

## Description

Communication between logic modules uses telegrams that follow a URL-like syntax:


- request:///\{lock|unlock|unlatch|info\}
- response:///\{lock|unlock|unlatch|info\}?param1=value1


[Nuki Request Sender](/en/logic.bridges.nuki/gatewayrequest) modules generate "request://" telegrams and send them to the input of [Nuki Bridge](/en/logic.bridges.nuki/gatewayconnector). The bridge responds with "response://" telegrams and sends them from its output to all [Nuki Response Filter](/en/logic.bridges.nuki/gatewayresponse) inputs ("broadcast").


Each telegram defines Nuki ID (*device-id*), Nuki device type (*device-type*), and an action (e.g., *lock*, *info*).


For responses, an optional parameter list follows. It starts with question mark ("?") and the first key-value pair. Additional pairs are appended using ampersand ("&").


In most cases, manually constructing telegrams is not required - but it can be used to integrate devices that are not officially tested yet.

## Properties

### Protocol


| Name | Description |
| --- | --- |
| request:// | Telegram addressed to input of [Nuki Bridge](/en/logic.bridges.nuki/gatewayconnector). Used to query properties or trigger actions on devices. |
| response:// | Telegram sent from output of [Nuki Bridge](/en/logic.bridges.nuki/gatewayconnector). Indicates property changes of devices. |



### Identifiers


| Name | Protocols | Description |
| --- | --- | --- |
| device-id | request{{br}}response | Unique identifier of a device ("nukiID"). IDs of all devices paired with a bridge can be retrieved using [/list](https://developer.nuki.io/page/nuki-bridge-http-api-1-13/4/#heading--list). |
| device-type | request{{br}}response | Type of a device ("deviceType"). Device types of all paired devices can be retrieved using [/list](https://developer.nuki.io/page/nuki-bridge-http-api-1-13/4/#heading--list). |



### Actions


| Name | Protocols | Description |
| --- | --- | --- |
| /lock | request{{br}}response | Requests action "2" (= "lock", see [Lock Actions](https://developer.nuki.io/page/nuki-bridge-http-api-1-13/4/#heading--lock-actions)) for the specified device. Response indicates successful or failed execution. |
| /unlock | request{{br}}response | Requests action "1" (= "unlock", see [Lock Actions](https://developer.nuki.io/page/nuki-bridge-http-api-1-13/4/#heading--lock-actions)) for the specified device. Response indicates successful or failed execution. |
| /unlatch | request{{br}}response | Requests action "3" (= "unlatch", see [Lock Actions](https://developer.nuki.io/page/nuki-bridge-http-api-1-13/4/#heading--lock-actions)) for the specified device. Response indicates successful or failed execution. |
| /info | request{{br}}response | Requests device status and, if available, door sensor and battery status from the specified device. Response returns those values. |



### Parameters


| Name | Protocols | Description |
| --- | --- | --- |
| success | response | Defines if an action was successful: *true* *false* |
| lock | response | Returns lock status (typically smart lock): *1* - locked *3* - unlocked *5* - unlatched. See also: [Nuki Lock States](https://developer.nuki.io/page/nuki-bridge-http-api-1-13/4/#heading--lock-states) |
| door | response | Returns door sensor status: *2* - closed *3* - open. See also: [Nuki Door Sensor States](https://developer.nuki.io/page/nuki-bridge-http-api-1-13/4/#heading--doorsensor-states) |
| battery | response | Battery/accumulator charge level of the Nuki device in percent. |

## Usage

The following examples assume a second-generation Nuki SmartLock (device-type=0) with identifier "13579" (device-id=13579):


request://13579:0/lock ► Locks the door.


response://13579:0/lock?success=true ► Confirms the door was locked.


request://13579:0/info ► Requests current device status.


response://13579:0/info?lock=3&door=2&battery=50 ► Indicates that the door is unlocked (lock=3), not open (door=2), and battery/accumulator level is 50%.
