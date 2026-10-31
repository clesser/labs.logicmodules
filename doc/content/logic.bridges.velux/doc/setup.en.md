---
title: [neleo.labs] X1 Velux KLF modules
---

## Introduction

Characteristics of KNX, Gira X1, and Velux KLF-200 can make connecting all three feel complex at first glance.


This page walks through practical configurations.


The configurations below focus on individual actuators/devices. If more than 3 actuators/devices should be controlled at the same time (simultaneous start/value change), it is recommended to define groups and scenes and call those instead.


Good results were also achieved by controlling individual devices with delay (about 2 seconds per device using a telegram delay module) to avoid overloading Velux KLF-200 capacity.

## X1 standalone mode

In this scenario, a virtual actuator is modeled directly in X1. Only variable data points are required. For a shutter actuator, it can look like this:


### GPA data points


![Configuration of variable data points and visualization](/content/logic.bridges.velux/img/doc--example01-01.png)
*Configuration of variable data points for shutter visualization in Gira Project Assistant*


### GPA logic sheets


![Logic sheet for controlling a single shutter](/content/logic.bridges.velux/img/doc--example01-02.png)
*If shutter control is implemented exclusively in Gira X1, control via Min, Max, and Stop can be used instead of short and long operation. This loses short-step operation but allows direct % positioning.*


Even if only one actuator is modeled, node identifiers in request sender and response filter must be set correctly so telegrams are processed correctly.

## KNX and X1 mixed operation

A mixed setup is usually more realistic, where some functions are triggered via classic KNX push-buttons. In the case described below, a Gira push sensor 3 sends long/short commands (up/down) to an awning.


Position control and movement feedback of the awning are processed and visualized exclusively in Gira X1.


### ETS parameterization


![Configuration of push sensor in ETS](/content/logic.bridges.velux/img/doc--example02-01.png)
*In ETS, configuration "Long-Short" has proven useful. Based on personal preference, downward movement is mapped to the left rocker and upward movement to the right rocker.*


### GPA data points


![Configuration of group-address and variable data points in visualization](/content/logic.bridges.velux/img/doc--example02-02.png)
*In this example, movement commands are controlled via group-address data points and position/feedback via variables.*


Of course, all data points can also be defined as group-address data points. In some cases, however, X1 tends to swallow telegrams when no KNX participant listens to the address.


If **position** is configured as group-address data point, the GA for feedback (= output of response filter) must be entered under "Send" and GA for incoming position requests under "Status/Feedback". Only then KNX telegrams are interpreted correctly and status is written back correctly.


### GPA logic sheets


![Logic sheet with central gateway logic module](/content/logic.bridges.velux/img/doc--example02-03.png)
*Since this specific setup includes many shutters and awnings, modules are not all placed on one logic sheet but grouped by room. The central gateway module gets its own logic sheet and two text-variable data points through which all modules communicate.*


![Logic sheet of awning control](/content/logic.bridges.velux/img/doc--example02-04.png)
*Awning control is on a dedicated logic sheet. Module inputs and outputs are connected to previously defined group-address and variable data points.*


![Logic sheet with group control](/content/logic.bridges.velux/img/doc--example02-05.png)
*The two request senders are assigned to groups (configuration see "Safety considerations") and move awnings in at night and shutters down at dusk/up at dawn, unless blocked by automation (see also: [Blocker+](/en/logic.control/blocker)).*

## Safety considerations

The signal path from button/weather sensor over KNX bus to X1, then to Velux KLF-200 Gateway, and then to shutter/awning is long and can be prone to faults. For safety functions, it may therefore be worth bypassing X1 and network. The potential-free contacts of Velux KLF-200 are suitable for this.


In the case shown below, all shutters and all awnings are grouped into two groups. These groups can be used via request sender (in group configuration; e.g., open shutters in morning, retract awnings in evening).


![Group configuration in Velux KLF-200](/content/logic.bridges.velux/img/doc--example03-01.png)
*Shutters and awnings are grouped separately to trigger different safety functions.*


In addition, simple switching actuators can trigger safety actions (e.g., raise shutters on smoke alarm, retract awnings on rain or wind). The switch actuator is wired potential-free to the inputs.


When mixing potential-free inputs via KNX actuators, ensure that the same group addresses in X1 (e.g., via request sender in group configuration) do **not** trigger the same action, otherwise commands stop each other.


It is still easy to trigger safety functions via potential-free inputs and comfort functions via X1. Just use different group addresses/data points.
