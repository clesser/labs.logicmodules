---
title: [neleo.labs] X1 Velux KLF Bausteine
---

## Einführung

Mitunter ist es recht aufwändig, das Setup mit einem Gira X1, einem Velux KLF-200 und dem Gira Projekt Assistenten (GPA) zu konfigurieren.

    
Aus diesem Grund stelle ich eine Kommandozeilenanwendung zur Verfügung, die ich während der Entwicklungsarbeit genutzt habe und die es ermöglicht, die Funktionen mit Hilfe der [Telegrammsyntax](/de/logic.bridges.velux/doc/syntax) einfacher zu testen.

    
Die Anwendung erfordert das .NET-Framework, das aber bereits mit dem GPA installiert wird.

## Download und Start

Die Anwendung kann als [KLF200Tester.zip](https://1drv.ms/u/c/19ea6aaf4b1f919a/EQNViBgZ5QlMq81n5-HUQwIBJAuOsFukZe4_3_INWDiSnA) heruntergeladen werden.


Nach dem Entpacken der ZIP-Datei kann die Anwendung durch einen Doppelklick auf die Datei KLF200Tester.exe gestartet werden. Falls der Tester häufiger genutzt werden soll, kann eine Verknüpfung mit allen Parametern angelegt werden:

    
- "Pfad\zu\KLF200Tester.exe"
- "Pfad\zu\KLF200Tester.exe" **
- "Pfad\zu\KLF200Tester.exe" ** *
* Nicht übergebene Parameter werden von der Anwendung abgefragt.

    
- Beispiel: KLF200Tester 192.168.1.100 P@ssw0rd

## Benutzung

Nach dem Start der Anwendung werden fehlende Parameter abgefragt und es wird versucht, eine Verbindung zum KLF-200 herzustellen.

    
Die Anwendung fragt anschließend nach dem Betriebsmodus:

    
- (S)ocket - stellt die Kommunikation mit dem KLF-200 dar und ist für Debugging-Zwecke gedacht.
- (G)ateway - ermöglicht das Senden und Empfangen von Telegrammen entsprechend der [Telegrammsyntax](/de/logic.bridges.velux/doc/syntax).

    
(G)ateway ist der Standardmodus und kann durch einen Druck der [Enter]-Taste ausgewählt werden. Die Anwendung wird dann beginnen, den Zustand aller am KLF-200 angemeldeten Geräte zu empfangen. Es kann dabei vorkommen, dass nicht alle Geräte ihren Status melden.

    
Die Telegramme, die durch die Anwendung dargestellt bzw. entgegengenommen werden, sind in der [Telegrammsyntax](/de/logic.bridges.velux/doc/syntax) dokumentiert und entsprechen denen, die [Auftraggeber](/de/logic.bridges.velux/gatewayrequest) und [Antwortfilter](/de/logic.bridges.velux/gatewayresponse) verwenden.

    
Mit Hilfe der #min und #max Parameter können die minimalen und maximalen Werte für die Geräte festgelegt werden. Diese sind nicht immer selbsterklärend...

    
- Rollläden, Markisen, Dachfenster, Shades: #min = komplett eingefahren, #max = komplett ausgefahren / geöffnet
- Leuchten, Heizungen: #min = Licht / Heizung eingeschaltet, #max = Licht / Heizung ausgeschaltet...

    
Mit Hilfe des Testers können so die Zielwerte für verschiedene Geräte ermittelt und dann entweder über die bereitgestellten Logikbausteine oder über einfache Telegrammgeneratoren / Textformatierer in einem X1-Logikblatt verwendet werden.
