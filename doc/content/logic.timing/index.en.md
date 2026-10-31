---
title: Timing modules
pageLevel: package
headingTitle: Table of Contents
---

## Description

This package contains components for time-based telegram control on the Gira X1.

## Logic modules

| Name | Description |
|---|---|
| [Observer](/en/logic.timing/observer) | Observer e.g. for presence detection or as a simple alarm system. |
| [Calendar](/en/logic.timing/calendar) | Reads iCal/ics calendars and provides information about the next appointment. |
| [Holiday calendar](/en/logic.timing/holiday) | Reads iCal/ics calendars and provides information about the next all-day event. |
| [Alarm clock (wake-up time)](/en/logic.timing/wakeup) | Selects the earliest time from multiple wake-up times and supports default wake-up times for workdays and holidays. |
| [Alarm clock](/en/logic.timing/alarm) | Alarm clock that can generate telegrams before the actual wake-up time. |

## License

{{mitLicense}}

## Changelog

| Version | Changes |
|---|---|
| 1.1.1 | New module [Holiday calendar](/en/logic.timing/holiday), extension of [Alarm clock (wake-up time)](/en/logic.timing/wakeup) with default wake-up times for workdays and holidays, and parser bugfix in [Calendar](/en/logic.timing/calendar). |
| 1.0.1 | Bugfix for one parameter configuration of [Alarm clock (wake-up time)](/en/logic.timing/wakeup). |
| 1.0.0 | Documentation fix |
| 0.9.1 | Bugfix: Calendar now correctly considers the configured format template. |
| 0.9.0 | First published version |

{{downloadLinks knx="https://service.knx-user-forum.de/?comm=download&id=20000102" appstore="https://www.smarthome-appstore.de/produkt/timing-x1-labs-neleo-com" github="https://github.com/clesser/labs.logicmodules"}}
