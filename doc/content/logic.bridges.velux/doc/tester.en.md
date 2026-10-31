---
title: [neleo.labs] X1 Velux KLF modules
---

## Introduction

In some cases, configuring setup with Gira X1, Velux KLF-200, and Gira Project Assistant (GPA) can be quite time-consuming.


For this reason, I provide a command-line application that I used during development. It helps test functions more easily using [telegram syntax](/en/logic.bridges.velux/doc/syntax).


The application requires .NET Framework, which is already installed together with GPA.

## Download and start

The application can be downloaded as [KLF200Tester.zip](https://1drv.ms/u/c/19ea6aaf4b1f919a/EQNViBgZ5QlMq81n5-HUQwIBJAuOsFukZe4_3_INWDiSnA).


After extracting the ZIP file, start it by double-clicking KLF200Tester.exe. If you use the tester frequently, create a shortcut with all parameters:


- "Path\to\KLF200Tester.exe"
- "Path\to\KLF200Tester.exe" **
- "Path\to\KLF200Tester.exe" ** *
* Parameters not provided are requested by the application.


- Example: KLF200Tester 192.168.1.100 P@ssw0rd

## Usage

After startup, missing parameters are requested and the application tries to connect to KLF-200.


Then the application asks for operation mode:


- (S)ocket - shows communication with KLF-200 and is mainly intended for debugging.
- (G)ateway - allows sending and receiving telegrams according to [telegram syntax](/en/logic.bridges.velux/doc/syntax).


(G)ateway is default mode and can be selected by pressing [Enter]. The application then starts receiving status of all devices registered on KLF-200. It may happen that not all devices report status.


Telegrams shown and received by the application are documented in [telegram syntax](/en/logic.bridges.velux/doc/syntax) and correspond to those used by [Request Sender](/en/logic.bridges.velux/gatewayrequest) and [Response Filter](/en/logic.bridges.velux/gatewayresponse).


Using #min and #max parameters, minimum and maximum values for devices can be set. These are not always self-explanatory...


- Shutters, awnings, roof windows, shades: #min = fully retracted, #max = fully extended / opened
- Lights, heaters: #min = light/heating on, #max = light/heating off...


Using this tester, target values for different devices can be determined and then used either via provided logic modules or via simple telegram generators / text formatters in an X1 logic sheet.
