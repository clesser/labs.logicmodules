---
title: Observer
pageLevel: module
moduleLogo: /content/logic.timing/img/observer--icons8-attendance-44.png
---

## Description

This module is an observer for e.g. presence detection and multi-level alarming.


After the observer is activated by an event, it waits for the configured lag time and ignores activity signals during that period. Then up to 15 consecutive waiting times start, each signaled at its output when elapsed. If an activity signal arrives during observation time (= the waiting times), observation is ended and signaled at the output.

## Properties

### Inputs


| Name | Port type | Description |
| --- | --- | --- |
| Start Observation Cycle | Binary | Resets all outputs and starts a new observation cycle (lag time + waiting times 1..15). |
| Activity | Binary | Reports activity (1 / true), which aborts the observation cycle. |



### Parameters


| Name | Port type | Description |
| --- | --- | --- |
| Lag time | Timespan | Defines the initial timespan of an observation cycle during which activity signals are ignored. |
| Observation Steps | Integer | Defines 1..15 possible observation windows. Each observation step is defined by one wait time (parameter) and one corresponding trigger (output). |
| Wait time x | Timespan | Defines wait time of an observation step. If no activity is received during this wait time, a 1/true is sent to the corresponding output when the step ends. Wait times are cumulative. Example: if first step should end 30 min after cycle start and second step 60 min after cycle start (assuming lag time = 0), set both steps to "30 min". |



### Outputs


| Name | Port type | Description |
| --- | --- | --- |
| Observation Active | Binary | Indicates whether an observation cycle is currently active. The cycle starts after sending 1/true to the corresponding input and ends when either the last step is reached or an activity signal is detected that aborts the cycle. |
| Wait time x over | Binary | Sends a "1" telegram when the corresponding wait time elapsed and no activity was received in the meantime. |
| Activity detected | Binary | Sends a "1" telegram when activity is detected and a running (or elapsed) cycle is aborted. |

## Usage

### Example: Presence detection


This use case is inspired by Sönke Jacobs' "Presence" module, but is designed to be more universal.


In the example below, presence detection is activated whenever the apartment door is opened/closed. After a lag time, during which existing automations can move into idle state, three observation windows begin. Different actions/scenes (e.g., Away, Business Trip, Vacation) can be assigned to them.


A blocker can suppress activation of the observation period. For this, balcony/terrace doors or scenes (sleeping, plant watering during vacation) can be used.


![Observer - Switch scenes based on absence duration](/content/logic.timing/img/observer--example01.png)
*This example uses value generators at different outputs to map states through scenes in one scene set. The blocker suppresses presence detection when certain scenes (2, 3, 6) are active. These blocking scenes are reset indirectly after at most 15h by starting an observation cycle (thereby bypassing the blocker). After about 15h (+ 5m) + 30m + 11h30m, final status ("Scene 5") is always reached if no activity was detected.*


### Example: Multi-level alarming


This logic can also implement a multi-level watchdog: after activation (= start observation cycle), it waits for another event (= activity). While wait time passes, actions (e.g., notice, warning, pre-alarm, alarm) can be triggered at defined times.


![Observer - Multi-level alarming](/content/logic.timing/img/observer--example02.png)
*In this case, monitoring window-open duration starts on rising edge (= open) and stops on falling edge (= close). In between, warning and alarm are triggered after specific time marks pass.*

## License information

{{moduleLicenseLink path="/en/logic.timing#license"}}
- [Attendance](https://icons8.com/icon/50896/attendance) icon by [Icons8](https://icons8.com)
