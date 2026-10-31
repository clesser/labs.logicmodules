---
title: Alarm clock (wake-up time)
pageLevel: module
moduleLogo: /content/logic.timing/img/wakeup--icons8-alarm-44.png
---

## Description

This module selects the nearest time (minus lead time) from multiple wake-up times (e.g., from work, private, and family calendars).


A default time for workdays and holidays can also be enabled and included in the calculation. No lead time is applied to default wake-up times.


The following algorithm is used to determine the wake-up time:


- If default wake-up time is enabled and configured, the default wake-up time is determined based on workday configuration and holidays. Default times are determined for the current and next day and stored in an internal list.
- All appointments are also stored in the internal list with lead time deducted.
- The next timestamp from the current moment is output as wake-up time.


The module does not update the "Wake-up time" output by itself - i.e., an external update trigger is required. This already happens when using [Calendar](/en/logic.timing/calendar) and/or [Holiday calendar](/en/logic.timing/holiday), which update "Next event" after an event starts and therefore also refresh this module's result.

## Properties

### Inputs


| Name | Port type | Description |
| --- | --- | --- |
| Appointment x | Date and time | Next appointment from one calendar. Wake-up time is derived from appointment start time minus lead time. |
| Holiday x | Date and time | A holiday date. If the current day matches this holiday, default holiday wake-up time is chosen (if no appointment is defined). |
| Enable Default Time | Binary | Indicates whether default wake-up time is included in wake-up time calculation (e.g., for workdays and weekends). |
| Default Time (workdays) | Time | Default wake-up time for workdays. Workdays are configured in the module. No lead time is deducted from default wake-up time - it is included directly. |
| Default Time (holidays) | Time | Default wake-up time. Holidays are all weekdays not configured as workdays, or days explicitly marked as holiday. No lead time is deducted from default wake-up time - it is included directly. |



### Parameters


| Name | Port type | Description |
| --- | --- | --- |
| Diaries | Integer | 1..10 appointments can be defined. In combination with [Calendar](/en/logic.timing/calendar), the next appointment can be evaluated from up to 10 calendars. |
| Lead Time (Appointments) | Time | Optional lead time for wake-up time (wake-up time = start time - lead time; 00:00:00 - 23:59:59). Lead time can be used, for example, to allow time for a morning routine. If the first calendar appointment starts at 08:00 and you want to wake up one hour earlier, set lead time to one hour. |
| Holidays | Integer | 0..25 holidays can be defined. In combination with [Holiday calendar](../holiday/), the next holiday can be evaluated from up to 25 calendars. |
| \{Monday..Sunday\} is a workday | Binary | These switches define workdays. |



### Outputs


| Name | Port type | Description |
| --- | --- | --- |
| Wakeup Date and Time | Time | Determines the earliest wake-up time by calculating wake-up time (= start time - lead time) for each calendar and selecting the earliest timestamp. If no wake-up time is found, output is "01.01.0001 00:00:00". |

## Usage

### Example: Determine wake-up time


If your routine starts at different times that cannot be mapped well to weekdays (-> schedule), it makes sense to evaluate existing calendars. Often multiple calendars exist in parallel, so the next or earliest appointment must be selected from all of them.


This module takes the next appointment times from 1..10 calendars and determines the next/earliest one, which can then be forwarded to the alarm clock module.


Because the alarm clock must be activated to actually wake up, next appointments can still be sent throughout the day. The alarm will only trigger if activated during daytime.


![Alarm clock (wake-up time) - Read calendar and holiday calendar](/content/logic.timing/img/wakeup--example01.png)
*First, appointments are read from an Exchange Online calendar and holidays are provided by a web service. To use data across multiple logic sheets, values are stored in X1 variable data points.*


![Alarm clock (wake-up time) - Use calendar and holiday calendar](/content/logic.timing/img/wakeup--example02.png)
*In this example, wake-up time is determined from a calendar. Holiday calendar and workday configuration support selecting default wake-up time.*


Default wake-up times can be provided via scenes, schedules (e.g., weekday/weekend), or input devices to one of the inputs.


For an alarm to trigger, the appointment must be sent in time. For example, if alarm sequence duration is 30 min and lead time of this module is 45 min, the appointment must be sent at least 1h 15min before actual wake-up time by the Calendar module.


If a [Holiday calendar](/en/logic.timing/holiday) is used and a holiday is detected for today, the next holiday is only sent shortly after midnight and may therefore update default wake-up time.

## License information

{{moduleLicenseLink path="/en/logic.timing#license"}}
- [Alarm](https://icons8.com/icon/Cxb6rt2FZRtd/alarm) icon by [Icons8](https://icons8.com)
