---
title: Nuki Bridge modules
pageLevel: package
headingTitle: Table of Contents
---

## Description

This package contains components for controlling Nuki door locks.

If only one single door lock should be controlled via Nuki Bridge, [Nuki SmartLock](/en/logic.bridges.nuki/smartlock) can be used.

If multiple devices are connected to one Nuki Bridge, it is recommended to model each device with one [Nuki Request Sender](/en/logic.bridges.nuki/gatewayrequest) and one [Nuki Response Filter](/en/logic.bridges.nuki/gatewayresponse), and connect those components with one [Nuki Bridge](/en/logic.bridges.nuki/gatewayconnector).

With the help of [telegrams](/en/logic.bridges.nuki/doc/syntax), virtually any device can be addressed.

## Logic modules

| Name | Description |
|---|---|
| [Nuki SmartLock](/en/logic.bridges.nuki/smartlock) | Lock, unlock and unlatch a single Nuki SmartLock; provides lock state, door sensor and battery level. |
| [Nuki Bridge](/en/logic.bridges.nuki/gatewayconnector) | Establishes connection to the Nuki Bridge and enables usage with multiple devices. |
| [Nuki Request Sender](/en/logic.bridges.nuki/gatewayrequest) | Input-side module of a Nuki device for sending commands. |
| [Nuki Response Filter](/en/logic.bridges.nuki/gatewayresponse) | Output-side module of a Nuki device for reading status information. |

The three lower modules communicate using [telegrams](/en/logic.bridges.nuki/doc/syntax) that follow a URL-like syntax.

## License

{{mitLicense}}

“Nuki” and related names describe products and services of [Nuki Home Solutions GmbH](https://nuki.io/de/impressum/). The provided logic modules use interfaces published on [developer.nuki.io](https://developer.nuki.io/), but are not products of Nuki Home Solutions GmbH.

## Changelog

| Version | Changes |
|---|---|
| 1.0.1 | Device ID can now contain alphanumeric characters; data field changed from “Number” to “Text”. |
| 1.0.0 | New internal architecture of Nuki modules. Note: after updates, used modules may need to be removed and re-added. |
| 0.9.5 | Token is no longer transmitted hashed; transmission is plain text. |
| 0.9.0 | First published version |

{{downloadLinks knx="https://service.knx-user-forum.de/?comm=download&id=20000103" appstore="https://www.smarthome-appstore.de/produkt/nuki-bridges-x1-labs-neleo-com" github="https://github.com/clesser/labs.logicmodules"}}
