---
title: Comparer+
pageLevel: module
moduleLogo: /content/logic.control/img/switch--GiraIconLogicNodesNumericCompare.png
---

## Description

This module extends the "Comparer" module. Multiple comparisons (numbers or text) can be signaled in parallel at the outputs.


Some modules output values that traditionally require multiple comparer modules in a logic sheet. With this module, incoming values can be evaluated simultaneously in one module, similar to a switch-case logic.

## Properties

### Inputs


| Name | Port type | Description |
| --- | --- | --- |
| Input | Integer / Text | Input of the blocker (depending on configuration, the module accepts numeric or text values) |



### Parameters


| Name | Port type | Description |
| --- | --- | --- |
| Comparison mode | Selection | Controls module behavior: *Number* = numeric values are compared. *Text* = string comparison based on current culture. *Text (neutral)* = string comparison without current culture and without case sensitivity. Default value = Number |
| Case statements | Integer | Defines how many comparisons run in parallel |
| Statement x | Integer / Text | Comparison value (depending on configuration, the module accepts numeric or text values) |



### Outputs


| Name | Port type | Description |
| --- | --- | --- |
| No match | Binary | Indicates that the input value does not match any comparison value. |
| Trigger x | Binary | Indicates that the input value matches the corresponding comparison value. |

## Usage

### Example: Scene visualization


Central scenes are often visualized centrally. When a scene number is received, corresponding telegrams can be sent to LEDs, etc.


![Comparer+ - Scene visualization](/content/logic.control/img/switch--example01.png)
*This Comparer+ is used to visualize the most recently selected cross-area scene (scene number --> LEDs).* 


### Example: Evaluating IoT device states


Many IoT devices output states as status codes that can be handled in logic. With this module, an incoming byte value can trigger different signals.


Example: [Error evaluation with Nuki SmartLock](/en/logic.bridges.nuki/smartlock).

## License information

{{moduleLicenseLink path="/en/logic.control#license"}}
- The icon was taken from the "Comparer" component in Gira Project Assistant.
