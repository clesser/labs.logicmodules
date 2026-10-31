---
title: Smartlock
pageLevel: module
moduleLogo: /content/logic.bridges.nuki/img/smartlock--icons8-lock-44.png
---

## Beschreibung

Dieser Baustein ermöglicht das Abschließen (Sperren), Aufschließen (Entsperren) und Entriegeln eines Türschlosses mittels Nuki SmartLock und liefert Informationen über den Schließzustand, Türsensor und Ladestand.

Um diesen Baustein zu nutzen, wird eine Nuki Bridge benötigt, die im gleichen Netzwerk erreichbar ist. Zur Konfiguration sind die Parameter "token" der Nuki Bridge und "nukiId" (nachfolgend als "Geräte-ID" bezeichnet) des Nuki SmartLocks erforderlich. Die Einrichtung der Nuki Bridge und das Auslesen dieser Parameter ist auf dieser Webseite beschrieben:

1. [Ermitteln der Nuki Bridge IP-Adresse](https://developer.nuki.io/page/nuki-bridge-http-api-1-13/4/#heading--bridge-discovery) --> Die IP-Adresse der Nuki Bridge sollte statisch vergeben werden.
2. [Erzeugen eines Zugriffstokens](https://developer.nuki.io/page/nuki-bridge-http-api-1-13/4/#heading--token) --> Das Erzeugen muss innerhalb von 30sek an der Nuki Bridge bestätigt werden.
3. [Ermitteln der Nuki SmartLock Gerätenummer](https://developer.nuki.io/page/nuki-bridge-http-api-1-13/4/#heading--list) --> Falls mehrere Geräte angezeigt werden, ist die Nummer des Nuki SmartLocks (deviceType = 0 oder 4) relevant.


Nach jeder Aktion (Abschließen, Aufschließen, Entriegeln), die die Nuki Bridge als "erfolgreich" quittiert, wird der "Status: Schloss" entsprechend aktualisiert, ohne dass dabei der tatsächliche Zustand abgefragt wird.


In der Praxis führt dies zu zügigen Rückmeldungen (Visualisierung), die im Rahmen des nächsten Aktualisierungszyklus gegebenenfalls korrigiert werden. Dies betrifft insbesondere das Entsperren der Tür (Status = 5), der nach der eingestellten Sperrzeit auf "Nicht verschlossen" (Status = 3) wechselt, sich aber erst bei der nächsten Aktualisierung des Gerätestatus im X1 widerspiegelt.


**Hinweis**: Dieser Baustein eignet sich gut, um ein **einzelnes** SmartLock, das mit einer Bridge verbunden ist, zu steuern. Falls **mehrere** Geräte mit der gleichen Bridge verbunden sind, ist die Kombination von [Nuki Bridge](/de/logic.bridges.nuki/gatewayconnector), [Nuki Auftraggeber](/de/logic.bridges.nuki/gatewayrequest) und [Nuki Antwortfilter](/de/logic.bridges.nuki/gatewayresponse) vorzuziehen.

## Eigenschaften

### Eingänge


| Bezeichnung | Porttyp | Beschreibung |
| --- | --- | --- |
| Abschließen | Binär | Fordert zum Abschließen (Sperren) der Tür auf, wenn eine 1 übermittelt wird. |
| Aufschließen | Binär | Fordert zum Aufschließen (Entsperren) der Tür auf, wenn eine 1 übermittelt wird. Hinweis: Je nach Konfiguration des Nuki SmartLocks kann "Aufschließen" den Schlüssel genauso weit drehen wie beim "Entsperren". |
| Entsperren | Binär | Fordert zum Entsperren des Türriegels auf, wenn eine 1 gesendet wird. Falls die Tür abgeschlossen war, wird sie zuvor aufgeschlossen. |
| Info (direkt) | Binär | Fragt den Status des Nuki SmartLocks und des Türsensors direkt am Gerät ab. Dies wirkt sich negativ auf die Batterielaufzeit der Geräte aus. |
| Info (zwischengespeichert) | Binär | Fragt den Status des Nuki SmartLocks und des Türsensors an der Bridge ab. Dies wirkt sich positiv auf die Batterielaufzeit der Geräte aus. |



### Parameter


| Bezeichnung | Porttyp | Beschreibung |
| --- | --- | --- |
| Bridge IP-Adresse | Text | Legt die IPv4-Adresse der Nuki Bridge (z.B. "0.0.0.0") fest. Der Logikbaustein ergänzt intern den Port "8080". |
| Bridge Token | Text | Eingabe des Zugriffstokens. |
| Gerät-ID | Text | Legt den Bezeichner des Geräts fest, an das die Anforderungen (Abschließen, Aufschließen, Entsperren, Statusaktualisierung) gesendet werden sollen. |
| Gerättyp | Ganzzahl | Angabe des Nuki-Gerättyps (siehe [Device Types](https://developer.nuki.io/page/nuki-bridge-http-api-1-13/4/#heading--device-types)). |



### Ausgänge


| Bezeichnung | Porttyp | Beschreibung |
| --- | --- | --- |
| Status: Schloss | Ganzzahl | Liefert den Status des Nuki SmartLocks (siehe [Lock State](https://developer.nuki.io/page/nuki-bridge-http-api-1-13/4#heading--lock-states)). |
| Status: Tür | Ganzzahl | Liefert den Status des Nuki DoorSensors (siehe [DoorSensor State](https://developer.nuki.io/page/nuki-bridge-http-api-1-13/4#heading--doorsensor-states)). |
| Status: Batterie | Prozent | Liefert den Ladestand des Nuki SmartLocks (0..100%). |
| Fehler | Text | Liefert eventuelle Netzwerk-Fehlermeldungen aus der Kommunikation mit der Nuki Bridge. |

## Anwendung

### Beispiel: Türschloss


Die Steuerung der Wohnungstür mit Hilfe eines Nuki SmartLocks kann folgendermaßen aussehen:


![Nuki SmartLock Logikbaustein](/content/logic.bridges.nuki/img/smartlock--example01.png)
*Anwendungsbeispiel des Nuki SmartLocks Logikbausteins.*


Gesteuert wird das Schloss mit Hilfe von drei Eingängen, über die beispielsweise das Schloss mit Tastern oder im Rahmen einer Szene abgeschlossen, aufgeschlossen oder entriegelt werden kann. Hierzu ein paar Empfehlungen für die Konfiguration der Datenpunkte:


- Gruppenadress-Datenpunkte. Es werden zwei Gruppenadressen und zwei Datenpunkte benötigt. Auf Datenpunkt 1 wird die Gruppenadresse 1 (Senden) und Gruppenadresse 2 (Status) gelegt, auf den Datenpunkt 2 die Gruppenadresse 2 (Senden). Datenpunkt 1 wird nun zum Schalten des Nuki SmartLocks (mit Hilfe eines Flankendetektors oder Filters) gelegt und der Datenpunkt 2 hinter dem "Vergleicher+", um den Schaltzustand zu erfassen. Für die Visualisierung kann ein Schalter gewählt werden.
- Variablen-Datenpunkte. Hierfür empfiehlt es sich, drei Datenpunkte anzulegen: Datenpunkt 1 zum Abschließen, Datenpunkt 2 zum Aufschließen und Datenpunkt 3 für die Statusanzeige nach dem "Vergleicher+". Die Visualisierung kann derzeit nur mit Hilfe von Ein/Aus-Tastern und einer Binäranzeige gestaltet werden. Falls das Schloss ausschließlich über Szenen abgeschlossen bzw. aufgeschlossen wird (und daher keine Schaltervisualisierung erforderlich ist), kann ein Datenpunkt mit dem Flankendetektor bzw. Filter (analog zu den Gruppenadress-Datenpunkten) genutzt werden (siehe Abbildung).
- Für das Entriegeln bietet sich ein beliebiger Datenpunkt an, der über einen Ein/Aus-Taster visualisiert werden kann.


Der Telegrammgenerator sorgt dafür, dass der Zustand (incl. Ladestand der Batterie) regelmäßig von der Nuki Bridge ausgelesen wird. Dafür sind zwei Eingänge vorgesehen:


- *Info (direkt)* liest den Status des Gerätes direkt aus dem Gerät aus. Werden die Statusabfragen häufig durchgeführt, wirkt sich das negativ auf die Lebensdauer von Batterie / Akku aus.
- *Info (zwischengespeichert)* liest zwischengespeicherten Status des Geräts an der Bridge aus. In dem Fall wird die Batterie des Geräts geschont.


Auf der Seite der Ausgänge werden die vom Nuki SmartLock definierten Zustände ausgegeben. Mit dem "Vergleicher+"-Logikbaustein werden die Werte ausgewertet (Abgeschlossen/Aufgeschlossen/Störung bzw. Geöffnet/Geschlossen/Störung). Der Ladestand wird direkt weitergereicht. Mit einem Vergleicher könnte hier ein Alarm bei kritischem Ladestand realisiert werden.


Ein klassischer Türsensor erkennt, wenn die Tür geöffnet wird, und setzt den Zustand des Schlosses auf "Offen" (Filterbaustein in der letzten Zeile). Dadurch wird dieser Status auch außerhalb eines Updatezyklus umgehend bereitgestellt.

## Lizenzinformationen

- {{sectionLink path="/de/logic.bridges.nuki" section="lizenz" label="Lizenzbedingungen und rechtliche Hinweise für diesen Logikbaustein"}}
- [Lock](https://icons8.com/icon/15437/lock) icon by [Icons8](https://icons8.com)
