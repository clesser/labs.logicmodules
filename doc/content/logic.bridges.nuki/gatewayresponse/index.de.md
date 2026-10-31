---
title: Antwortfilter
pageLevel: module
moduleLogo: /content/logic.bridges.nuki/img/gatewayresponse--icons8-output-44.png
---

## Beschreibung

Dieser Baustein simuliert die Eingabeseite eines Nuki-Geräts und kommuniziert dafür mit der [Nuki Bridge](/de/logic.bridges.nuki/gatewayconnector). Der Gerätestatus wird dann mittels [Nuki Antwortfilter](/de/logic.bridges.nuki/gatewayresponse) bereitgestellt.


Um diesen Baustein zu nutzen, wird eine Nuki Bridge benötigt, die im gleichen Netzwerk erreichbar ist. Zur Konfiguration sind die Parameter "nukiID" und "deviceType" des zu verwaltenden Geräts erforderlich. Das Auslesen dieser Parameter ist auf dieser Webseite beschrieben:


[Ermitteln der Nuki SmartLock Gerätenummer](https://developer.nuki.io/page/nuki-bridge-http-api-1-13/4/#heading--list) --> Falls mehrere Geräte angezeigt werden, ist die Nummer des Nuki SmartLocks (deviceType = 0 oder 4) relevant.


**Hinweis**: Soll nur ein einzelnes SmartLock gesteuert werden, so gibt es einen Baustein ([Nuki SmartLock](/de/logic.bridges.nuki/smartlock)) der die drei Komponenten (Bridge, Auftraggeber, Antwortfilter) vereint.

## Eigenschaften

### Eingänge


| Bezeichnung | Porttyp | Beschreibung |
| --- | --- | --- |
| Antworten | Text | Empfängt Antworttelegramme, die der [Telegrammsyntax](/de/logic.bridges.nuki/doc/syntax) entsprechen. |



### Parameter


| Bezeichnung | Porttyp | Beschreibung |
| --- | --- | --- |
| Gerät-ID | Text | Legt den Bezeichner des Geräts fest, an das die Anforderungen (Abschließen, Aufschließen, Entsperren, Statusaktualisierung) gesendet werden sollen. |
| Gerättyp | Ganzzahl | Angabe des Nuki-Gerättyps (siehe [Device Types](https://developer.nuki.io/page/nuki-bridge-http-api-1-13/4/#heading--device-types)). |



### Ausgänge


| Bezeichnung | Porttyp | Beschreibung |
| --- | --- | --- |
| Status: Schloss | Ganzzahl | Liefert den Status des Nuki SmartLocks (siehe [Lock State](https://developer.nuki.io/page/nuki-bridge-http-api-1-13/4#heading--lock-states)). |
| Status: Tür | Ganzzahl | Liefert den Status des Nuki DoorSensors (siehe [DoorSensor State](https://developer.nuki.io/page/nuki-bridge-http-api-1-13/4#heading--doorsensor-states)). |
| Status: Batterie | Prozent | Liefert den Ladestand des Nuki SmartLocks (0..100%). |

## Anwendung

### Beispiel: Türschloss


Die Steuerung der Wohnungstür mit Hilfe der Nuki Logikbausteine kann folgendermaßen aussehen:


![Nuki Bridge Logikbaustein](/content/logic.bridges.nuki/img/gatewayconnector--example01.png)
*Anwendungsbeispiel der Nuki Logikbausteine.*


Weitere Informationen sind auf [dieser Seite](/de/logic.bridges.nuki/gatewayconnector) zusammengefasst.

## Lizenzinformationen

- {{sectionLink path="/de/logic.bridges.nuki" section="lizenz" label="Lizenzbedingungen und rechtliche Hinweise für diesen Logikbaustein"}}
- [Output](https://icons8.com/icon/90318/output) icon by [Icons8](https://icons8.com)
