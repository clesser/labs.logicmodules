---
title: Zeitsteuerungsbausteine
pageLevel: package
headingTitle: Inhalt des Logikpakets
---

## Beschreibung

Dieses Paket enthält Logikbausteine für den Gira X1, die zur Zeitsteuerung von Telegrammen verwendet werden können.

## Logikbausteine

| Bezeichnung | Beschreibung |
|---|---|
| [Beobachter](/de/logic.timing/observer) | Beobachter u.a. für Anwesenheitserkennung oder als einfache Alarmanlage. |
| [Terminkalender](/de/logic.timing/calendar) | Liest iCal/ics-Kalender ein und stellt Informationen zum nächsten Termin bereit. |
| [Feiertagskalender](/de/logic.timing/holiday) | Liest iCal/ics-Kalender ein und stellt Informationen zum nächsten ganztägigen Ereignis bereit. |
| [Wecker (Weckzeit)](/de/logic.timing/wakeup) | Wählt aus mehreren Weckzeiten den frühesten Termin und unterstützt Standardweckzeiten für Werk- und Feiertage. |
| [Wecker](/de/logic.timing/alarm) | Wecker, der bereits vor der eigentlichen Weckzeit Telegramme erzeugen kann. |

## Lizenz

{{mitLicense}}

## Changelog

| Version | Änderungen |
|---|---|
| 1.1.1 | Neuer Baustein [Feiertagskalender](/de/logic.timing/holiday), Erweiterung von [Wecker (Weckzeit)](/de/logic.timing/wakeup) um Standardweckzeiten für Werk- und Feiertage sowie Fehlerkorrektur im Parser von [Terminkalender](/de/logic.timing/calendar). |
| 1.0.1 | Fehlerkorrektur für eine Parameterkonfiguration von [Wecker (Weckzeit)](/de/logic.timing/wakeup). |
| 1.0.0 | Korrektur der Hilfe |
| 0.9.1 | Fehlerkorrektur: Terminkalender berücksichtigt die eingegebene Formatvorlage korrekt. |
| 0.9.0 | Erste veröffentlichte Version |

{{downloadLinks knx="https://service.knx-user-forum.de/?comm=download&id=20000102" appstore="https://www.smarthome-appstore.de/produkt/timing-x1-labs-neleo-com" github="https://github.com/clesser/labs.logicmodules"}}
