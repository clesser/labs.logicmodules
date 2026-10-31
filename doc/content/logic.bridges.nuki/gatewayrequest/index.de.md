---
title: Auftraggeber
pageLevel: module
moduleLogo: /content/logic.bridges.nuki/img/gatewayrequest--icons8-input-44.png
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
| Abschließen | Binär | Fordert zum Abschließen (Sperren) der Tür auf, wenn eine 1 übermittelt wird. |
| Aufschließen | Binär | Fordert zum Aufschließen (Entsperren) der Tür auf, wenn eine 1 übermittelt wird. Hinweis: Je nach Konfiguration des Nuki SmartLocks kann "Aufschließen" den Schlüssel genauso weit drehen wie beim "Entsperren". |
| Entsperren | Binär | Fordert zum Entsperren des Türriegels auf, wenn eine 1 gesendet wird. Falls die Tür abgeschlossen war, wird sie zuvor aufgeschlossen. |
| Info (direkt) | Binär | Fragt den Status des Nuki SmartLocks und des Türsensors direkt am Gerät ab. Dies wirkt sich negativ auf die Batterielaufzeit der Geräte aus. |



### Parameter


| Bezeichnung | Porttyp | Beschreibung |
| --- | --- | --- |
| Gerät-ID | Text | Legt den Bezeichner des Geräts fest, an das die Anforderungen (Abschließen, Aufschließen, Entsperren, Statusaktualisierung) gesendet werden sollen. |
| Gerättyp | Ganzzahl | Angabe des Nuki-Gerättyps (siehe [Device Types](https://developer.nuki.io/page/nuki-bridge-http-api-1-13/4/#heading--device-types)). |



### Ausgänge


| Bezeichnung | Porttyp | Beschreibung |
| --- | --- | --- |
| Aufträge | Text | Sendet Auftragstelegramme, die der [Telegrammsyntax](/de/logic.bridges.nuki/doc/syntax) entsprechen. |

## Anwendung

### Beispiel: Türschloss


Die Steuerung der Wohnungstür mit Hilfe der Nuki Logikbausteine kann folgendermaßen aussehen:


![Nuki Bridge Logikbaustein](/content/logic.bridges.nuki/img/gatewayconnector--example01.png)
*Anwendungsbeispiel der Nuki Logikbausteine.*


Weitere Informationen sind auf [dieser Seite](/de/logic.bridges.nuki/gatewayconnector) zusammengefasst.

## Lizenzinformationen

- {{sectionLink path="/de/logic.bridges.nuki" section="lizenz" label="Lizenzbedingungen und rechtliche Hinweise für diesen Logikbaustein"}}
- [Input](https://icons8.com/icon/82745/input) icon by [Icons8](https://icons8.com)
