---
title: Alarm clock
pageLevel: module
moduleLogo: /content/logic.timing/img/alarm--icons8-alarm-44.png
---

## Description

This module is an alarm clock that can generate telegrams before the actual wake-up time. This allows, for example, simulating sunrise alarms or switching devices on step by step.


Alternatively, trigger mode is available. In that mode, a regular schedule is used for triggering and sequence duration is subtracted from wake-up time (wake-up time: 08:00, sequence duration: 30 min ==> trigger at 07:30).

## Properties

### Inputs


| Name | Port type | Description |
| --- | --- | --- |
| Time (one-time) | Date and time | Starts the wake-up sequence so that alarm is triggered at the configured time on the configured day. The alarm is triggered only on that specific day/time. |
| Time (daily) | Time | Starts the wake-up sequence so that alarm is triggered at the configured time. The alarm is reconfigured every day for that time. |
| Sequence Trigger | Binary | Starts wake-up sequence when receiving 1 |
| Snooze/Off | Binary | Controls behavior: 0 = switches (from sequence step or alarm) to snooze mode; 1 = ends wake-up sequence and exits alarm/snooze mode |
| Alarm Active | Binary | Controls behavior: 0 = wake-up sequences are not started and running ones are canceled; 1 = wake-up sequences can start automatically (mode: time) or by trigger (mode: trigger) |



### Parameters


| Name | Port type | Description |
| --- | --- | --- |
| Alarm Mode | Selection | Defines how sequence starts. *Time (one-time)* = at a fixed time on a fixed date. *Time (daily)* = at a fixed time every day. *Trigger* = by incoming telegram. Default value = Trigger |
| Sequence Steps | Integer | Number of steps (0..16) executed before alarm. Each step has one step duration and one trigger. |
| Duration x | Timespan | Duration of one step (15 sec .. 30 min). Step durations are configured absolutely (not relative to wake-up time). |
| Snooze Time | Timespan | Snooze duration (15 sec .. 30 min). After alarm is triggered, snooze mode can be activated (telegram "0" to "Snooze/Off"). After snooze duration, alarm is triggered again. |



### Outputs


| Name | Port type | Description |
| --- | --- | --- |
| Sequence Active | Binary | Indicates whether wake-up sequence is currently active. Output becomes "1" when the first step starts and remains "1" during alarm and snooze modes. If sequence is ended with telegram "1" at "Snooze/Off", output returns to "0". |
| Step x | Binary | Sends "1" telegram when step is active and then falls back to "0". |
| Alarm | Binary | Sends "1" telegram when alarm is triggered. Sends "0" telegram when snooze mode becomes active or sequence ends. |
| Snooze | Binary | Sends "1" telegram when snooze mode is active. Sends "0" telegram when snooze mode ends (e.g., because alarm is triggered again or sequence ended). |
| Alarm Active | Binary | Output indicates whether a wake-up time is defined for the next 24h (mode "Time") and whether alarm is enabled ("Alarm Active" = true). Output is refreshed hourly if a value change for the next 24h occurs. Example: if next alarm for Monday is set on Friday, output only shows active alarm on Sunday. In "Trigger" mode, output equals input "Alarm Active". |

## Usage

### Example: Alarm clock with multiple switch points


If people wake up at the same time every day, schedules are often used to automate flows. But if daily routines shift (e.g., due to irregular travel), the need for one central alarm clock from which all switch points are derived quickly appears.


Within one wake-up sequence, multiple switch points can be defined, e.g., to simulate sunrise, switch on hot water preparation, or power entertainment devices / coffee machine. Typically, these actions happen before actual wake-up time.


The alarm clock therefore calculates total duration of all steps before wake-up time and starts the sequence at the calculated time, so that after all steps run, the actual alarm is triggered at the configured wake-up time. Snooze functionality completes the alarm clock.


![Alarm clock - Scene-based wake-up sequence](/content/logic.timing/img/alarm--example01.png)
*This example uses value generators at different outputs to map individual switch states via scenes in one scene set.*


### Example: Using wake-up times


An example using mode "Time" is described in combination with [Alarm clock (wake-up time)](/en/logic.timing/wakeup).

## License information

{{moduleLicenseLink path="/en/logic.timing#license"}}
- [Alarm](https://icons8.com/icon/Cxb6rt2FZRtd/alarm) icon by [Icons8](https://icons8.com)
