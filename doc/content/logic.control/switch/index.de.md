---
title: Vergleicher+
pageLevel: module
moduleLogo: /content/logic.control/img/switch--GiraIconLogicNodesNumericCompare.png
---

## Beschreibung

Dieser Baustein ergänzt den Baustein "Vergleicher". Es können mehrere Vergleiche (Zahlen oder Texte) parallel an den Ausgängen signalisiert werden.


Mitunter werden von Bausteinen Werte ausgegeben, die traditionell mit mehreren Vergleichern innerhalb einer Logik verteilt werden müssen. Mit diesem Baustein können eingehende Werte ähnlich einer Switch-Case-Logik simultan in einem einzelnen Baustein ausgewertet werden.

## Eigenschaften

### Eingänge


| Bezeichnung | Porttyp | Beschreibung |
| --- | --- | --- |
| Eingang | Ganzzahl / Text | Eingang der Sperre (je nach Konfiguration akzeptiert der Baustein Zahl- oder Textwerte) |



### Parameter


| Bezeichnung | Porttyp | Beschreibung |
| --- | --- | --- |
| Vergleichsmodus | Auswahl | Steuert das Verhalten des Bausteins: *Zahl* = numerische Werte werden miteinander verglichen*Text* = Zeichenvergleich basierend auf der aktuellen Kultur*Text (neutral)* = Zeichenvergleich ohne Berücksichtigung der aktuellen Kultur, sowie Groß-/ und Kleinschreibung Standardwert = Zahl |
| Anzahl Vergleichswerte | Ganzzahl | Gibt an, wie viele Vergleiche parallel stattfinden sollen |
| x. Wert | Ganzzahl / Text | Vergleichwert (je nach Konfiguration akzeptiert der Baustein Zahl- oder Textwerte) |



### Ausgänge


| Bezeichnung | Porttyp | Beschreibung |
| --- | --- | --- |
| Keine Übereinstimmung | Binär | Zeigt an, dass der Eingangswert nicht mit einem Vergleichswert übereinstimmt. |
| x. Auslöser | Binär | Zeigt an, dass der Eingangswert mit dem korrespondierenden Vergleichswert übereinstimmt. |

## Anwendung

### Beispiel: Szenenvisualisierung


Zentrale Szenen sollen mitunter zentral visualisiert werden. Bei einer eingehenden Szenennummer können dann entsprechende Telegramme an LEDs etc. verschickt werden.


![Vergleicher+ - Szenenvisualisierung](/content/logic.control/img/switch--example01.png)
*Dieser Vergleicher+ wird gewählt, um die jeweils zuletzt gewählte bereichsübergreifende Szene zu visualisieren (Szenennummer --> LEDs).*


### Beispiel: IoT-Gerätezustände auswerten


Viele IoT-Geräte geben den Zustand in Form von Zustandskennzahlen aus, auf die in einer Logik reagiert werden kann. Mit Hilfe des Bausteins kann so z.B. ein eingehender Byte-Wert verschiedene Signalisierungen auslösen.


Beispiel: [Fehlerauswertung beim Nuki Smartlock](/de/logic.bridges.nuki/smartlock).

## Lizenzinformationen

{{moduleLicenseLink path="/de/logic.control#lizenz"}}
- Das Icon wurde der Komponente "Vergleicher" aus dem Gira Projekt Assistent entnommen.
