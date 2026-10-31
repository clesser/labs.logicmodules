---
title: [neleo.labs] X1 Velux KLF Bausteine
---

## Beschreibung

Die Kommunikation der Logikbausteine untereinander erfolgt mit Hilfe von Telegrammen, die einer URL-Syntax nachempfunden sind:


- request://\{scene|group|node\}\{:id|/name\}?param1=value1&param2=value2#action
- response://\{scene|group|node\}\{:id|/name\}?param1=value1
- info://\{scene|group|node\}


Die [Velux-Auftraggeber](/de/logic.bridges.velux/gatewayrequest)-Bausteine generieren "request://"-Telegramme, die an den Eingang des [Velux Gateways](/de/logic.bridges.velux/gatewayconnector) gesendet werden. Dieses antwortet mit "response://"-Telegrammen, die es über seinen Ausgang an die Eingänge der [Velux-Ausgabefilter](/de/logic.bridges.velux/gatewayresponse)-Bausteine sendet ("Broadcast").


*scene*, *group* oder *node* geben an, ob sich die nachfolgenden Parameter auf Szenen, Gruppen oder einzelne Geräte ("node") beziehen.


Die Szenen, Gruppen oder Geräte können entweder mit Hilfe ihrer internen Nummer (*:id*) oder anhand ihres Namens (*/name*) identifiziert werden (z.B. "request://scene/Aufwachen#start" oder "request://node:3?position=25").


Es schließt sich nun die optionale Parameterliste an. Sie beginnt mit dem Fragezeichen ("?") und dem ersten Schlüssel-Wert-Paar. Weitere Schlüssel-Wert-Paare werden durch ein kaufmännisches Und ("&") angehängt.


Weiterhin besteht die Möglichkeit, Schnellbefehle zu definieren. Diese werden mit der Raute ("#aktion") eingeleitet. So werden beispielsweise Stop-Befehle (z.B. "request://node:2#stop") gesendet.


Die möglichen Schlüssel-Wert-Paare und Aktionen hängen u.a. von der Art der Anfrage (Szene, Gruppe, Gerät) und deren Fähigkeiten (z.B. auf/ab/stop, ein/aus) ab. Gegenwärtig wird nur das Setzen des Standardparameters (target=) unterstützt.


Es gibt zusätzlich die Möglichkeit, die Liste aller Szenen, Gruppen und Geräte (Nummer und Name) ausgeben zu lassen.


In der Regel ist das manuelle Zusammenstellen der Telegramme nicht erforderlich - jedoch können damit gegebenenfalls auch noch nicht offiziell getestete Geräte angebunden werden.

## Eigenschaften

### Protokoll


| Bezeichnung | Beschreibung |
| --- | --- |
| request:// | Das Telegramm richtet sich an den Eingang des [Velux Gateway](/de/logic.bridges.velux/gatewayconnector)s. Es wird verwendet, um Eigenschaften in Geräten oder Gruppen zu setzen oder Szenen zu beginnen/beenden. |
| response:// | Das Telegramm wird vom Ausgang des [Velux Gateway](/de/logic.bridges.velux/gatewayconnector)s versendet. Es zeigt Veränderungen der Eigenschaften von Geräten an. |
| info:// | Das Telegramm richtet sich an den Eingang des [Velux Gateway](/de/logic.bridges.velux/gatewayconnector)s. Es wird verwendet, um die Liste der Geräte (Nodes), Gruppen oder Szenen abzurufen. Die Liste wird Ausgang "Antworten" des Gateway-Logikbausteins gesendet und ist für die erleichterte Einrichtung gedacht. Alternativ kann auch der [KLF-200 Tester](/de/logic.bridges.velux/doc/tester) verwendet werden, um die Liste der Geräte, Gruppen und Szenen auszulesen. |



### Geltungsbereich


| Bezeichnung | Beschreibung |
| --- | --- |
| node | Gibt an, dass die nachfolgenden Parameter für ein einzelnes Gerät (Node) gelten. |
| group | Gibt an, dass die nachfolgenden Parameter für eine Gruppe an Geräten gelten. |
| scene | Gibt an, dass die nachfolgenden Aktionen sich auf eine Szene beziehen. |



### Bezeichner


| Bezeichnung | Protokolle | Geltungsbereiche | Beschreibung |
| --- | --- | --- | --- |
| :id | request | node{{br}}group{{br}}scene | Der Bezeichner eines Geräts, einer Gruppe oder einer Szene wird in Form der internen Stellennummer im Velux KLF-200 Gateway angegeben. Das Velux KLF-200 Gateway unterstützt die folgenden Nummernkreise: Geräte: 0..199, Gruppen: 0..99, Szenen: 0..31. Hinweis: die Gerät-, Gruppen- oder Szenennummer kann nur bei "request://"-Telegrammen bzw. im Auftraggeber genutzt werden. In "response://"-Telegrammen bzw. im Ausgabefilter werden immer die Namen (siehe nächste Zeile) verwendet! |
| /name | requestresponse | node{{br}}group{{br}}scene | Der Bezeichner eines Geräts, einer Gruppe oder einer Szene wird mit dem im Velux KLF-200 Gateway vergebenen Namen angesprochen (z.B. "Esszimmer"). Der Name darf max. 64 Buchstaben lang sein. |



### Aktionen


| Bezeichnung | Protokolle | Geltungsbereiche | Beschreibung |
| --- | --- | --- | --- |
| #start | request | node{{br}}group{{br}}scene | Setzt den Standardparameter (z.B. Position bei Rollläden) eines Geräts oder einer Gerätegruppe auf den Standardwert ("0xD300"). Dieser Wert ist nicht für alle Geräte definiert und wird gegebenenfalls ignoriert. Beginnt, eine Szene auszuführen. |
| #stop | request | node{{br}}group{{br}}scene | Setzt den Standardparameter (z.B. Position bei Rollläden) eines Geräts oder einer Gerätegruppe auf den aktuellen Wert ("0xD200"). Damit werden laufende Aktionen eines Geräts oder einer Gruppe angehalten. Haben die Geräte oder Gerätegruppen ihre jeweiligen Zielwerte schon erreicht, wird das Telegramm ignoriert. Beendet eine laufende Szene. Ist die Szene bereits abgelaufen, wird das Telegramm ignoriert. |
| #min | request | node{{br}}group | Setzt den Standardparameter (z.B. Position bei Rollläden) eines Geräts oder einer Gerätegruppe auf den minimalen Wert ("0x000" = Auffahren bis zur Endposition bei Rollläden). Siehe Kapitel 14.1 der Velux KLF-200-Dokumentation. |
| #max | request | node{{br}}group | Setzt den Standardparameter (z.B. Position bei Rollläden) eines Geräts oder einer Gerätegruppe auf den maximalen Wert ("0xC800" = Abfahren bis zur Endposition bei Rollläden). Siehe Kapitel 14.1 der Velux KLF-200-Dokumentation. |



### Parameter


| Bezeichnung | Protokolle | Geltungsbereiche | Aktionen | Beschreibung |
| --- | --- | --- | --- | --- |
| source | request | node{{br}}group{{br}}scene | minmaxstartstop | Definiert, wer das Telegramm initiiert hat: *User* *Rain* *Timer* *UPS* *SAAC* *Wind* *LoadShedding* *LocalLight* *UnspecificEnvironmentSensor* *Emergency* Standardwert: *User* |
| priority | request | node{{br}}group{{br}}scene | minmaxstartstop | Definiert, mit welcher Priorität das Telegramm behandelt werden soll: *HumanProtection* *EnvironmentProtection* *UserLevel1* *UserLevel2* *ComfortLevel1* *ComfortLevel2* *ComfortLevel3* *ComfortLevel4* Standardwert: *UserLevel2*, für Stop-Telegramme: *UserLevel1* |
| velocity | request | node{{br}}group{{br}}scene | minmaxstart | Definiert, mit welcher Geschwindigkeit die Geräte bzw. Gerätegruppen arbeiten sollen, sofern sie dies technisch unterstützen: *Default* *Silent* *Fast* Standardwert: *Default* |
| target | request | node{{br}}group | (wird durch #start, #stop, #min und #max überschrieben) | Der Zielwert kann auf unterschiedliche Weise festgelegt werden: *Absoluter Zielwert*0x0000 = 0 = 0%0xC800 = 51200 = 100% *Relativer Zielwert*0xC900 = 51456 = -100%0xCCE8 = 52456 = +/-0%0xD0D0 = 53456 = +100% *Default* entspricht #start *Current* entspricht #stop Absolute und relative Zielwerte müssen in Dezimalschreibweise angegeben werden. Siehe Kapitel 13 der Velux KLF-200-Dokumentation. |
| target | response | node | - | Es wird der absolute Zielwert (0x000 .. 0xC800 bzw. 0..51200) für ein Gerät ausgegeben. Wurde ein relativer Zielwert (+/-%) gesetzt, liefert das Gateway den undefinierten Wert (0xF7FF = 63487). Werte werden immer in Dezimalschreibweise ausgegeben. Bei einem Rollladen entspricht dies der anzufahrenden Position. |
| current | response | node | - | Es wird der absolute Wert (0x000 .. 0xC800 bzw. 0..51200) des Standardparameters eines Geräts ausgegeben. Werte werden immer in Dezimalschreibweise ausgegeben. Bei einem Rollladen entspricht dies der gegenwärtigen Position. |
| countdown | response | node | - | Dieser Parameter informiert über die Zeit in Sekunden, die vom angegebenen Gerät benötigt wird, um den Zielwert des Standardparameters umzusetzen. Bei einem Rollladen entspricht dies der Fahrzeit von der aktuellen zur Zielposition. |
| state | response | node | - | Definiert, wer das Telegramm initiiert hat: *NonExecuting* *ErrorWhileExecution* *NotUsed* *WaitingForPower* *Executing* *Done* *StateUnknown* Typische Werte sind: *NotUsed*, *Executing* und *Done* |

## Anwendung

Die nachfolgenden Beispiele gehen von einem Rollladen mit dem Namen "Esszimmer" und der Gateway-internen Nummer von "1" aus.


request://node:1#min ► Fährt den Rollladen aufwärts bis zur Endposition (intern verwendete Standardparameter: source=User, priority=UserLevel2, velocity=Default).


request://node:1?priority=UserLevel1#min ► Fährt den Rollladen mit einer höheren Priorisierung aufwärts bis zur Endposition (intern verwendete Standardparameter: source=User, velocity=Default).


request://node/Esszimmer?target=25600 ► Fährt den Rollladen auf 50% (50% von 51200 entsprechen 25600; intern verwendete Standardparameter: source=User, priority=UserLevel2, velocity=Default). Die Angabe des Zielwertes 50% bezieht sich auf die Fahrzeit, nicht auf die "technische" Höhe des Rollladens. Hier muss ein wenig experimentiert werden, um die für jeden Rollladen korrekten "50%" zu finden.
