---
title: Hilfe
pageLevel: module
moduleLogo: /content/logic.bridges.velux/img/gatewayrequest--icons8-input-44.png
---

## Beschreibung

Dieser Baustein simuliert die Bedieneingänge eines Geräts (Node), einer Gerätegruppe (Group) oder dient dem Abruf einer Szene (Scene). Dabei setzt der Baustein die Steuersignale in Telegramme um, die das Velux KLF 200 Gateway verarbeiten kann.


Für die Telegramme wird eine URL-artige [Telegrammsyntax](/de/logic.bridges.velux/doc/syntax) verwendet.

## Eigenschaften

### Eingänge


| Bezeichnung | Porttyp | Geltungsbereiche | Beschreibung |
| --- | --- | --- | --- |
| Zielwert | Ganzzahl | NodeGroup | Empfängt den Zielwert für ein Gerät oder eine Gerätegruppe entsprechend des Eingangswertbereichs. |
| Zielwert (%) | Prozent | NodeGroup | Empfängt den Zielwert für ein Gerät oder eine Gerätegruppe entsprechend des Eingangswertbereichs. |
| Min / Max | Binär | NodeGroup | Weist das Gerät oder die Gerätegruppe an, den maximalen (1 = 100%) oder minimalen (0 = 0%) Wert für den Standardparameter zu setzen. Bei einem Rollladen fährt ein 1-Telegramm den Rollladen vollständig herunter, bei einem 0-Telegramm vollständig herauf. |
| Stop / Start | Binär | NodeGroupScene | Beginnt (1) oder unterbricht (0) die Ausführung einer Szene. Bei Geräten oder Gerätegruppen wird mit (1) der Standardwert eingestellt und mit (0) die Ausführung einer Wertänderung abgebrochen. Bei einem Rollladen führt eine (1) in der Regel zu keiner Aktion, eine (0) jedoch zum Anhalten des Motors. |
| Auf / Ab (Lang) | Binär | NodeGroup | Weist das Gerät oder die Gerätegruppe an, den maximalen (1 = 100%) oder minimalen (0 = 0%) Wert für den Standardparameter zu setzen. Bei einem Rollladen fährt ein 1-Telegramm den Rollladen vollständig herunter, bei einem 0-Telegramm vollständig herauf. |
| Auf / Ab (Kurz) | Binär | NodeGroup | Die Wertänderungen im Gerät bzw. der Gerätegruppe erfolgen in der definierten Schrittweite (z.B. 10%). Bei einem Rollladen fährt ein 1-Telegramm den Rollladen um den in der Schrittweite definierten Wert herunter, bei einem 0-Telegramm - analog dazu - herauf. Führt das Gerät gerade eine Wertänderung durch (z.B. ein fahrender Rollladen), so wird ein "Auf/Ab (Kurz)" für die "Auf/Ab (Dauer)" als "Stop" interpretiert. |



### Parameter


| Bezeichnung | Porttyp | Geltungsbereiche | Beschreibung |
| --- | --- | --- | --- |
| Geltungsbereich | Auswahl | NodeGroupScene | Wählt aus, ob der Auftraggeber ein Gerät, eine Gerätegruppe oder eine Szene repräsentieren soll. |
| Eingabewertbereich | Auswahl | NodeGroup | Legt fest, wie der "Zielwert" eingegeben werden sollen:*Standard (0..51200)* erwartet die Rohdaten,*Prozent (0..100)* erwartet einen Prozentwert, *Prozent (100..0)* erwartet einen Prozentwert und invertiert ihn intern. Standardwert: *Prozent (0..100)*. |
| Auf / Ab (Dauer) | Zeitspanne | NodeGroup | Legt die Zeitdauer einer vollständigen Wertänderung (z.B. Auffahrt eines Rollladens) fest. In dieser Zeitspanne werden "Auf / Ab (Kurz)"- Befehle als "Stop"-Befehl ausgeführt. Nach Ablauf dieser Zeit, werden "Auf / Ab (Kurz)"-Befehle wieder als Schritt-Befehle ausgeführt. Weiterhin wird in dieser Zeit das wiederholte Senden eines Wertes unterbunden (es kann jederzeit ein anderer Wert gesendet werden - nur nicht der gleiche. Nach Ablauf der Zeitspanne, wird auch der gleiche Wert wieder als Telegramm geschickt.* Bei einem Rollladen wird beispielsweise die Zeit für eine vollständige Aufwärtsbewegung (+5-10% Puffer) angegeben. |
| Auf / Ab (Kurz %) | Ganzzahl | NodeGroup | Legt die Schrittweite für eine Wertänderung über den Eingang "Auf / Ab (Kurz)" in 0..100% fest. |
| Name oder :Bezeichner | Text | NodeGroup | Name oder Bezeichner des Geräts, der Gerätegruppe oder Szene. Dem Bezeichner kann ein "/" vorangestellt sein. Es kann zudem der interne Bezeichner (Nummer) genutzt werden, wenn diesem ein ":" (z.B. ":3") vorangestellt wird. |
| Ausführungsgeschwindigkeit | Auswahl | NodeGroupScene | Einige Geräte unterstützen unterschiedliche Geschwindigkeiten für die Wertänderungen. Es kann gewählt werden zwischen:*Standard* entspricht der normalen Ausführungsgeschwindigkeit, *Still* reduziert die Geschwindigkeit und *Schnell* erhöht sie gegenüber der Normalgeschwindigkeit. Standardwert: *Standard* |
| Absender | Auswahl | NodeGroupScene | Definiert den Absender eines Telegramms. Der Standardwert *Benutzer - Prio 2* wird für alle menschlichen Interaktionen genutzt. Der Wert *Komfort - Prio 2* für alle Automatiken. |
| Priorität | Auswahl | NodeGroupScene | Definiert die Priorität des Telegramms. Der Standardwert *Mensch* wird für alle menschlichen Interaktionen genutzt. Der Wert *Logik / Automatik* wird für automatische Steuerungen verwendet. |



### Ausgänge


| Bezeichnung | Porttyp | Beschreibung |
| --- | --- | --- |
| Aufträge | Text | Sendet Auftragstelegramme, die der [Telegrammsyntax](/de/logic.bridges.velux/doc/syntax) entsprechen. |

## Anwendung

In der Regel befindet sich ein Gateway-Logikbaustein auf einem Logikblatt, um den herum die verschiedenen Auftraggeber (Rollladen: Küche, Rollladen: Esszimmer, Szene: Urlaub) und die Auswertungsmodule (Rollladen: Küche, Rollladen: Esszimmer) angeordnet sind.


![Übersicht der Velux KLF-200 Logikkomponenten](/content/logic.bridges.velux/img/gatewayconnector--example01.png)
*Ein Gateway-Logikmodul im Verbund mit den Auftraggebern und Ausgabefiltern.*


Logikblätter können so gestaltet werden, dass Funktionen gruppiert werden. So können beispielsweise Rollladensteuerungen für einzelne Rollläden oder Gruppen realisiert werden. Auf der Seite [Velux Gateway](/de/logic.bridges.velux/gatewayconnector) wird die verteilte Architektur abgebildet, die im nachfolgenden Beispiel verwendet wird.


![Auftraggeberbaustein für die Gruppensteuerung](/content/logic.bridges.velux/img/gatewayrequest--example01.png)
*In diesem Beispiel werden Auftraggeberbausteine verwendet, um die Rollläden von zwei Fensterfronten als Gruppe zu steuern. Achtung: Eine Gruppe kann nur angesteuert werden - die Rückmeldung der Positionen erfolgt über die einzelnen Geräte.*


Weitere Beispielinformationen wurden auf der Seite [Hinweise zur Einrichtung](/de/logic.bridges.velux/doc/setup) gesammelt.

## Lizenzinformationen

{{moduleLicenseLink path="/de/logic.bridges.velux#lizenz"}}
- [Input](https://icons8.com/icon/82745/input) icon by [Icons8](https://icons8.com)
