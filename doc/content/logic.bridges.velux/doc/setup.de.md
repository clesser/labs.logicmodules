---
title: [neleo.labs] X1 Velux KLF Bausteine
---

## Einführung

Die Besonderheiten von KNX, dem Gira X1 und dem Velux KLF-200 führen zu einer möglicherweise auf den ersten Blick komplizierten Verbindung aller drei Akteure.


Auf dieser Seite sollen daher einige Konfigurationen durchgesprochen werden.


Die nachfolgend gezeigten Konfigurationen beziehen sich auf einzelne Aktoren/Geräte. Sollen mehr als 3 Aktoren/Geräte gleichzeitig gesteuert werden (gleichzeitiger Start/Wertänderung), ist es ratsam Gruppen und Szenen zu definieren, die dann abgerufen werden können.


Gute Erfahrungen wurden auch damit gesammelt, die einzelnen Geräte zeitverzögert (ca. 2 Sekunden pro Gerät mit Hilfe eines Telegrammverzögerers) anzusteuern um die Kapazität des Velux KLF-200 nicht zu überlasten.

## X1-Solobetrieb

In diesem Einsatzszenario wird im X1 ein virtueller Aktor nachgebaut. Dafür werden ausschließlich Variablen-Datenpunkte benötigt. Für einen Rollladenaktor würde dies beispielsweise folgendermaßen aussehen:


### GPA Datenpunkte


![Konfiguration der Variablen-Datenpunkte und Visualisierung](/content/logic.bridges.velux/img/doc--example01-01.png)
*Konfiguration der Variablen-Datenpunkte für die Visualisierung eines Rollladens im Gira Projekt Assistent*


### GPA Logikblätter


![Logikblatt für die Steuerung eines einzelnen Rollladens](/content/logic.bridges.velux/img/doc--example01-02.png)
*Wenn die Rollladensteuerung ausschließlich im Gira X1 abgebildet wird, kann anstelle des Kurz- und Langbetriebs auch die Steuerung über Min, Max und Stopp erfolgen. Dabei geht zwar der "Kurzbetrieb" verloren - jedoch ist die %-genaue Positionierung direkt erreichbar.*


Auch wenn nur ein einzelner Aktor abgebildet ist, müssen die Gerätebezeichner im Auftraggeber und Antwortfilter korrekt gesetzt sein, damit die Telegramme richtig verarbeitet werden können.

## KNX- und X1-Mischbetrieb

Realistischer dürfte jedoch eine Mischkonfiguration sein, bei der einige Funktionen über klassische KNX-Taster abgerufen werden. Im nachfolgend beschriebenen Fall sendet ein Gira Tastsensor 3 die Befehle zum Langzeit- und Kurzzeitbetrieb (jeweils Auf/Ab) an eine Markise.


Die Ansteuerung einer konkreten Position und die Rückmeldung der Bewegung der Markise wird ausschließlich im Gira X1 verarbeitet und visualisiert.


### ETS Parametrierung


![Konfiguration des Tastsensors in der ETS](/content/logic.bridges.velux/img/doc--example02-01.png)
*In der ETS hat sich die Konfiguration "Lang-Kurz" bewährt. Aufgrund persönlicher Präferenzen liegt die Abwärtsbewegung auf der linken Wippenseite und die Aufwärtsbewegung daraus folgend auf der rechten Wippenseite.*


### GPA Datenpunkte


![Konfiguration der Gruppenadress- und Variablen-Datenpunkte in der Visualisierung](/content/logic.bridges.velux/img/doc--example02-02.png)
*Im Beispiel werden die Fahrbefehle durch Gruppenadress-Datenpunkte angesteuert und die Position bzw. Rückmeldung mittels Variablen.*


Natürlich ist es auch möglich, dass alle Datenpunkte als Gruppenadress-Datenpunkte definiert werden.


Falls die **Position** als Gruppenadress-Datenpunkt angelegt wird, so wird die GA für die Rückmeldung (= Ausgang des Antwortfilters) unter "Senden" eingetragen werden und die GA für eingehende Positionsanfragen als "Status/Rückmeldung". Nur so werden KNX-Telegramme vom Bus richtig interpretiert und der Status korrekt zurückgeschrieben.


### GPA Logikblätter


![Logikblatt mit zentralem Gateway-Logikbaustein](/content/logic.bridges.velux/img/doc--example02-03.png)
*Da es im konkreten Beispiel viele Rollläden und Markisen gibt, werden nicht alle Logikbausteine auf ein Logikblatt gebracht, sondern diese raumweise gruppiert. Der zentrale Gateway-Baustein bekommt ein eigenes Logikblatt und zwei Text-Variablen-Datenpunkte, über die alle Bausteine untereinander kommunizieren.*


![Logikblatt der Markisensteuerung](/content/logic.bridges.velux/img/doc--example02-04.png)
*Die Steuerung der Markise ist auf einem eigenen Logikblatt untergebracht. Die Ein- und Ausgänge der Bausteine sind mit den zuvor definierten Gruppenadress- und Variablen-Datenpunkten verbunden.*
       

![Logikblatt mit einer Gruppensteuerung](/content/logic.bridges.velux/img/doc--example02-05.png)
*Die beiden Auftraggeber sind Gruppen (Konfiguration siehe "Sicherheitsüberlegungen") zugewiesen und fahren die Markisen bei Nacht ein und die Rollladen bei Nachtanbruch herunter und bei Taganbruch wieder herauf, sofern dies durch die Automatik (siehe auch: [Sperre+](/de/logic.control/blocker)) nicht unterbunden wird.*

## Sicherheitsüberlegungen

Die Signalstrecke von einem Tast- oder Wettersensor über den KNX-Bus zum X1, weiter zum Velux KLF-200 Gateway und dann zum Rollladen oder zur Markise ist lang und kann störungsanfällig sein. Es wäre daher zu überlegen, ob man für Sicherheitsfunktionen den X1 und das Netzwerk umgeht. Dafür bieten sich die potentialfreien Kontakte des Velux KLF-200 an.


Im nachfolgend dargestellten Fall werden je alle Rollläden und alle Markisen in zwei Gruppen zusammengefasst. Die Gruppen können nun einerseits über den Auftraggeber (in der Gruppenkonfiguration; z.B. Rollladen auf am Morgen, Markisen einfahren am Abend) genutzt werden.


![Gruppenkonfiguration im Velux KLF-200](/content/logic.bridges.velux/img/doc--example03-01.png)
*Rollladen und Markisen sind jeweils in eigenen Gruppen zusammengefasst, um jeweils unterschiedliche Sicherheitsfunktionen auszulösen.*


Darüber hinaus können aber mit einem einfachen Schaltaktor Sicherheitsaktionen ausgeführt werden (z.B. Rollladen auffahren bei Rauchalarm, Markisen einfahren bei Regen oder Wind). Der Schaltaktor wird dabei potentialfrei mit den Eingängen verbunden.


Beim Mischbetrieb der potentialfreien Eingänge über KNX-Aktoren ist darauf zu achten, dass die gleichen Gruppenadressen im X1 (z.B. über einen Auftraggeber in der Gruppenkonfiguration) **nicht** die gleiche Aktion auslösen, da die Befehle sich gegenseitig stoppen.


Es ist aber problemlos möglich, Sicherheitsfunktionen über die potentialfreien Eingänge auszulösen und Komfortfunktionen über den X1. Dabei sollten nur andere Gruppenadressen / Datenpunkte verwendet werden.
