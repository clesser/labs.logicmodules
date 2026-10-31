---
title: Beobachter
pageLevel: module
moduleLogo: /content/logic.timing/img/observer--icons8-attendance-44.png
---

## Beschreibung

Dieser Baustein ist ein Beobachter u.a. für die Anwesenheitserkennung und mehrstufige Alarmierung.


Nachdem der Beobachter durch ein Ereignis aktiv wird, wartet er die eingestellte Nachlaufzeit ab und ignoriert in dieser Zeit noch alle Aktivitätssignale. Anschließend beginnen bis zu 15 aufeinanderfolgende Wartezeiten, deren Ende jeweils an einem Ausgang signalisiert wird. Geht während der Beobachtungszeit (= die einzelnen Wartezeiten) ein Aktivitätssignal ein, so wird die Beobachtung beendet und dies am Ausgang signalisiert.

## Eigenschaften

### Eingänge


| Bezeichnung | Porttyp | Beschreibung |
| --- | --- | --- |
| Beobachtungszyklus beginnen | Binär | Setzt alle Ausgänge zurück und beginnt mit einem neuen Beobachtungszyklus (Nachlaufzeit + Wartezeiten 1..15). |
| Aktivität melden | Binär | Meldet eine Aktivität (1 / true), die zum Abbruch des Beobachtungszyklus führt. |



### Parameter


| Bezeichnung | Porttyp | Beschreibung |
| --- | --- | --- |
| Nachlaufzeit | Zeitspanne | Definiert die Zeitspanne zu Beginn eines Beobachtungszyklus, in dem Aktivitätssignale ignoriert werden. |
| Beobachtungsschritte | Ganzzahl | Definiert 1..15 mögliche Beobachtungszeiten. Jeder Beobachtungsschritt wird durch eine Wartezeit (Parameter) und einen entsprechenden Auslöser (Ausgang) definiert. |
| x. Wartezeit | Zeitspanne | Definiert die Wartezeit eines Beobachtungsschritts. Geht während der Wartezeit kein Aktivitätssignal ein, so wird am Ende der Wartezeit eines Schritts, eine 1/true an den zugehörigen Ausgang geschickt. Die Wartezeiten bauen auf einander auf. Soll die erste Wartezeit 30 min nach Zyklusbeginn und die zweite Wartezeit nach 60 min nach Zyklusbeginn (der Einfachheit halber sei die Nachlaufzeit = 0) zu Ende sein, so wird für beide Schritte "30 min"" angegeben. |



### Ausgänge


| Bezeichnung | Porttyp | Beschreibung |
| --- | --- | --- |
| Laufender Beobachtungszyklus | Binär | Zeigt an, ob gerade ein Beobachtungszyklus aktiv ist. Der Beobachtungszyklus beginnt, nachdem eine 1/true an den gleichnamigen Eingang geschickt wird und endet, wenn entweder der letzte Beobachtungsschritt erreicht wurde, oder zwischenzeitlich ein Aktivitätssignal erkannt wurde, dass zum Abbruch des Zyklus führte. |
| x. Wartezeit abgelaufen | Binär | Zeigt mit einem "1"-Telegramm an, dass die Wartezeit des jeweiligen Schritts abgelaufen ist und zwischenzeitlich kein Aktivitätssignal eingegangen ist. |
| Aktivität erkannt | Binär | Zeigt mit einem "1"-Telegramm an, eine Aktivität erkannt wurde und damit ein laufender (bzw. abgelaufener) Zyklus abgebrochen wird. |

## Anwendung

### Beispiel: Anwesenheitserkennung


Dieser Anwendungsfall ist inspiriert von dem Baustein "Präsenz" von Sönke Jacobs. Er ist jedoch etwas universeller angelegt.


Im nachfolgenden Beispiel wird die Anwesenheitserkennung immer dann aktiviert, wenn die Wohnungstür geöffnet/geschlossen wird. Nach einer Nachlaufzeit, in der existierende Automatiken in den Ruhezustand gehen können, beginnen drei Überwachungszeiträume, denen unterschiedliche Aktionen bzw. Szenen zugewiesen (z.B. Außer Haus, Dienstreise, Urlaub) werden können.


Mit Hilfe einer Sperre kann die Aktivierung des Beobachtungszeitraums unterdrückt werden. Hierfür können ggf. Balkon- und Terrassentüren oder Szenen (Schlafen, Blumengießen im Urlaub) herangezogen werden.


![Beobachter - Szenen entsprechend der Abwesenheitsdauer schalten](/content/logic.timing/img/observer--example01.png)
*Dieses Beispiel nutzt Wertgeneratoren an den verschiedene Ausgängen, um mit Hilfe von Szenen in einem Szenenset die einzelnen Schaltzustände abzubilden. Die Sperre unterdrückt die Anwesenheitserkennung, wenn bestimmte Szenen (2, 3, 6) eingestellt wurden. Diese sperrenden Szenen werden nach spätestens 15h indirekt zurückgesetzt, in dem ein Beobachtungszyklus eingeleitet (und damit die Sperre überbrückt) wird. Nach ca. 15h (+ 5m) + 30m + 11h30m ist in jedem Fall der letzte Status ("Szene 5") erreicht, falls währenddessen keine Aktivität erkannt wurde.*


### Beispiel: Mehrstufige Alarmierung


Mit Hilfe dieser Logik lässt sich auch ein mehrstufiger Wachhund implementieren, bei dem nach der Aktivierung (= Beobachtungszyklus beginnen) auf ein weiteres Ereignis (= Aktivität melden) gewartet wird. Während die Wartezeit verstreicht, kann zu bestimmten Zeiten eine Aktion (z.B. Hinweis, Warnung, Voralarm, Alarm) ausgelöst werden.


![Beobachter - Mehrstufige Alarmierung](/content/logic.timing/img/observer--example02.png)
*In diesem Fall wird die Überwachung der Öffnungszeit eines Fensters bei steigender Flanke (= Öffnen) begonnen und bei fallender Flanke (= Schließen) beendet. In der Zwischenzeit werden nach dem Verstreichen bestimmter Zeitmarken eine Warnung und ein Alarm ausgelöst.*

## Lizenzinformationen

{{moduleLicenseLink path="/de/logic.timing#lizenz"}}
- [Attendance](https://icons8.com/icon/50896/attendance) icon by [Icons8](https://icons8.com)
