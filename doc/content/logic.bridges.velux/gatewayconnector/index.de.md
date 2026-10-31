---
title: Gateway
pageLevel: module
moduleLogo: /content/logic.bridges.velux/img/gatewayconnector--icons8-router-44.png
---

## Beschreibung

Dieser Baustein stellt die Verbindung zum Velux KLF-200 Gateway her. Er nimmt Auftragstelegramme entgegen und leitet sie als Datagramme an das Gateway weiter. Die Rückmeldungen des Gateways werden als Antworttelegramme ausgegeben.


Für die Telegramme wird eine URL-artige [Telegrammsyntax](/de/logic.bridges.velux/doc/syntax) verwendet.

## Eigenschaften

### Eingänge


| Bezeichnung | Porttyp | Beschreibung |
| --- | --- | --- |
| Aufträge | Text | Nimmt Auftragstelegramme entgegen, die der [Telegrammsyntax](/de/logic.bridges.velux/doc/syntax) entsprechen. |
| Status erfragen | Binär | Fordert den Velux KLF-200 auf, den Status aller Geräte abzufragen. Normalerweise ist es **nicht erforderlich**, den Status aktiv abzufragen - der Velux KLF-200 wird intern so vom Logikbaustein konfiguriert, dass er Aktualisierungen der Geräte mitbekommt und diese mitteilt. Beim Neustart des X1 oder des Logikbausteins wird der Status aller Geräte ebenfalls automatisch abgefragt (sofern die Geräte diesen an den KLF-200 zurückmelden). |
| Neu verbinden | Binär | Baut eine neue Verbindung zum Velux KLF-200 auf, wenn eine "1" gesendet wird. Typischerweise ist ein manueller Verbindungsaufbau nicht erforderlich, da dieser automatisch beim Starten des X1-Logikmoduls beginnt. Falls die Verbindung zum Velux KLF-200 unterbrochen wird, versucht der Baustein automatisch, in einem siebenminütigen Zeitfenster die Verbindung neu aufzubauen. |
| Neu starten | Binär | Sendet die Bitte zum Velux KLF-200, sich neu zu starten. Der Logikbaustein versucht nach einer Minute automatisch, die Verbindung zum Velux KLF-200 aufzubauen. Damit der Befehl den Velux KLF-200 erreicht, muss zuvor eine aktive Verbindung zum Gateway bestehen. |



### Parameter


| Bezeichnung | Porttyp | Beschreibung |
| --- | --- | --- |
| IPv4-Adresse | Text | IPv4-Adresse des Velux KLF-200 Gateways (z.B. "192.168.2.100"). Es muss sichergestellt werden, dass das Gateway auf Port 51200 erreichbar ist. |
| Passwort | Text | Passwort für das Velux KLF-200 Gateway. Sofern das Passwort nicht geändert wurde, entspricht es dem WiFi-Passwort, dass auf der Unterseite des Gateways zu finden ist. |



### Ausgänge


| Bezeichnung | Porttyp | Beschreibung |
| --- | --- | --- |
| Antworten | Text | Sendet Antworttelegramme, die der [Telegrammsyntax](/de/logic.bridges.velux/doc/syntax) entsprechen. |
| Status | Text | Informiert über den Verbindungsstatus des Logikmoduls zum Velux KLF-200 Gateway. *Disconnecting* Status nach einer Anforderung zum Neustart des Gateways bzw. wenn sich der Logikbaustein neu verbinden soll. *Disconnected* Der Logikbaustein kommuniziert nicht mit dem Velux KLF-200 Gateway. *Connecting* Der Logikbaustein beginnt damit, eine Verbindung zum Velux KLF-200 aufzubauen. *Connected* Der Logikbaustein hält eine aktive Verbindung zum Velux KLF-200. Standardwert: *Disconnected* Alle sieben Minuten wird geprüft, ob der Logikbaustein noch eine aktive Verbindung zum Velux KLF-200 hält. Falls die Verbindung (z.B. durch einen Strom- oder Netzwerkausfall des Velux KLF-200) abbricht, kann es sein, dass der Status innerhalb dieser Zeit noch *Connected* anzeigt, bevor er auf *Disconnected* springt oder ein automatischer Versuch für einen neuen Verbindungsaufbau stattfindet. Scheitert der Logikbaustein dabei, die Verbindung automatisch neu aufzubauen (es wird genau einmal versucht), muss die Verbindung manuell neu hergestellt werden. |
| Fehler | Text | Teilt Fehler mit, die bei der Auswertung von Telegrammen oder der Kommunikation mit dem Velux KLF-200 Gateway auftreten. |

## Anwendung

### Konzept


In der Regel befindet sich ein Gateway-Logikbaustein auf einem Logikblatt, um den herum die verschiedenen Auftraggeber (Rollladen: Küche, Rollladen: Esszimmer, Szene: Urlaub) und die Auswertungsmodule (Rollladen: Küche, Rollladen: Esszimmer) angeordnet sind.


![Übersicht der Velux KLF-200 Logikkomponenten](/content/logic.bridges.velux/img/gatewayconnector--example01.png)
*Ein Gateway-Logikmodul im Verbund mit den Auftraggebern und Ausgabefiltern.*


Da ein Velux KLF-200 Gateway maximal zwei Verbindungen halten kann, **soll pro physikalischem Gateway nur ein passendes Gateway-Logikmodul verwendet werden**.


Aus Gründen der Übersichtlichkeit können Telegramme durchaus auf verschiedenen Logikblättern erzeugt und ausgewertet werden. Dazu werden die Auftragstelegramme über eine lokale Variable "eingesammelt" und an das Gateway weitergeleitet. Umgekehrt werden die Ausgangstelegramme über eine weitere Variable "verteilt" und dann auf anderen Logikblättern gefiltert.


![Zentrales Velux KLF-200](/content/logic.bridges.velux/img/doc--example02-03.png)
*In diesem Fall wird ein Logikblatt verwendet, um das Gateway mit zwei internen Textvariablen zu verbinden, über die dann Aufträge eingereicht und Antworten verteilt werden.*


![Logikblatt mit einem Rollladenaktor.](/content/logic.bridges.velux/img/doc--example02-04.png)
*Beispiel einer Markisensteuerung. Der Antwortfilter filtert Telegramme für das Gerät "Balkon" und aktualisiert die Eigenschaften "Position" und "Bewegung"; der Auftraggeber nimmt die KNX-Telegramme entgegen und reicht umgewandelt sie an das Velux KLF-200 Gateway (Logikbaustein) weiter.*


Gleichzeitig ist es so auch möglich, unterschiedliche Auftraggeber für ein und dasselbe Gerät zu konfigurieren und dabei unterschiedliche Prioritäten (z.B. Automatiken, Sensoren, Nutzerinteraktion) zu hinterlegen.


![Logikblatt mit erweiterten Möglichkeiten zur Steuerung des Logikbausteins.](/content/logic.bridges.velux/img/doc--example04-01.png)
*Das Logikblatt zeigt, wie ein neuer Verbindungsaufbau bzw. ein Neustart des Velux KLF-200 angefordert werden. Es wird zudem gezeigt, wie man den Verbindungsstatus ausgeben bzw. eine Störung signalisieren kann (dazu wird der [Vergleicher+](/de/logic.control/switch)-Baustein im Vergleichsmodus "Text" verwendet).*


Die Seite [Hinweise zur Einrichtung](/de/logic.bridges.velux/doc/setup) hält weitere Informationen bereit.

## Lizenzinformationen

{{moduleLicenseLink path="/de/logic.bridges.velux#lizenz"}}
- [Router](https://icons8.com/icon/0ruxkz6Qtla5/router) icon by [Icons8](https://icons8.com)
