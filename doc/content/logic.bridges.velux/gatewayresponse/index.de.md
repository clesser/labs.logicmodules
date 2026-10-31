---
title: Antwortfilter
pageLevel: module
moduleLogo: /content/logic.bridges.velux/img/gatewayresponse--icons8-output-44.png
---

## Beschreibung

Der Antwortfilter liest die vom [Gateway Connector](/de/logic.bridges.velux/gatewayconnector) des Velux KLF-200 Gateways generierten Antworttelegramme ein, filtert die für ein Gerät passenden Nachrichten heraus und stellt die Werte an den Ausgängen dar.


Je nach Initialisierungsreihenfolge der Bausteine kann es sein, dass die Statusinformationen über die Geräte, die per Velux KLF-200 verbunden sind, erst im nächsten Aktualisierungsintervall (alle 15 min) oder nach der ersten Aktion der Geräte an den Antwortfiltern sichtbar werden. Ein "Stop"-Befehl für ein Gerät liefert eine unmittelbare Aktualisierung der Ausgänge.


Für die Telegramme wird eine URL-artige [Telegrammsyntax](/de/logic.bridges.velux/doc/syntax) verwendet.

## Eigenschaften

### Eingänge


| Bezeichnung | Porttyp | Beschreibung |
| --- | --- | --- |
| Antworten | Text | Nimmt Antworttelegramme entgegen, die der [Telegrammsyntax](/de/logic.bridges.velux/doc/syntax) entsprechen. |



### Parameter


| Bezeichnung | Porttyp | Beschreibung |
| --- | --- | --- |
| Gerätebezeichner | Text | Bezeichner (Name) des Geräts. Dem Bezeichner kann ein "/" vorangestellt sein. Wird der Antwortfilter mit einem [Gateway](/de/logic.bridges.velux/gatewayconnector) verbunden, so muss immer der Name des Geräts (aus der Gerätetabelle des Velux KLF-200) eingetragen werden! Zu Testzwecken (ohne Gateway) kann der interne Bezeichner (Gerätenummer) genutzt werden, wenn diesem ein ":" (z.B. ":3") vorangestellt wird. Das Gateway wird jedoch immer Telegramme mit dem Namen aussenden. |
| Ausgabewertbereich | Auswahl | Legt fest, wie die Werte "Aktueller Wert" und "Zielwert" ausgegeben werden sollen:*Standard (0..51200)* liefert die Rohdaten,*Prozent (0..100)* rechnet die Rohdaten in einen Prozentwert um, *Prozent (100..0)* rechnet die Rohdaten in Prozent (invertiert) um. Standardwert: *Prozent (0..100)*. |



### Ausgänge


| Bezeichnung | Porttyp | Beschreibung |
| --- | --- | --- |
| Aktueller Wert | Ganzzahl | Zeigt den aktuell eingestellten Wert des Standardparameters des gefilterten Geräts an. Die Ausgabe erfolgt entsprechend des gewählten Ausgabewertbereichs. |
| Aktueller Wert (%) | Prozent | Zeigt den aktuell eingestellten Wert des Standardparameters des gefilterten Geräts an. Die Ausgabe erfolgt entsprechend des gewählten Ausgabewertbereichs. |
| Zielwert | Ganzzahl | Zeigt den (zuletzt) angeforderten Wert des Standardparameters des gefilterten Geräts an. Die Ausgabe erfolgt entsprechend des gewählten Ausgabewertbereichs. |
| Zielwert (%) | Prozent | Zeigt den (zuletzt) angeforderten Wert des Standardparameters des gefilterten Geräts an. Die Ausgabe erfolgt entsprechend des gewählten Ausgabewertbereichs. |
| Aktivität | Binär | Zeigt an, ob das Gerät gerade arbeitet, um den Zielwert für den Standardparameter einzunehmen. |
| Fehler | Text | Zeigt an, ob ein Gerät einen Fehler meldet. *NoContact* - Gerät ist nicht erreichbar (d.h. in der Regel stromlos) |

## Anwendung

In der Regel befindet sich ein Gateway-Logikbaustein auf einem Logikblatt, um den herum die verschiedenen Auftraggeber (Rollladen: Küche, Rollladen: Esszimmer, Szene: Urlaub) und die Auswertungsmodule (Rollladen: Küche, Rollladen: Esszimmer) angeordnet sind.


![Übersicht der Velux KLF-200 Logikkomponenten](/content/logic.bridges.velux/img/gatewayconnector--example01.png)
*Ein Gateway-Logikmodul im Verbund mit den Auftraggebern und Ausgabefiltern.*


Weitere Beispielinformationen wurden auf der Seite [Hinweise zur Einrichtung](/de/logic.bridges.velux/doc/setup) gesammelt.

## Lizenzinformationen

{{moduleLicenseLink path="/de/logic.bridges.velux#lizenz"}}
- [Output](https://icons8.com/icon/90318/output) icon by [Icons8](https://icons8.com)
