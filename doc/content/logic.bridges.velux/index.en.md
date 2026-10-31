---
title: Velux KLF-200 modules
pageLevel: package
headingTitle: Table of Contents
---

## Description

This package contains components for communication with the Velux KLF-200.

For configuration, see additional notes under [Setup](/en/logic.bridges.velux/doc/setup) and [Telegram syntax](/en/logic.bridges.velux/doc/syntax).

The [Velux Gateway](/en/logic.bridges.velux/gatewayconnector) communicates directly with the KLF-200 and keeps the connection alive. [Velux Request Sender](/en/logic.bridges.velux/gatewayrequest) translates telegrams into commands, while [Velux Response Filter](/en/logic.bridges.velux/gatewayresponse) translates responses into KNX/variable telegrams.

Tested device classes:

- *Roller Shutter*
- *Horizontal Awning*
- *On/Off - Switch*
- *Light*
- *Exterior Heating*

For additional testing, the [KLF-200 Tester](/en/logic.bridges.velux/doc/tester) is available.

## Logic modules

| Name | Description |
|---|---|
| [Velux Gateway](/en/logic.bridges.velux/gatewayconnector) | Establishes connection to a Velux KLF-200 Gateway. |
| [Velux Request Sender](/en/logic.bridges.velux/gatewayrequest) | Abstracts the control interface of a device or group; output is connected to the gateway. |
| [Velux Response Filter](/en/logic.bridges.velux/gatewayresponse) | Abstracts the status view of a device or group; input is connected to the gateway. |

The modules communicate internally via [telegrams](/en/logic.bridges.velux/doc/syntax).

## License

{{mitLicense}}

“Velux” and related names and brands describe products and services of the [Velux Group](https://www.velux.com/legal-notice). The provided logic modules use interfaces published on [velux.com/klf200](https://www.velux.com/klf200), but are not products of the Velux Group.

## Changelog

| Version | Changes |
|---|---|
| 1.0.3 | Bugfix of internal connection monitoring. See {{githubIssue number="1" label="GitHub"}}. |
| 1.0.2 | For firmware 2.8.869 (20.12.2023) or 2.8.874 (05.09.2024) without TLS 1.2 support; bugfix of internal connection monitoring. |
| 1.0.1 | For firmware 2.8.869 without TLS 1.2 support; BouncyCastle handles TLS 1.2 encryption again. |
| 1.0.0 | For firmware 2.8.858 with TLS 1.2 support; switched to native TLS functions of X1. |
| 0.9.0 | First published version |

{{downloadLinks knx="https://service.knx-user-forum.de/?comm=download&id=20000104" appstore="https://www.smarthome-appstore.de/produkt/velux-bridges-x1-labs-neleo-com" github="https://github.com/clesser/labs.logicmodules"}}
