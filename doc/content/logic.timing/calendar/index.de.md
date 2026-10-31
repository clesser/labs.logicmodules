---
title: Terminkalender
pageLevel: module
moduleLogo: /content/logic.timing/img/calendar--icons8-today-44.png
---

## Beschreibung

Dieser Baustein liest einen als iCal / ics formatierten Kalender ein und stellt Informationen zum jeweils nächsten Termin dar. Damit können Wecker gesteuert oder Termine auf grafischen Anzeigen visualisiert werden.


Wiederkehrende Termine (Serientermine) werden derzeit noch **nicht** vollständig unterstützt; während Serientermine selbst noch nicht einbezogen werden, werden die von der Serie abweichenden Termine berücksichtigt.

## Eigenschaften

### Eingänge


| Bezeichnung | Porttyp | Beschreibung |
| --- | --- | --- |
| iCal Kalender | Zeit | Eingang, an den ein iCal / ics Kalender übermittelt wird. Da die Auswertung großer Kalender auf dem X1 recht viel Last erzeugen kann, sollten die Aktualisierungsintervalle eher großzügig gewählt werden und Lastzeiten (z.B. volle Stunden) gemieden werden. Gute Erfahrungen wurden mit einer Aktualisierungszeit von z.B. 2h 22min erzielt. |



### Parameter


| Bezeichnung | Porttyp | Beschreibung |
| --- | --- | --- |
| Einschließende Parameter | Ganzzahl | Um die Auswahl der Termine eines Tages zu filtern, können Termine mittels einschließender Parameter gefiltert werden. Ein Termin wird dann angezeigt, wenn **alle** 0..10 einschließenden Parameter erfüllt sind. Die Filterung erfolgt mittels "String.StartsWith(.. OrdinalIgnoreCase)", sodass auch selbst gewählte Prefixe erfasst werden können. |
| xx. Einschließender Parameter | Ganzzahl | Ein einschließender Parameter. Sollen beispielsweise bei einem von Microsoft Exchange erzeugten iCal-Kalender nur Termine berücksichtigt werden, die als "gebucht" klassifiziert sind, so würde als Parameter "X-MICROSOFT-CDO-BUSYSTATUS:BUSY" eingetragen werden. |
| Ausschließende Parameter | Ganzzahl | Um die Auswahl der Termine eines Tages zu filtern, können Termine mittels ausschließender Parameter gefiltert werden. Ein Termin wird dann angezeigt, wenn **keiner** der 0..10 ausschließenden Parameter erfüllt ist. Die Filterung erfolgt mittels "String.StartsWith(.. OrdinalIgnoreCase)", sodass auch selbst gewählte Prefixe erfasst werden können. |
| xx. Ausschließender Parameter | Ganzzahl | Ein ausschließender Parameter. Sollen beispielsweise in einem Kalender alle Buchungen ausgeblendet werden, die "Focus Time" heißen, so würde als Parameter "SUMMARY:Focus Time" eingetragen werden. |
| Aktualisieren nach | Zeitspanne | Definiert die Zeit nach Beginn des aktuellen Termins (0 sek .. 15 min), nachdem der nächste Termin an den Ausgangsports angekündigt wird. |
| Formatvorlage | Text | Erlaubt die Formatierung des Ausgabetextes mit Platzhaltern "\{x\}" für den nächsten Termin. Dafür stehen die folgenden Werte zur Verfügung: *0* [DateTime] Startzeit des nächsten Termins. *1* [TimeSpan] Dauer des nächsten Termins. *2* [String] Betreff/Titel des nächsten Termins. *3* [String] Ort des nächsten Termins. Wird keine Formatierung angegeben, so bleibt die Ausgabe "Zusamenfassung" leer! Formatierungsoptionen werden hier erläutert: [Formatierungsoptionen für Datum und Zeit](https://learn.microsoft.com/de-de/dotnet/standard/base-types/standard-date-and-time-format-strings) bzw. [Formatierungsoptionen für Zeitspannen](https://learn.microsoft.com/de-de/dotnet/standard/base-types/standard-timespan-format-strings). |
| Text "Keine Eregnisse" | Text | Text, der am Ausgang "Zusammenfassung" ausgegeben wird, wenn im Kalender für die nächsten sechs Monate kein Ereignis eingetragen ist. |



### Ausgänge


| Bezeichnung | Porttyp | Beschreibung |
| --- | --- | --- |
| Nächstes Ereignis | Zeit | Gibt die Startzeit des nächsten Termins zurück. Der jeweils nächste Termin wird nach Ablauf der Zeit "Aktualisieren nach" nach Beginn des aktuellen Termins angezeigt. Liegt für die nächsten sieben Tage kein Termin an, so wird der Standardzeitpunkt "01.01.0001 00:00:00" zurückgegeben. Der Parameter "Zusammenfassung" wird immer unmittelbar **vor** diesem Ausgang aktualisiert. |
| Zusamenfassung | Zeit | Liefert eine Zusammenfassung des nächsten Termins gemäß der in "Formatvorlage" eingestellten Formatierung. Liegt für nächsten sieben Tage kein Termin an, kann ein definierter Text ("Text "Keine Ereignisse"") ausgegeben werden. Der Parameter "Nächstes Ereignis" wird immer unmittelbar **nach** diesem Ausgang aktualisiert. |

## Anwendung

### Beispiel: Weckzeit ermitteln


Wenn die Tagesroutine zu unterschiedlichen Zeiten beginnt, die sich nicht auf Wochentage festlegen lassen (-> Zeitschaltuhr), bietet es sich an, die meist bereits vorhandenen Kalender auszulesen. Häufig existieren verschiedene Kalender parallel, sodass aus allen Kalendern gegebenenfalls der nächste bzw. früheste Termin ausgewählt werden muss.


Dieser Baustein nimmt von 1..10 Kalendern die Zeiten des jeweils nächsten Termins entgegen und bestimmt so den nächsten bzw. frühesten Termin, der dann an den Wecker weitergeleitet werden kann.


Da der Wecker zum Wecken aktiv eingeschaltet werden muss, können auch während des Tages jeweils die nächsten Termine an die Logikbausteine geschickt werden. Der Wecker würde nur dann auslösen, wenn er untertags aktiv geschaltet wird.


![Wecker (Weckzeit) - Terminkalender und Feiertagskalender auslesen](/content/logic.timing/img/calendar--example01.png)
*Zunächst werden Termine aus einem Exchange-Online-Kalender ausgelesen und die Feiertage von einem Webdienst bereitgestellt. Um die Daten in verschiedenen Logikblättern zu verwenden, werden sie in X1-Variablen-Datenpunkten gespeichert.*


![Wecker (Weckzeit) - Terminkalender und Feiertagskalender verwenden](/content/logic.timing/img/wakeup--example02.png)
*In diesem Beispiel wird die Weckzeit aus einem Kalender bestimmt. Der Feiertagskalender und die Werktagskonfiguration unterstützen bei der Auswahl der Standardweckzeit.*


Standardweckzeiten können beispielsweise durch Szenen, Zeitschaltuhren (z.B. Wochentag, Wochenende) oder auch Eingabegeräte auf einen der Eingänge gelegt werden.


Damit ein Wecker auslöst, muss der Termin rechtzeitig übermittelt werden. Dauert beispielsweise die Wecksequenz des Weckers 30 min, die Vorlaufzeit des Weckzeit-Bausteins 45 min, so muss der Termin mindestens 1h 15min vor der eigentlichen Weckzeit durch den Terminkalender-Logikbaustein übermittelt worden sein.


Wird ein [Feiertagskalender](/de/logic.timing/holiday) verwendet und für den aktuellen Tag ein Feiertag ermittelt, so wird der nächste Feiertag erst kurz nach Mitternacht übermittelt und damit ggf. die Standardweckzeit aktualisiert.

## Lizenzinformationen

{{moduleLicenseLink path="/de/logic.timing#lizenz"}}
- Teilweise Übernahme des iCal.NET Parsers von [GitHub / der-daniel](https://github.com/der-daniel/ical.NET)
- [Today](https://icons8.com/icon/nGCq83WiIaj1/today) icon by [Icons8](https://icons8.com)
