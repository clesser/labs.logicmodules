---
title: Übersicht über die Logikpakete
pageLevel: root
---

## Logikpakete

| Bezeichnung | Beschreibung |
|---|---|
| [Aggregationsbausteine](/de/logic.aggregation) | Logikbausteine zur Datenaggregation und Datenauswertung für den GIRA X1.{{br}}[Wertesammler](/de/logic.aggregation/aggregator) |
| [Datenflussbausteine](/de/logic.control) | Logikbausteine zur Steuerung von Telegrammen.{{br}}[Sperre+](/de/logic.control/blocker), [Vergleicher+](/de/logic.control/switch) |
| [Zeitsteuerungsbausteine](/de/logic.timing) | Logikbausteine für zeitabhängige Steuerung von Telegrammen.{{br}}[Beobachter](/de/logic.timing/observer), [Terminkalender](/de/logic.timing/calendar), [Feiertagskalender](/de/logic.timing/holiday), [Wecker (Weckzeit)](/de/logic.timing/wakeup), [Wecker](/de/logic.timing/alarm) |
| [Nuki Bridge Bausteine](/de/logic.bridges.nuki) | Logikbausteine zur Steuerung von Nuki Türschlössern.{{br}}[Nuki SmartLock](/de/logic.bridges.nuki/smartlock), [Nuki Bridge](/de/logic.bridges.nuki/gatewayconnector), [Nuki Auftraggeber](/de/logic.bridges.nuki/gatewayrequest), [Nuki Antwortfilter](/de/logic.bridges.nuki/gatewayresponse) |
| [Velux KLF-200 Bausteine](/de/logic.bridges.velux) | Logikbausteine für die Kommunikation mit dem Velux KLF-200.{{br}}[Velux KLF-200 Gateway](/de/logic.bridges.velux/gatewayconnector), [Velux KLF-200 Auftraggeber](/de/logic.bridges.velux/gatewayrequest), [Velux KLF-200 Antwortfilter](/de/logic.bridges.velux/gatewayresponse) |

## Changelog

| Jahr-Monat | Änderungen |
|---|---|
| 2024-12 | Neuer Baustein zur Ermittlung ganztägiger Ereignisse aus iCal / ics Kalendern ([Feiertagskalender](/de/logic.timing/holiday)), Anpassung von [Wecker (Weckzeit)](/de/logic.timing/wakeup) mit Standardweckzeiten für Werk- und Feiertage sowie Fehlerkorrekturen am Parser von [Terminkalender](/de/logic.timing/calendar).{{br}}Bugfix der internen Verbindungsüberwachung der [Velux KLF-200 Bausteine](/de/logic.bridges.velux). |
| 2024-11 | Fehlerbehebung für eine Parameterkonfiguration von [Wecker (Weckzeit)](/de/logic.timing/wakeup). |
| 2024-09 | Aktualisierung der Velux-KLF200- und Nuki-Komponenten sowie Veröffentlichung des Quellcodes auf GitHub.{{br}}Aufgrund des Downgrades der Kernkomponenten der X1-Firmware musste wieder eine externe Bibliothek zur TLS-1.2-Verschlüsselung für Velux-KLF200 eingebunden werden. Dadurch kann teils eine höhere Speichernutzung angezeigt werden.{{br}}Nuki-Komponenten unterstützen nun alphanumerische Geräte-IDs. |
| 2023-12 | Veröffentlichung der Komponenten in Version 1.0 und Überarbeitung der Dokumentation.{{br}}Version 1.0 erfordert mindestens Gira-X1-Firmware 2.8.858.0 und läuft nicht auf Gira L1.{{br}}Velux-Bausteine wurden erweitert; Nuki-Bausteine erhielten eine neue Architektur (Hinweis: bei Updates Bausteine ggf. neu einfügen). |
| 2022-03 | Initiale Veröffentlichung als Version 0.9 |

## Rechtliche Hinweise

Neleo.Labs mit den hier veröffentlichten Komponenten ist ein rein privat betriebenes Projekt. Sollten unabsichtlich Rechte Dritter berührt oder verletzt worden sein, bitte ich um Kontaktaufnahme (E-Mail: info-at-labs-dot-neleo-dot-com oder via [LinkedIn](https://www.linkedin.com/in/clesser/)).

Diese Webseite nutzt [Azure Static Web Apps](https://azure.microsoft.com/de-de/services/app-service/static/). Der Quellcode liegt auf [GitHub](https://github.com/clesser/labs.logicmodules). Es werden keine weiteren externen Dienste verwendet.
