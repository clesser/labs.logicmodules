---
title: Velux KLF-200 Bausteine
pageLevel: package
headingTitle: Inhalt des Logikpakets
---

## Beschreibung

Dieses Paket enthält Logikbausteine, die die Kommunikation mit dem Velux KLF-200 ermöglichen.

Für die Konfiguration gibt es zusätzliche Hinweise unter [Einrichtung](/de/logic.bridges.velux/doc/setup) und [Telegramm-Syntax](/de/logic.bridges.velux/doc/syntax).

Das [Velux Gateway](/de/logic.bridges.velux/gatewayconnector) kommuniziert direkt mit dem KLF-200 und hält die Verbindung aktiv. [Velux Auftraggeber](/de/logic.bridges.velux/gatewayrequest) übersetzen Telegramme in Anweisungen, [Velux Ausgabefilter](/de/logic.bridges.velux/gatewayresponse) übersetzen Antworten in KNX-/Variablen-Telegramme.

Getestete Geräteklassen:

- *Roller Shutter* (Rollläden)
- *Horizontal Awning* (Markisen)
- *On/Off - Switch* (Schalter)
- *Light* (Licht)
- *Exterior Heating* (Terrassenheizung)

Für zusätzliche Tests steht der [KLF-200 Tester](/de/logic.bridges.velux/doc/tester) bereit.

## Logikbausteine

| Bezeichnung | Beschreibung |
|---|---|
| [Velux Gateway](/de/logic.bridges.velux/gatewayconnector) | Stellt die Verbindung zu einem Velux KLF-200 Gateway her. |
| [Velux Auftraggeber](/de/logic.bridges.velux/gatewayrequest) | Abstrahiert die Bedienschnittstelle eines Geräts oder einer Gruppe; Verbindung am Ausgang zum Gateway. |
| [Velux Ausgabefilter](/de/logic.bridges.velux/gatewayresponse) | Abstrahiert die Statusanzeige eines Geräts oder einer Gruppe; Verbindung am Eingang zum Gateway. |

Die Bausteine kommunizieren intern über [Telegramme](/de/logic.bridges.velux/doc/syntax).

## Lizenz

{{mitLicense}}

„Velux“ und damit verbundene Namen und Marken beschreiben Produkte und Dienste der [Velux Group](https://www.velux.com/legal-notice). Die angebotenen Logikbausteine nutzen die unter [velux.com/klf200](https://www.velux.com/klf200) veröffentlichten Schnittstellen, sind jedoch selbst kein Produkt der Velux Group.

## Changelog

| Version | Änderungen |
|---|---|
| 1.0.3 | Bugfix der internen Verbindungsüberwachung. Siehe {{githubIssue number="1" label="GitHub"}}. |
| 1.0.2 | Für Firmware 2.8.869 (20.12.2023) oder 2.8.874 (05.09.2024) ohne TLS-1.2-Unterstützung; Bugfix der internen Verbindungsüberwachung. |
| 1.0.1 | Für Firmware 2.8.869 ohne TLS-1.2-Unterstützung; BouncyCastle übernimmt wieder TLS-1.2-Verschlüsselung. |
| 1.0.0 | Für Firmware 2.8.858 mit TLS-1.2-Unterstützung; Umstellung auf native TLS-Funktionen des X1. |
| 0.9.0 | Erste veröffentlichte Version |

{{downloadLinks knx="https://service.knx-user-forum.de/?comm=download&id=20000104" appstore="https://www.smarthome-appstore.de/produkt/velux-bridges-x1-labs-neleo-com" github="https://github.com/clesser/labs.logicmodules"}}
