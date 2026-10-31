---
title: Wecker
pageLevel: module
moduleLogo: /content/logic.timing/img/alarm--icons8-alarm-44.png
---

## Beschreibung

Dieser Baustein ist ein Wecker, der vor der eigentlichen Weckzeit bereits Telegramme generieren kann. Damit können beispielsweise Sonnenaufgangswecker simuliert werden, oder Geräte nach und nach eingeschaltet werden.


Alternativ steht auch ein Auslöse-Modus zur Verfügung. Dabei wird eine klassische Zeitschaltuhr zum Auslösen verwendet und die Dauer der Wecksequenz von der Weckzeit abgezogen (Weckzeit: 08:00 Uhr, Sequenzdauer: 30 min ==> Auslösen um 07:30 Uhr).

## Eigenschaften

### Eingänge


| Bezeichnung | Porttyp | Beschreibung |
| --- | --- | --- |
| Weckzeit (einmalig) | Datum und Zeit | Startet die Wecksequenz so, dass der Alarm zur angegebenen Zeit am definierten Tag ausgelöst wird. Der Wecker wird nur an dem festgelegten Tag (und Zeit) ausgelöst. |
| Weckzeit (täglich) | Zeit | Startet die Wecksequenz so, dass der Alarm zur angegebenen Zeit ausgelöst wird. Der Wecker stellt sich jeden Tag neu auf die angegebene Zeit ein. |
| Auslöser | Binär | Startet die Wecksequenz, wenn eine 1 empfangen wird |
| Schlummern / Aus | Binär | Steuert das Verhalten des Bausteins: 0 = wechselt (von einem Sequenzschritt oder dem Alarm) in den Schlummerzustand; 1 = beendet die Wecksequenz und verlässt den Alarm- oder Schlummerzustand |
| Wecker aktiv | Binär | Steuert das Verhalten des Bausteins: 0 = Wecksequenzen werden nicht gestartet und bereits laufende werden abgebrochen; 1 = Wecksequenzen können automatisch (Betriebsmodus: Weckzeit) oder durch Auslösen (Betriebsmodus: Auslöser) gestartet werden |



### Parameter


| Bezeichnung | Porttyp | Beschreibung |
| --- | --- | --- |
| Betriebsmodus | Auswahl | Der Betriebsmodus des Weckers gibt an, wie die Wecksequenz ausgelöst wird. *Weckzeit (einmalig)* = zu einer festgelegten Zeit an dem festgelegten Datum*Weckzeit (täglich)* = zu einer festgelegten Zeit an jedem Tag*Auslöser* = durch ein Telegram Standardwert = Auslöser |
| Schritte | Ganzzahl | Gibt die Anzahl der Schritte (0..16) an, die vor dem Alarm ausgeführt werden sollen. Jeder Schritt stellt eine Schrittdauer und einen Auslöser zur Verfügung. |
| x. Verzögerung | Zeitspanne | Definiert die Dauer eines Schrittes (15 sek .. 30 min). Die Verzögerung (= Dauer eines Schritts) wird für jeden Schritt absolut (und damit nicht relativ zur Weckzeit!) definiert. |
| Schlummerzeit | Zeitspanne | Definiert die Dauer der Schlummerzeit (15 sek .. 30 min). Nachdem der Weckalarm ausgelöst wurde, kann in den Schlummermodus gewechselt werden (Telegramm "0" an "Schlummern / Aus"). Nach Ablauf der Schlummerzeit wird der Weckalarm erneut ausgelöst. |



### Ausgänge


| Bezeichnung | Porttyp | Beschreibung |
| --- | --- | --- |
| Wecken aktiv | Binär | Zeigt an, ob die Wecksequenz gerade läuft. Der Ausgang zeigt eine "1" an, wenn der erste Schritt beginnt und bleibt auch während des Alarm- und Schlummermodus auf "1". Wenn die Wecksequenz mit einem "1"-Telegramm auf "Schlummern / Aus" beendet wird, fällt dieser Ausgang zurück auf "0". |
| x. Schritt | Binär | Zeigt mit einem "1"-Telegramm an, dass der Schritt aktiv ist und fällt anschließend wieder auf "0" zurück. |
| Weckalarm | Binär | Zeigt mit einem "1"-Telegramm an, dass der Weckalarm ausgelöst wurde. Es wird ein "0"-Telegramm gesendet, wenn der Schlummermodus aktiv wird oder die Wecksequenz beendet wird. |
| Schlummern | Binär | Zeigt mit einem "1"-Telegramm an, dass der Schlummermodus aktiv ist. Es wird ein "0"-Telegramm gesendet, wenn der Schlummermodus verlassen wird (z.B. weil der Weckalarm erneut ausgelöst wird oder die Wecksequenz beendet wurde). |
| Wecker aktiv | Binär | Der Ausgang "Wecker aktiv" gibt darüber Auskunft, ob für die nächsten 24h eine Weckzeit definiert wurde (Betriebsmodus "Weckzeit") und ob der Wecker eingeschaltet ist ("Wecker aktiv" = wahr). Der Ausgang wird stündlich aktualisiert, sofern eine Wertänderung für die kommenden 24h eingetreten ist. Dies passiert beispielsweise, wenn am Freitag die nächste Weckzeit für den Montag eingetragen ist. In dem Fall zeigt der Ausgang erst am Sonntag einen aktivierten Wecker (für Montag) an. Im Betriebsmodus "Auslöser" entspricht der Ausgabewert dem Eingangswert "Wecker aktiv". |

## Anwendung

### Beispiel: Wecker mit verschiedenen Schaltpunkten


Wenn Menschen jeden Tag zur gleichen Zeit aufstehen, so werden häufig Zeitschaltuhren eingesetzt, um Abläufe zu automatisieren. Verschiebt sich der Tagesablauf jedoch (z.B. aufgrund unregelmäßiger Reisetätigkeit), entsteht schnell der Wunsch nach einem einzelnen Wecker, von dem sich alle Schaltzeitpunkte ableiten.


Im Rahmen einer Wecksequenz können verschiedene Schaltzeitpunkte definiert werden, um beispielsweise einen Sonnenaufgang zu simulieren, die Warmwasserbereitung einzuschalten oder auch die Unterhaltungselektronik oder Kaffeemaschine einzuschalten. Typischerweise liegen diese Aktionen bereits vor der eigentlichen Weckzeit.


Der Wecker ermittelt also die Dauer aller Schritte vor der eigentlichen Weckzeit und beginnt mit der Wecksequenz zum errechneten Zeitpunkt, so dass nach dem Auslösen aller Schritte der eigentliche Alarm zur eingestellten Zeit ausgelöst wird. Eine Schlummerfunktion komplettiert den Wecker.


![Wecker - Szenenbasierte Wecksequenz](/content/logic.timing/img/alarm--example01.png)
*Dieses Beispiel nutzt Wertgeneratoren an den verschiedene Ausgänge, um mit Hilfe von Szenen in einem Szenenset die einzelnen Schaltzustände abzubilden.*


### Beispiel: Weckzeiten nutzen


Ein Beispiel unter Verwendung des Betriebsmodus "Weckzeit" wird in Kombination mit dem Logikbaustein [Wecker (Weckzeit)](/de/logic.timing/wakeup) beschrieben.

## Lizenzinformationen

{{moduleLicenseLink path="/de/logic.timing#lizenz"}}
- [Alarm](https://icons8.com/icon/Cxb6rt2FZRtd/alarm) icon by [Icons8](https://icons8.com)
