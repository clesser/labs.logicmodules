---
title: Wecker (Weckzeit)
pageLevel: module
moduleLogo: /content/logic.timing/img/wakeup--icons8-alarm-44.png
---

## Beschreibung

Dieser Baustein wählt aus mehreren Weckzeiten (z.B. von beruflichen, privaten und Familienkalendern) die nächstgelegene Zeit (abzüglich der Vorlaufzeit) aus.


Es kann zudem eine Standardzeit für Werktage bzw. Feiertage aktiviert werden, die in die Berechnung einbezogen wird. Bei den Standardweckzeiten wird keine Vorlaufzeit berücksichtigt.


Für die Bestimmung der Weckzeit wird der folgende Algorithmus verwendet:


- Wenn eine Standardweckzeit verwendet werden soll und diese auch eingestellt ist, wird in Abhängigkeit von der Werktagkonfiguration bzw. der Feiertage die Standardweckzeit bestimmt. Die Standardweckzeiten werden für den aktuellen und den darauffolgenden Tag bestimmt und in einer Liste vermerkt.
- Alle Termine werden abzüglich der Vorlaufzeiten ebenfalls in der internen Liste vermerkt.
- Der vom jetzigen Zeitpunkt aus gesehen nächste Zeitpunkt wird als Weckzeit am Ausgang des Logikbausteins ausgegeben.


Der Baustein aktualisiert den Ausgang "Weckzeit" nicht von selbst - d.h. von außen muss eine Aktualisierung angestoßen werden. Dies passiert aber bereits bei der Verwendung der Logikbausteine [Terminkalender](/de/logic.timing/calendar) und/oder [Feiertagskalender](/de/logic.timing/holiday), die nach Beginn eines Ereignisses den Ausgang "Nächstes Ereignis" aktualisieren und somit auch die Ergebnisse dieses Logikbausteins aktualisieren.

## Eigenschaften

### Eingänge


| Bezeichnung | Porttyp | Beschreibung |
| --- | --- | --- |
| xx. Termin | Datum und Zeit | Der nächste Termin auf einem Kalender. Die Weckzeit leitet sich von der Startzeit des Termins abzüglich der Vorlaufzeit ab. |
| xx. Feiertag | Datum und Zeit | Ein Feiertag. Fällt der aktuelle Tag auf diesen Feiertag, so wird die Standardweckzeit für Feiertage gewählt (falls kein Termin definiert ist). |
| Standardweckzeit verwenden | Binär | Gibt an, ob die Standardweckzeit bei der Ermittlung der Weckzeit einbezogen wird (z.B. für Werktage und Wochenenden). |
| Standardweckzeit (Werktag) | Zeit | Standardweckzeit für Werktage. Werktage werden im Logikbaustein konfiguriert. Bei der Standardweckzeit wird die Vorlaufzeit nicht abgezogen - d.h. sie wird direkt in die Berechnung einbezogen. |
| Standardweckzeit (Feiertag) | Zeit | Standardweckzeit. Als Feiertage zählen alle Wochentage, die nicht als Werktag konfiguriert sind oder als Feiertag hinterlegt sind. Bei der Standardweckzeit wird die Vorlaufzeit nicht abgezogen - d.h. sie wird direkt in die Berechnung einbezogen. |



### Parameter


| Bezeichnung | Porttyp | Beschreibung |
| --- | --- | --- |
| Termine | Ganzzahl | Es können 1..10 Termine definiert sein. In Kombination mit dem [Terminkalender](/de/logic.timing/calendar) kann aus bis zu 10 Kalendern der jeweils nächste Termin ausgewertet werden. |
| Vorlaufzeit | Zeit | Optionale Vorlaufzeit für eine Weckzeit (Weckzeit = Startzeit - Vorlaufzeit; 00:00:00 - 23:59:59). Die Vorlaufzeit kann genutzt werden, um beispielsweise Zeit für die Morgenroutine einzuplanen. Wenn also beispielsweise der erste Termin im Kalender um 08:00 Uhr beginnt und man eine Stunde vorher geweckt werden möchte, dann wird als Vorlaufzeit eine Stunde eingetragen. |
| Feiertage | Ganzzahl | Es können 0..25 Feiertage definiert sein. In Kombination mit dem [Feiertagskalender](../holiday/) kann aus bis zu 25 Kalendern der jeweils nächste Feiertag ausgewertet werden. |
| \{Montag..Sonntag\} ist ein Werktag | Binär | Mit diesen Schaltern werden die Werktage konfiguriert. |



### Ausgänge


| Bezeichnung | Porttyp | Beschreibung |
| --- | --- | --- |
| Weckzeit | Zeit | Ermittelt die früheste Weckzeit, indem es für jeden Kalender die Weckzeit (= Startzeit - Vorlaufzeit) ermittelt und dann den frühesten Zeitpunkt auswählt. Wird keine Weckzeit ermittelt, so liefert der Ausgang den Zeitpunkt "01.01.0001 00:00:00". |

## Anwendung

### Beispiel: Weckzeit ermitteln


Wenn die Tagesroutine zu unterschiedlichen Zeiten beginnt, die sich nicht auf Wochentage festlegen lassen (-> Zeitschaltuhr), bietet es sich an, die meist bereits vorhandenen Kalender auszulesen. Häufig existieren verschiedene Kalender parallel, sodass aus allen Kalendern gegebenenfalls der nächste bzw. früheste Termin ausgewählt werden muss.


Dieser Baustein nimmt von 1..10 Kalendern die Zeiten des jeweils nächsten Termins entgegen und bestimmt so den nächsten bzw. frühesten Termin, der dann an den Wecker weitergeleitet werden kann.


Da der Wecker zum Wecken aktiv eingeschaltet werden muss, können auch während des Tages jeweils die nächsten Termine an die Logikbausteine geschickt werden. Der Wecker würde nur dann auslösen, wenn er untertags aktiv geschaltet wird.


![Wecker (Weckzeit) - Terminkalender und Feiertagskalender auslesen](/content/logic.timing/img/wakeup--example01.png)
*Zunächst werden Termine aus einem Exchange-Online-Kalender ausgelesen und die Feiertage von einem Webdienst bereitgestellt. Um die Daten in verschiedenen Logikblättern zu verwenden, werden sie in X1-Variablen-Datenpunkten gespeichert.*


![Wecker (Weckzeit) - Terminkalender und Feiertagskalender verwenden](/content/logic.timing/img/wakeup--example02.png)
*In diesem Beispiel wird die Weckzeit aus einem Kalender bestimmt. Der Feiertagskalender und die Werktagskonfiguration unterstützen bei der Auswahl der Standardweckzeit.*


Standardweckzeiten können beispielsweise durch Szenen, Zeitschaltuhren (z.B. Wochentag, Wochenende) oder auch Eingabegeräte auf einen der Eingänge gelegt werden.


Damit ein Wecker auslöst, muss der Termin rechtzeitig übermittelt werden. Dauert beispielsweise die Wecksequenz des Weckers 30 min, die Vorlaufzeit des Weckzeit-Bausteins 45 min, so muss der Termin mindestens 1h 15min vor der eigentlichen Weckzeit durch den Terminkalender-Logikbaustein übermittelt worden sein.


Wird ein [Feiertagskalender](/de/logic.timing/holiday) verwendet und für den aktuellen Tag ein Feiertag ermittelt, so wird der nächste Feiertag erst kurz nach Mitternacht übermittelt und damit ggf. die Standardweckzeit aktualisiert.

## Lizenzinformationen

{{moduleLicenseLink path="/de/logic.timing#lizenz"}}
- [Alarm](https://icons8.com/icon/Cxb6rt2FZRtd/alarm) icon by [Icons8](https://icons8.com)
