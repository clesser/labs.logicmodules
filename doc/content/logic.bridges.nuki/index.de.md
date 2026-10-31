---
title: Nuki Bridge Bausteine
pageLevel: package
headingTitle: Inhalt des Logikpakets
---

## Beschreibung

Dieses Paket enthält Logikbausteine zur Steuerung von Nuki Türschlössern.

Soll nur ein einzelnes Türschloss über die Nuki Bridge gesteuert werden, kann [Nuki SmartLock](/de/logic.bridges.nuki/smartlock) verwendet werden.

Sind mehrere Geräte an einer Nuki Bridge angeschlossen, wird empfohlen, jedes Gerät mit je einem [Nuki Auftraggeber](/de/logic.bridges.nuki/gatewayrequest) und einem [Nuki Antwortfilter](/de/logic.bridges.nuki/gatewayresponse) abzubilden und diese Komponenten mit einer [Nuki Bridge](/de/logic.bridges.nuki/gatewayconnector) zu verbinden.

Mit Hilfe von [Telegrammen](/de/logic.bridges.nuki/doc/syntax) können prinzipiell beliebige Geräte angesteuert werden.

## Logikbausteine

| Bezeichnung | Beschreibung |
|---|---|
| [Nuki SmartLock](/de/logic.bridges.nuki/smartlock) | Sperren, Entsperren und Entriegeln eines einzelnen Nuki SmartLock; liefert Schließzustand, Türsensor und Ladestand. |
| [Nuki Bridge](/de/logic.bridges.nuki/gatewayconnector) | Stellt die Verbindung zur Nuki Bridge her und ermöglicht die Verwendung mehrerer Geräte. |
| [Nuki Auftraggeber](/de/logic.bridges.nuki/gatewayrequest) | Eingangsseite eines Nuki-Geräts, über die Anforderungen gesendet werden können. |
| [Nuki Antwortfilter](/de/logic.bridges.nuki/gatewayresponse) | Ausgangsseite eines Nuki-Geräts zum Auslesen von Statusinformationen. |

Die drei unteren Bausteine kommunizieren über [Telegramme](/de/logic.bridges.nuki/doc/syntax), die an eine URL angelehnt sind.

## Lizenz

{{mitLicense}}

„Nuki“ und damit verbundene Namen beschreiben Produkte und Dienste der [Nuki Home Solutions GmbH](https://nuki.io/de/impressum/). Die angebotenen Logikbausteine nutzen die unter [developer.nuki.io](https://developer.nuki.io/) veröffentlichten Schnittstellen, sind jedoch selbst kein Produkt der Nuki Home Solutions GmbH.

## Changelog

| Version | Änderungen |
|---|---|
| 1.0.1 | Geräte-ID kann nun alphanumerische Zeichen enthalten; Datenfeld von „Zahl“ auf „Text“ geändert. |
| 1.0.0 | Neue interne Architektur der Nuki-Bausteine. Hinweis: Bei Updates müssen verwendete Bausteine ggf. entfernt und neu hinzugefügt werden. |
| 0.9.5 | Token wird nicht mehr gehashed übertragen; Übertragung im Klartext. |
| 0.9.0 | Erste veröffentlichte Version |

{{downloadLinks knx="https://service.knx-user-forum.de/?comm=download&id=20000103" appstore="https://www.smarthome-appstore.de/produkt/nuki-bridges-x1-labs-neleo-com" github="https://github.com/clesser/labs.logicmodules"}}
