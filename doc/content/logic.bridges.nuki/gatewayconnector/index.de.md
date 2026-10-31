---
title: Bridge
pageLevel: module
moduleLogo: /content/logic.bridges.nuki/img/gatewayconnector--icons8-router-44.png
---

## Beschreibung

Dieser Baustein kommuniziert mit der Nuki Bridge. Mit Hilfe der [Nuki Auftraggeber](/de/logic.bridges.nuki/gatewayrequest) und [Nuki Antwortfilter](/de/logic.bridges.nuki/gatewayresponse) können bei den an die Bridge angeschlossenen Geräten Aktionen ausgelöst oder der Status ausgegeben werden.


Diese Komponente verfügt über einen Eingang (*Info (zwischengespeichert)*), über den alle angeschlossenen Geräte zentral abgefragt werden können.


Um diesen Baustein zu nutzen, wird eine Nuki Bridge benötigt, die im gleichen Netzwerk erreichbar ist. Zur Konfiguration sind die Parameter "token" der Nuki Bridge sowie deren IP-Adresse erforderlich. Die Einrichtung der Nuki Bridge und das Auslesen dieser Parameter ist auf dieser Webseite beschrieben:


1. [Ermitteln der Nuki Bridge IP-Adresse](https://developer.nuki.io/page/nuki-bridge-http-api-1-13/4/#heading--bridge-discovery) --> Die IP-Adresse der Nuki Bridge sollte statisch vergeben werden.
2. [Erzeugen eines Zugriffstokens](https://developer.nuki.io/page/nuki-bridge-http-api-1-13/4/#heading--token) --> Das Erzeugen muss innerhalb von 30sek an der Nuki Bridge bestätigt werden.


**Hinweis**: Soll nur ein einzelnes SmartLock gesteuert werden, so gibt es einen Baustein ([Nuki SmartLock](/de/logic.bridges.nuki/smartlock)) der die drei Komponenten (Bridge, Auftraggeber, Antwortfilter) vereint.

## Eigenschaften

### Eingänge


| Bezeichnung | Porttyp | Beschreibung |
| --- | --- | --- |
| Aufträge | Text | Nimmt Auftragstelegramme entgegen, die der [Telegrammsyntax](/de/logic.bridges.nuki/doc/syntax) entsprechen. |
| Info (zwischengespeichert) | Binär | Fragt den (an der Bridge zwischengespeicherten) Status aller an der Bridge angeschlossenen Geräte gleichzeitig ab. Dies wirkt sich positiv auf die Batterielaufzeit der Geräte aus. |



### Parameter


| Bezeichnung | Porttyp | Beschreibung |
| --- | --- | --- |
| Bridge IP-Adresse | Text | Legt die IPv4-Adresse der Nuki Bridge (z.B. "0.0.0.0") fest. Der Logikbaustein ergänzt intern den Port "8080". |
| Bridge Token | Text | Eingabe des Zugriffstokens. |



### Ausgänge


| Bezeichnung | Porttyp | Beschreibung |
| --- | --- | --- |
| Antworten | Text | Sendet Antworttelegramme, die der [Telegrammsyntax](/de/logic.bridges.nuki/doc/syntax) entsprechen. |
| Fehler | Text | Liefert eventuelle Netzwerk-Fehlermeldungen aus der Kommunikation mit der Nuki Bridge. |

## Anwendung

### Beispiel: Türschloss


Die Steuerung der Wohnungstür mit Hilfe der Nuki Logikbausteine kann folgendermaßen aussehen:


![Nuki Bridge Logikbaustein](/content/logic.bridges.nuki/img/gatewayconnector--example01.png)
*Anwendungsbeispiel der Nuki Logikbausteine.*


Im Beispiel ist ein einzelnes SmartLock mit allen drei Komponenten realisiert. Das nachfolgende Beispiel stellt zwei SmartLocks (z.B. Haustür, Kellertür) dar. Beide SmartLocks sind mit der gleichen Bridge verbunden.


![Nuki Bridge Logikbausteine für zwei SmartLocks](/content/logic.bridges.nuki/img/gatewayconnector--example02.png)
*Anwendungsbeispiel der Nuki Logikbausteine für zwei SmartLocks.*


:::warning
Da die Nuki Bridge nicht mehrere Anfragen gleichzeitig bzw. kurzzeitig hintereinander abarbeiten kann, wird empfohlen, eingehende Telegramme stufenweise zu verzögern (Baustein: Telegrammverzögerer; ca. 2-3 sek zwischen zwei Geräten), wenn diese gleichzeitig geschaltet werden sollen (z.B. "Alle Türen verschließen"). Die Nuki-Bausteine selbst verzögern keine Nachrichten, da sonst keine fachliche Priorisierung einzelner Geräte / Befehle möglich wäre!
:::

## Lizenzinformationen

- {{sectionLink path="/de/logic.bridges.nuki" section="lizenz" label="Lizenzbedingungen und rechtliche Hinweise für diesen Logikbaustein"}}
- [Router](https://icons8.com/icon/0ruxkz6Qtla5/router) icon by [Icons8](https://icons8.com)
