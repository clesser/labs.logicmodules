---
title: [neleo.labs] X1 Nuki Bridge Bausteine
---

## Beschreibung

Die Kommunikation der Logikbausteine untereinander erfolgt mit Hilfe von Telegrammen, die einer URL-Syntax nachempfunden sind:


- request:///\{lock|unlock|unlatch|info\}
- response:///\{lock|unlock|unlatch|info\}?param1=value1


Die [Nuki Auftraggeber](/de/logic.bridges.nuki/gatewayrequest)-Bausteine generieren "request://"-Telegramme, die an den Eingang der [Nuki Bridge](/de/logic.bridges.nuki/gatewayconnector) gesendet werden. Diese antwortet mit "response://"-Telegrammen, die es über seinen Ausgang an die Eingänge der [Nuki Antwortfilter](/de/logic.bridges.nuki/gatewayresponse)-Bausteine sendet ("Broadcast").


Jedes Telegramm definiert die Nuki-ID (*device-id*), den Nuki-Gerätetyp (*device-type*) und eine Aktion (z.B. *lock*, *info*).


Bei Antworten schließt sich nun die optionale Parameterliste an. Sie beginnt mit dem Fragezeichen ("?") und dem ersten Schlüssel-Wert-Paar. Weitere Schlüssel-Wert-Paare werden durch ein kaufmännisches Und ("&") angehängt.


In der Regel ist das manuelle Zusammenstellen der Telegramme nicht erforderlich - jedoch können damit gegebenenfalls auch noch nicht offiziell getestete Geräte angebunden werden.

## Eigenschaften

### Protokoll


| Bezeichnung | Beschreibung |
| --- | --- |
| request:// | Das Telegramm richtet sich an den Eingang der [Nuki Bridge](/de/logic.bridges.nuki/gatewayconnector). Es wird verwendet, um Eigenschaften in Geräten abzufragen oder Aktionen auszulösen. |
| response:// | Das Telegramm wird vom Ausgang der [Nuki Bridge](/de/logic.bridges.nuki/gatewayconnector) versendet. Es zeigt Veränderungen der Eigenschaften von Geräten an. |



### Bezeichner


| Bezeichnung | Protokolle | Beschreibung |
| --- | --- | --- |
| device-id | request{{br}}response | Der eindeutige Bezeichner eines Geräts ("nukiID"). Die Bezeichner aller mit einer Bridge gekoppelten Geräte können mit Hilfe des [/list](https://developer.nuki.io/page/nuki-bridge-http-api-1-13/4/#heading--list)-Befehls ermittelt werden. |
| device-type | request{{br}}response | Der Typ eines Geräts ("deviceType"). Die Bezeichner aller mit einer Bridge gekoppelten Geräte können mit Hilfe des [/list](https://developer.nuki.io/page/nuki-bridge-http-api-1-13/4/#heading--list)-Befehls ermittelt werden. |



### Aktionen


| Bezeichnung | Protokolle | Beschreibung |
| --- | --- | --- |
| /lock | request{{br}}response | Fordert das angegebene Nuki Gerät auf, die Aktion "2" (= "lock", siehe [Lock Actions](https://developer.nuki.io/page/nuki-bridge-http-api-1-13/4/#heading--lock-actions)) durchzuführen. Eine Antwort meldet die erfolgreiche bzw. fehlgeschlagene Ausführung der Aktion. |
| /unlock | request{{br}}response | Fordert das angegebene Nuki Gerät auf, die Aktion "1" (= "unlock", siehe [Lock Actions](https://developer.nuki.io/page/nuki-bridge-http-api-1-13/4/#heading--lock-actions)) durchzuführen. Eine Antwort meldet die erfolgreiche bzw. fehlgeschlagene Ausführung der Aktion. |
| /unlatch | request{{br}}response | Fordert das angegebene Nuki Gerät auf, die Aktion "3" (= "unlatch", siehe [Lock Actions](https://developer.nuki.io/page/nuki-bridge-http-api-1-13/4/#heading--lock-actions)) durchzuführen. Eine Antwort meldet die erfolgreiche bzw. fehlgeschlagene Ausführung der Aktion. |
| /info | request{{br}}response | Fordert das angegebene Nuki Gerät auf, den Status des Geräts sowie ggf. des Türsensors und der Batterie zu melden. Eine Antwort meldet den Status des Geräts, des Türsensors und der Batterie. |



### Parameter


| Bezeichnung | Protokolle | Beschreibung |
| --- | --- | --- |
| success | response | Definiert, ob eine Aktion erfolgreich war: *true* *false* |
| lock | response | Liefert den Status des Geräts (typischerweise ein Smart Lock): *1* - verschlossen *3* - unverschlossen *5* - entsperrt Siehe auch: [Liste aller Nuki Lock States](https://developer.nuki.io/page/nuki-bridge-http-api-1-13/4/#heading--lock-states) |
| door | response | Liefert den Status des Türsensors: *2* - geschlossen *3* - geöffnet Siehe auch: [Liste aller Nuki Door Sensor States](https://developer.nuki.io/page/nuki-bridge-http-api-1-13/4/#heading--doorsensor-states) |
| battery | response | Prozentualer Ladezustand der Batterie bzw. des Akkus des Nuki Geräts. |

## Anwendung

Die nachfolgenden Beispiele gehen von einem Nuki Smartlock der zweiten Generation (device-type=0) mit dem Bezeichner "13579" (device-id=13579) aus:


request://13579:0/lockSchließt die Tür ab.


response://13579:0/lock?success=trueBestätigt, dass die Tür abgeschlossen wurde.


request://13579:0/infoFordert den aktuellen Gerätestatus an.


response://13579:0/info?lock=3&door=2&battery=50Teilt mit, dass die Tür unverschlossen (lock=3), jedoch auch nicht geöffnet (door=2) ist und dass noch 50% Kapazität in der Batterie bzw. dem Akku vorhanden sind.
