---
title: Sperre+
pageLevel: module
moduleLogo: /content/logic.control/img/blocker--GiraIconLogicNodesBlocker.png
---

## Beschreibung

Dieser Baustein ist eine Erweiterung des Bausteins "Sperre". Es gibt eine zusätzliche Option, einen Wert zu senden, wenn eine Sperre beendet wird.


Häufig verändern Logiken im Automatikbetrieb Werte, die durch eine Sperre nicht in jedem Fall das Ziel erreichen sollen. Wenn die Sperre aufgehoben wird, soll jedoch der letzte Wert an das Ziel geschickt werden. Dieser Baustein ermöglicht nicht nur das Senden fest eingestellter Werte bei der Aktivierung oder Deaktivierung der Sperre, sondern auch das Senden des jeweils zuletzt eingegangenen Werts beim Aufheben der Sperre.

## Eigenschaften

### Eingänge


| Bezeichnung | Porttyp | Beschreibung |
| --- | --- | --- |
| Eingang | (Wert) | Eingang der Sperre |
| Aktiv | Binär | Steuert das Verhalten des Bausteins: 1 = alle Telegramme werden gesperrt0 = alle Telegramme werden weitergeleitet Standardwert = 0 |



### Parameter


| Bezeichnung | Porttyp | Beschreibung |
| --- | --- | --- |
| Sende bei Aktivierung | Auswahl | Gibt an, ob ein Telegramm bei einer Aktivierung des Bausteins versendet werden soll. *Kein Wert* = Sendet nichts*Definierter Wert* = Sendet einen fest definierten Wert Standardwert = Kein Wert |
| Wert bei Aktivierung | (Wert) | Gibt an, welches Telegramm versendet werden soll, wenn der Baustein aktiviert wird. |
| Sende bei Deaktivierung | Auswahl | Gibt an, ob ein Telegramm bei einer Deaktivierung des Bausteins versendet werden soll. *Kein Wert* = Sendet nichts*Definierter Wert* = Sendet einen fest definierten Wert*Letzter Wert* = Sende den zuletzt eingegangenen Wert Standardwert = Kein Wert |
| Wert bei Deaktivierung | (Wert) | Gibt an, welches Telegramm versendet werden soll, wenn der Baustein deaktiviert wird. |



### Ausgänge


| Bezeichnung | Porttyp | Beschreibung |
| --- | --- | --- |
| Ausgang | (Wert) | Ausgang der Sperre |

## Anwendung

### Beispiel: Einfache Rollladensteuerung


Als Beispiel sei eine einfache Rollladensteuerung genannt. Grundsätzlich fahren die Rollladen helligkeitsgesteuert (Tag/Nacht) automatisch. Jedoch möchte ich diese Automatik unterbrechen, wenn Gäste da sind (Sperre = ein). Wenn die Gäste wieder zu Hause sind, soll die Automatik weiter ausgeführt werden (Sperre = aus).


Da ich im Vorfeld nicht weiß, ob der Rollladen nach Ende der Sperre auf- oder abgefahren werden muss (Gäste können übernachten oder auch nicht :-) ), scheiden feste Werte bei der Deaktivierung aus. Wird jedoch der zuletzt eingegangene Wert (Auf- oder Abfahren) ausgegeben, wird der durch die Automatik vorgegebene Zustand wiederhergestellt.


![Sperre+ - Einfache Rollladensteuerung 1](/content/logic.control/img/blocker--example01.png)
*In diesem Fall wird der Rollladen herabgefahren, wenn die Nacht anbricht und wenn die Automatiksteuerung (Sperre) dies zulässt. Wird die Automatik (z.B. nachdem der Besuch wieder weg ist) wieder aktiviert, so wird der Rollladen auf die gewünschte (durch die Automatik vorgegebene) Position eingestellt.*


![Sperre+ - Einfache Rollladensteuerung 2](/content/logic.control/img/blocker--example02.png)
*Der Ablauf dieser Logik ist mit der vorherigen vergleichbar - es wird jedoch zudem ein Fenster überwacht. Wird dieses geöffnet, so fährt der Rollladen nach oben (Sende "auffahren" bei Aktivierung). Wenn das Fenster geschlossen wird, wird die Sperre deaktiviert und der ggf durch die Automatik bestimmte Wert übermittelt.*



### Beispiel: Farbtemperaturnachführung


Mitunter sollen im Tagesverlauf Farbtemperaturen zentral für verschiedene Leuchten vorgegeben werden. Durch eine Sperre kann diese Automatik gesteuert werden. In einigen Fällen verwirft der LED-Treiber jedoch die Informationen für die Farbtemperatur, wenn die Leuchte nicht eingeschaltet ist.


Um den jeweils gültigen Farbtemperaturwert einzustellen, kann die Sperre auch mit dem Schaltzustand der Leuchte verbunden werden. Ist die Leuchte ausgeschaltet, werden keine Informationen an die Leuchte geschickt (sie würde sie ja ohnehin ignorieren). Wird die Leuchte eingeschaltet, so wird mit einer Verzögerung der zuletzt gespeicherte Farbtemperaturwert an die Leuchte geschickt und somit die Leuchte korrekt justiert.


![Sperre+ - Farbtemperaturnachführung](/content/logic.control/img/blocker--example03.png)
*Farbtemperaturnachführung mit zwei Sperren. Die erste Sperre verhindert Farbtemperaturnachführungen, wenn dies nicht automatisch geschehen soll. In Kombination mit der Ein-/Ausschaltverzögerung ist die zweite Sperre dafür verantwortlich, die Farbtemperatur beim Einschalten der Leuchte zu senden. Die beiden Dimmer werden durch zwei sonnenstandsabhängige (Sonnenaufgang/Sonnenuntergang) Zeitschaltuhren in Gang gesetzt.*


:::warning
Das gezeigte Beispiel verwendet den Logikbaustein DimCurve von Sönke Jacobs ([Link zum Download im knx-user-forum.de](http://service.knx-user-forum.de/?comm=download&id=20000030)).
:::

## Lizenzinformationen

{{moduleLicenseLink path="/de/logic.control#lizenz"}}
- Das Icon wurde der Komponente "Sperre" aus dem Gira Projekt Assistent entnommen.
