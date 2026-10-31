---
title: Packages Overview
pageLevel: root
---

## Logic modules

| Name | Description |
|---|---|
| [Aggregation modules](/en/logic.aggregation) | Logic modules for data aggregation and data analysis for the GIRA X1.{{br}}[Value collector](/en/logic.aggregation/aggregator) |
| [Control-flow modules](/en/logic.control) | Logic modules for controlling telegrams.{{br}}[Blocker+](/en/logic.control/blocker), [Comparer+](/en/logic.control/switch) |
| [Timing modules](/en/logic.timing) | Logic modules for time-based telegram control.{{br}}[Observer](/en/logic.timing/observer), [Calendar](/en/logic.timing/calendar), [Holiday calendar](/en/logic.timing/holiday), [Alarm clock (wake-up time)](/en/logic.timing/wakeup), [Alarm clock](/en/logic.timing/alarm) |
| [Nuki Bridge modules](/en/logic.bridges.nuki) | Logic modules for controlling Nuki door locks.{{br}}[Nuki SmartLock](/en/logic.bridges.nuki/smartlock), [Nuki Bridge](/en/logic.bridges.nuki/gatewayconnector), [Nuki Request Sender](/en/logic.bridges.nuki/gatewayrequest), [Nuki Response Filter](/en/logic.bridges.nuki/gatewayresponse) |
| [Velux KLF-200 modules](/en/logic.bridges.velux) | Logic modules for communication with the Velux KLF-200.{{br}}[Velux KLF-200 Gateway](/en/logic.bridges.velux/gatewayconnector), [Velux KLF-200 Request Sender](/en/logic.bridges.velux/gatewayrequest), [Velux KLF-200 Response Filter](/en/logic.bridges.velux/gatewayresponse) |

## Changelog

| Year-Month | Changes |
|---|---|
| 2024-12 | New module for detecting all-day events from iCal / ics calendars ([Holiday calendar](/en/logic.timing/holiday)), update of [Alarm clock (wake-up time)](/en/logic.timing/wakeup) with default wake-up times for workdays and holidays, plus parser fixes for [Calendar](/en/logic.timing/calendar).{{br}}Bugfix for the internal connection monitoring of the [Velux KLF-200 modules](/en/logic.bridges.velux). |
| 2024-11 | Bugfix for one parameter configuration of [Alarm clock (wake-up time)](/en/logic.timing/wakeup). |
| 2024-09 | Update of Velux-KLF200 and Nuki components plus source code release on GitHub.{{br}}Due to a downgrade of core X1 firmware components, an external TLS 1.2 library had to be integrated again for Velux-KLF200. This may show higher memory usage in some cases.{{br}}Nuki components now support alphanumeric device IDs. |
| 2023-12 | Release of version 1.0 and documentation revision.{{br}}Version 1.0 requires at least Gira X1 firmware 2.8.858.0 and does not run on Gira L1.{{br}}Velux modules were extended; Nuki modules received a new architecture (note: for updates, remove and re-add modules if needed). |
| 2022-03 | Initial release as version 0.9 |

## Legal notice

Neleo.Labs and the published components are a purely private project. If third-party rights were unintentionally affected or violated, please contact me (email: info-at-labs-dot-neleo-dot-com or via [LinkedIn](https://www.linkedin.com/in/clesser/)).

This website uses [Azure Static Web Apps](https://azure.microsoft.com/de-de/services/app-service/static/). The source code is hosted on [GitHub](https://github.com/clesser/labs.logicmodules). No additional external services are used.
