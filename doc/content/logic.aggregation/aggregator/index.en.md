---
title: Value collector
pageLevel: module
moduleLogo: /content/logic.aggregation/img/aggregator--icons8-bar-chart-44.png
---

## Description

This module collects daily and hourly data and provides statistical analysis.


The main use case is hourly logging of data (e.g., temperature) over a period of 1–14 days, so that switching actions (e.g., start/end of heating season) can be derived from it.


For each hour, the first incoming value is stored internally. This means up to 24 values per day are available internally. Since raw data is stored, the evaluation method can be changed later without invalidating already collected data.


:::warning
Removing the logic module from a logic sheet, resetting X1/L1, or deleting application data during commissioning will clear the internal value storage!
:::

## Properties

### Inputs


| Name | Port type | Description |
| --- | --- | --- |
| Input | Number | Input of the value collector |



### Parameters


| Name | Port type | Description |
| --- | --- | --- |
| Evaluation method | Selection | Defines the calculation rule used to aggregate collected values. *Average* = the [arithmetic mean](https://en.wikipedia.org/wiki/Arithmetic_mean) is calculated across all available values of the period. *Median* = the [median](https://en.wikipedia.org/wiki/Median) is selected across all available values of the period. *Mode* = the [mode](https://en.wikipedia.org/wiki/Mode_(statistics)) is selected from period values (rounded to whole numbers). Default value = Average |
| Evaluation period | Integer | Defines the number of days in the past (1..14) used for evaluation. Values remain available after restarting X1/L1 - however, calculations are only applied to available days. If 5 days are configured but data exists for only 4 days, only 4 days are evaluated. |



### Outputs


| Name | Port type | Description |
| --- | --- | --- |
| Maximum (overall) | Number | Returns the maximum value of all cached values in the full evaluation period. |
| Average (overall) | Number | Returns the arithmetic mean of all cached values in the full evaluation period. |
| Average (today-x) | Number | Returns the arithmetic mean for one previous day (Today-1 = yesterday, Today-2 = day before yesterday, ...). |
| Median (overall) | Number | Returns the median of all cached values in the full evaluation period. |
| Median (today-x) | Number | Returns the median for one previous day (Today-1 = yesterday, Today-2 = day before yesterday, ...). |
| Mode (overall) | Number | Returns the mode of all cached values in the full evaluation period. |
| Mode (today-x) | Number | Returns the mode for one previous day (Today-1 = yesterday, Today-2 = day before yesterday, ...). |
| Minimum (overall) | Number | Returns the minimum value of all cached values in the full evaluation period. |

## Usage

### Example: Heating season


A main use case is calculating the [heating season](https://en.wikipedia.org/wiki/Heating_degree_day). Typically, daily average temperatures are captured hourly for the previous five days. In Germany, the heating season starts if the five-day average drops below 15°C.


To stay flexible, the module focuses on hourly collection of temperature values and aggregation via a calculation rule (average, median, mode). Together with comparer modules (or threshold modules with hysteresis), an individual heating season logic can be implemented.


Especially during transition periods, significant fluctuations can occur. These can be smoothed, for example, using the median as the calculation rule. It can also make a difference whether daily average temperatures (comparer + AND combination) or the average of the full observation period is passed to comparers.


![Value collector - Simple heating season control](/content/logic.aggregation/img/aggregator--example01.png)
*Simplified evaluation: If the average temperature of the last 5 days was below 15°C, the heating season starts.*


![Value collector - Regulation-compliant heating season control](/content/logic.aggregation/img/aggregator--example02.png)
*Possible regulation-compliant evaluation (for Germany - unless the heating season is defined by calendar): The heating season starts when 5 consecutive cold days are detected and ends when 5 consecutive warm days are detected.*


:::warning
The example uses Horst Lehner's Formula Calculation module ([download link on knx-user-forum.de](http://service.knx-user-forum.de/?comm=download&id=20000010)) to avoid one complex AND and one inverted AND gate.
:::


This counts consecutive days below 15°C (5 comparers) and aggregates boolean outputs via type conversion and a simple sum formula "\{Day1:N\}+\{Day2:N\}+\{Day3:N\}+\{Day4:N\}+\{Day5:N\}" using the formula module.


If five consecutive days are too cold, the heating season starts. If zero consecutive days are too warm, it ends. For this, the "Threshold with hysteresis" module must be set to 1..4 so values can cross above/below properly.

## License information

{{moduleLicenseLink path="/en/logic.aggregation#license"}}
- [Bar Chart](https://icons8.com/icon/Ww4I6zPDi4xI/bar-chart) icon by [Icons8](https://icons8.com)
