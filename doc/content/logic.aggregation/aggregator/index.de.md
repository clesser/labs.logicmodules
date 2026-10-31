---
title: Wertesammler
pageLevel: module
moduleLogo: /content/logic.aggregation/img/aggregator--icons8-bar-chart-44.png
---

## Beschreibung

Dieser Baustein sammelt tage- und stundenweise Daten und liefert statistische Auswertungen darüber.


Der Hauptanwendungsfall ist die stündliche Protokollierung von Daten (z.B. der Temperatur) über einen Zeitraum von 1-14 Tagen hinweg, sodass daraus Schaltvorgänge (z.B. Heizperiode beginnen/beenden) abgeleitet werden können.


Es wird jeweils der erste pro Stunde eingehende Wert intern gespeichert. Für einen Tag stehen demnach intern bis zu 24 Werte zur Verfügung. Da die Rohdaten gespeichert werden, kann die Auswertungsmethode später geändert werden, ohne dass bereits erfasste Daten ungültig werden.


:::warning
Das Entfernen des Logikbausteins von einem Logikblatt, das Rücksetzen des X1/L1 oder das Löschen der Applikationsdaten bei der Inbetriebnahme führt dazu, dass der interne Wertspeicher gelöscht wird!
:::

## Eigenschaften

### Eingänge


| Bezeichnung | Porttyp | Beschreibung |
| --- | --- | --- |
| Eingang | Zahl | Eingang des Wertesammlers |



### Parameter


| Bezeichnung | Porttyp | Beschreibung |
| --- | --- | --- |
| Auswertungsmethode | Auswahl | Gibt die Berechnungsvorschrift an, mit der die gesammelten Werte zusammengefasst werden sollen. *Durchschnitt* = Es wird das [arithmetische Mittel](https://de.wikipedia.org/wiki/Arithmetisches_Mittel) über alle verfügbaren Werte des Zeitraums gebildet.*Median* = Es wird der [Median](https://de.wikipedia.org/wiki/Median) über alle verfügbaren Werte des Zeitraums gewählt.*Modus* = Es wird der [Modus](https://de.wikipedia.org/wiki/Modus_(Statistik)) aus den (auf ganze Zahlen gerundeten) Werten des Zeitraums gewählt. Standardwert = Durchschnitt |
| Auswertungszeitraum | Ganzzahl | Gibt die Anzahl der Tage (1..14) in der Vergangenheit an, auf die sich die Auswertungen beziehen. Die Werte stehen auch nach einem Neustart des X1/L1 zur Verfügung - jedoch werden alle Berechnungen nur auf die verfügbaren Tage angewendet. Wenn also 5 Tage berücksichtigt werden sollen, aber erst Daten für 4 Tage vorliegen, so werden auch nur 4 Tage ausgewertet. |



### Ausgänge


| Bezeichnung | Porttyp | Beschreibung |
| --- | --- | --- |
| Maximum (Auswertungszeitraum) | Zahl | Liefert den maximalen Wert aller zwischengespeicherten Werte des gesamten Auswertungszeitraums. |
| Durchschnitt (Auswertungszeitraum) | Zahl | Liefert das arithmetische Mittel (Durchschnitt / Mittelwert) aller zwischengespeicherten Werte des gesamten Auswertungszeitraums. |
| Durchschnitt (Heute-x) | Zahl | Liefert das arithmetische Mittel (Durchschnitt / Mittelwert) für einen der zurückliegenden Tage (Heute-1 = gestern, Heute-2 = vorgestern, ...). |
| Median (Auswertungszeitraum) | Zahl | Liefert den Median (Zentralwert) aller zwischengespeicherten Werte des gesamten Auswertungszeitraums. |
| Median (Heute-x) | Zahl | Liefert den Median (Zentralwert) für einen der zurückliegenden Tage (Heute-1 = gestern, Heute-2 = vorgestern, ...). |
| Modus (Auswertungszeitraum) | Zahl | Liefert den Modus (Modalwert) aller zwischengespeicherten Werte des gesamten Auswertungszeitraums. |
| Modus (Heute-x) | Zahl | Liefert den Modus (Modalwert) für einen der zurückliegenden Tage (Heute-1 = gestern, Heute-2 = vorgestern, ...). |
| Minimum (Auswertungszeitraum) | Zahl | Liefert den minimalen Wert aller zwischengespeicherten Werte des gesamten Auswertungszeitraums. |

## Anwendung

### Beispiel: Heizperiode


Der Hauptanwendungsfall ist die Berechnung der [Heizperiode](https://de.wikipedia.org/wiki/Heizperiode). Hierbei werden typischerweise die Tagesdurchschnittstemperaturen stündlich für fünf zurückliegende Tage ermittelt. Unterschreitet das Fünftagemittel 15°C in Deutschland, beginnt die Heizperiode.


Um den Baustein möglichst flexibel nutzen zu können, beschränkt er sich auf das stündliche Einsammeln der Temperaturwerte und deren Zusammenfassung über eine Berechnungsvorschrift (Durchschnitt, Median, Modus). Gemeinsam mit Vergleicherbausteinen (oder Schwellwertbausteinen mit Hysterese) kann eine individuelle Heizperiode bestimmt werden.


Gerade in der Übergangszeit können signifikante Schwankungen auftreten. Diese lassen sich z.B. mit dem Median als Berechnungsvorschrift besser ausgleichen. Außerdem kann es einen Unterschied machen, ob die Durchschnittstemperaturen der Einzeltage (Vergleicher + Und-Verknüpfung) oder des gesamten Überwachungszeitraums an die Vergleicher weitergereicht werden.


![Wertesammler - Einfache Steuerung der Heizperiode](/content/logic.aggregation/img/aggregator--example01.png)
*Vereinfachte Auswertung: Wenn die Durchschnittstemperatur der letzten 5 Tage unter 15 °C lag, beginnt die Heizperiode.*


![Wertesammler - Gesetzeskonforme Steuerung der Heizperiode](/content/logic.aggregation/img/aggregator--example02.png)
*Mögliche gesetzeskonforme Auswertung (für Deutschland - sofern die Heizperiode nicht kalendarisch definiert wird): Die Heizperiode beginnt, wenn 5 aufeinanderfolgende kalte Tage festgestellt werden und endet, wenn 5 aufeinanderfolgende warme Tage ermittelt wurden.*


:::warning
Das gezeigte Beispiel verwendet den Logikbaustein Formelberechnung von Horst Lehner ([Link zum Download im knx-user-forum.de](http://service.knx-user-forum.de/?comm=download&id=20000010)), um je ein komplexes UND- bzw. invertiertes UND-Gatter zu vermeiden.
:::


Dabei wird die Anzahl der aufeinanderfolgenden Tage gezählt, an denen es kälter ist als 15°C (5 Vergleicher) und Wahrheitswerte per Typumwandlung am Ausgang mittels einfacher Summenformel "\{Tag1:N\}+\{Tag2:N\}+\{Tag3:N\}+\{Tag4:N\}+\{Tag5:N\}" durch den Formelberechner aggregiert.


Sind fünf aufeinanderfolgende Tage zu kalt, beginnt die Heizperiode. Sind null aufeinanderfolgende Tage zu warm, endet sie. Der Baustein "Schwellwert mit Hysterese" muss dafür auf 1..4 eingestellt werden, damit die Werte über- bzw. unterschritten werden.

## Lizenzinformationen

{{moduleLicenseLink path="/de/logic.aggregation#lizenz"}}
- [Bar Chart](https://icons8.com/icon/Ww4I6zPDi4xI/bar-chart) icon by [Icons8](https://icons8.com)
