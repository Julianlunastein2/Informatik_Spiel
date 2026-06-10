Imports System.IO
Imports System.Threading
Imports System.Runtime.InteropServices

Module Module1

    Const NO_KEY = 0
    Const CURSOR_LEFT = 1
    Const CURSOR_RIGHT = 2
    Const SPACE_BAR = 3
    Const UNKNOWN_KEY = 99
    '============================================================================================
    'Spuren Einstellungen: Hier kann die Anzahl der Spuren angepasst werden, sowie die Breite der Spuren, damit das Spiel variabler wird. 
    Const SPUREN_ANZAHL = 10     'Variabel um die Anzahl der Spuren einzustellen !!!
    Const SPUR_BREITE = 12
    Const SPALTE_MAX = SPUREN_ANZAHL * SPUR_BREITE
    '===========================================================================================
    Const ZEILE_MAX = 24
    Const A_MIN = 1
    Const A_MAX_START = 2
    Const G_MIN = 1
    Const G_Max = 9
    Const P_MIN = 0
    Const P_MAX = 79
    Const SPIELFIGUR = 15

    Function Tastatur_Abfrage() As Integer
        Dim cki As New ConsoleKeyInfo()
        If Console.KeyAvailable = False Then
            Return NO_KEY
        Else
            cki = Console.ReadKey(True)
            If cki.Key = ConsoleKey.LeftArrow Then
                Return CURSOR_LEFT
            ElseIf cki.Key = ConsoleKey.RightArrow Then
                Return CURSOR_RIGHT
            ElseIf cki.Key = ConsoleKey.Spacebar Then
                Return SPACE_BAR
            Else
                Return UNKNOWN_KEY
            End If
        End If
    End Function


    '=========================================================================================================================
    'Hindernis und Spielfeld -Generierung
    '=========================================================================================================================


    Sub ZeilenErzeugung(ByRef Zeile() As Char, ByVal a_max As Integer, ByVal idx As Integer, ByVal auto_schicht As Integer, ByRef autoInSpur() As Boolean, ByRef spawnCooldown As Integer, ByRef itemWarscheinlichkeit As Single)

        'Deklarieren der Variablen
        'Dim A As Integer'    'Anzahl der Hindernisblocks
        Dim X As Single
        Dim i As Integer
        Dim G As Integer    'Größe des Hindernisblocks
        Dim P As Integer    'Position des Hindernisblocks



        'Auto Abbildung
        Dim Auto As String() = {"   _____",'7
                                "  /_..._\",'8
                                " (0[###]0)",'9
                                "  `'   `'"} '8



        'Zeilenvektor mit Leerzeichen füllen
        For i = 0 To SPALTE_MAX
            Zeile(i) = " "
        Next

        'Zeile vorher komplett füllen (verhidnert Fehler mit dem Rand
        If idx = 1 OrElse idx = 2 Then
            Zeile(SPALTE_MAX) = " "
        End If

        'Anzahl A der Hindernisblocks zufällig ermitteln
        Randomize()
        X = VBMath.Rnd

        'A = (a_max - A_MIN) * X + A_MIN
        'Console.WriteLine(A)

        'Für jeden der A Hindernisblocks:
        For i = 0 To SPUREN_ANZAHL - 1 'Ändern zu SPUREN_ANZAHL, damit die Anzahl der Spuren variabel ist

            'Spuren Begrenzung einfügen, alle 3 zeilen kommt kein strich

            P = 0 + (SPUR_BREITE * i)

            If idx = 1 OrElse idx = 2 Then
                Zeile(P) = "|"

            ElseIf idx = 0 Then
                Zeile(P) = " "
            End If

            'Console.WriteLine(idx)

            'Größe G des Hindernisblocks zufällig ermitteln

            'Entscheiden ob ein neues Auto "gedruckt" wird anahnd auto_schicht, um Überlappung zu vermeiden und grafik fehler zu vermeiden
            If auto_schicht = 3 Then

                If spawnCooldown Then
                    spawnCooldown = spawnCooldown - 1
                    autoInSpur(i) = False

                Else
                End If

                Randomize()
                X = VBMath.Rnd
                If X < 0.25 Then '25% Chance auf Auto
                    autoInSpur(i) = True
                Else
                    autoInSpur(i) = False
                End If
            End If

            'Auto ausgeben wenn autoInSpur = true
            If autoInSpur(i) = True Then

                P = 1 + (SPUR_BREITE * i)

                'Autoförmige Hindernisse anhand des Auto-Arrays in den Zeilen, zwischen den Fahrbahnmakierungen ausgeben
                If auto_schicht >= 0 Then

                    For j = 0 To Auto(auto_schicht).Length - 1
                        Zeile(P + j) = Auto(auto_schicht)(j)
                    Next

                End If
            End If

            'Item Generierung
            Randomize()
            X = VBMath.Rnd
            If X < itemWarscheinlichkeit Then 'Chance auf Item

                P = (SPUR_BREITE / 2) + (SPUR_BREITE * i)

                If idx = 1 OrElse idx = 2 Then
                    'Zufällig entscheiden welches Item gespawnt wird: +, B (Bier) oder * (Stern) oder "S" (Speed)
                    Dim r As Single
                    Randomize()
                    r = VBMath.Rnd
                    If r < 0.25 Then
                        Zeile(P) = "+"
                    ElseIf r < 0.5 Then
                        Zeile(P) = "B" 'Bier
                    ElseIf r < 0.75 Then
                        Zeile(P) = "*" 'Stern
                    Else
                        Zeile(P) = "S" 'Speed Boost
                    End If
                End If
            End If

        Next
        Exit Sub

        ''G = (G_Max - G_MIN) * X + G_MIN
        ''console.WriteLine("G: " & G)

        ''Startposition P des Hindernisblocks zufällig ermitteln
        'Randomize()
        'X = VBMath.Rnd

        'P = (SPALTE_MAX - P_MIN) * X + P_MIN
        ''Console.WriteLine("P: " & P)

        ''Für jedes der G Einzelhindernisse:
        'For j = 1 To G

        '    'Prüfen ob Hinderniss innerhalb des Wertebereichs ist
        '    If P + j - 1 <= SPALTE_MAX Then

        '        'Hinderniss an Position P+j-1 in den Zeilenvektor eintragen


        '    End If

        'Next



        ''Ausgabe zum Test
        'For i = 0 To SPALTE_MAX
        '    Console.Write(Zeile(i))
        'Next
        'Console.WriteLine()


    End Sub

    '=========================================================================================================================
    'Gameover Screen
    '=========================================================================================================================

    Sub Gameover(ByVal finalScore As Integer)
        Console.BackgroundColor = ConsoleColor.Red
        Console.ForegroundColor = ConsoleColor.White
        Console.Clear()
        'Game over Screen
        Console.WriteLine("



              ▄████  ▄▄▄       ███▄ ▄███▓▓█████     ▒█████   ██▒   █▓▓█████  ██▀███  
             ██▒ ▀█▒▒████▄    ▓██▒▀█▀ ██▒▓█   ▀    ▒██▒  ██▒▓██░   █▒▓█   ▀ ▓██ ▒ ██▒
            ▒██░▄▄▄░▒██  ▀█▄  ▓██    ▓██░▒███      ▒██░  ██▒ ▓██  █▒░▒███   ▓██ ░▄█ ▒
            ░▓█  ██▓░██▄▄▄▄██ ▒██    ▒██ ▒▓█  ▄    ▒██   ██░  ▒██ █░░▒▓█  ▄ ▒██▀▀█▄  
            ░▒▓███▀▒ ▓█   ▓██▒▒██▒   ░██▒░▒████▒   ░ ████▓▒░   ▒▀█░  ░▒████▒░██▓ ▒██▒
             ░▒   ▒  ▒▒   ▓▒█░░ ▒░   ░  ░░░ ▒░ ░   ░ ▒░▒░▒░    ░ ▐░  ░░ ▒░ ░░ ▒▓ ░▒▓░
              ░   ░   ▒   ▒▒ ░░  ░      ░ ░ ░  ░     ░ ▒ ▒░    ░ ░░   ░ ░  ░  ░▒ ░ ▒░
            ░ ░   ░   ░   ▒   ░      ░      ░      ░ ░ ░ ▒       ░░     ░     ░░   ░ 
                  ░       ░  ░       ░      ░  ░       ░ ░        ░     ░  ░   ░     
                                                                 ░                   
")
        Console.ReadLine()

        'Nach Game Over wieder normale Farben 

        Console.WriteLine("=========================================")
        Console.WriteLine("GAME OVER! Dein finaler Punktestand: " & finalScore)
        Console.WriteLine("=========================================")
        Console.WriteLine()

        ' Namensabfrage für das Scoreboard
        Console.Write("Bitte gib deinen Namen ein: ")
        Dim name As String = Console.ReadLine()
        If String.IsNullOrWhiteSpace(name) Then name = "Unbekannt"

        ' Passende Datei anhand der aktuellen Schwierigkeit wählen
        Dim dateiName As String = ""
        Select Case aktuelleSchwierigkeit
            Case Schwierigkeit.Leicht : dateiName = "highscores_leicht.txt"
            Case Schwierigkeit.Mittel : dateiName = "highscores_mittel.txt"
            Case Schwierigkeit.Schwer : dateiName = "highscores_schwer.txt"
        End Select

        ' Scoreboard füttern
        ScoreboardSpeichern(dateiName, name, finalScore)

        Console.WriteLine()
        Console.WriteLine("Score erfolgreich gespeichert! Drücke Enter...")
        Console.ReadLine()
        Console.Clear()

        Console.BackgroundColor = ConsoleColor.Black
        Console.Clear()

    End Sub


    '=========================================================================================================================
    'Startmenü
    '=========================================================================================================================

    Enum Hauptmenü
        Spielen
        Schwierigkeit
        Scoreboard
        AutoOptik
        Anleitung
        Beenden
    End Enum

    Enum Schwierigkeit
        Leicht
        Mittel
        Schwer
    End Enum

    Dim aktuelleSchwierigkeit As Schwierigkeit = Schwierigkeit.Mittel
    Dim gewaehlteAutoFarbe As ConsoleColor = ConsoleColor.White

    Function Startmenü() As Hauptmenü

        Dim auswahl As Integer = 0
        Dim optionen() As String = {"SPIEL STARTEN", "SCHWIERIGKEIT", "SCOREBOARD", "AUTO OPTIK", "ANLEITUNG", "SPIEL BEENDEN"}

        ' Dein ASCII-Schriftzug zeilenweise im Array hinterlegt
        Dim logo() As String = {
            " ░▒▓██████▓▒░░▒▓████████▓▒░▒▓█▓▒░░▒▓███████▓▒░▒▓████████▓▒░▒▓████████▓▒░▒▓███████▓▒░░▒▓████████▓▒░▒▓██████▓▒░░▒▓█▓▒░░▒▓█▓▒░▒▓████████▓▒░▒▓███████▓▒░ ",
            "░▒▓█▓▒░░▒▓█▓▒░▒▓█▓▒░      ░▒▓█▓▒░▒▓█▓▒░         ░▒▓█▓▒░   ░▒▓█▓▒░      ░▒▓█▓▒░░▒▓█▓▒░▒▓█▓▒░     ░▒▓█▓▒░░▒▓█▓▒░▒▓█▓▒░░▒▓█▓▒░▒▓█▓▒░       ░▒▓█▓▒░░▒▓█▓▒░ ",
            "░▒▓█▓▒░      ░▒▓█▓▒░      ░▒▓█▓▒░▒▓█▓▒░         ░▒▓█▓▒░   ░▒▓█▓▒░      ░▒▓█▓▒░░▒▓█▓▒░▒▓█▓▒░     ░▒▓█▓▒░░▒▓█▓▒░▒▓█▓▒░░▒▓█▓▒░▒▓█▓▒░       ░▒▓█▓▒░░▒▓█▓▒░ ",
            "░▒▓█▓▒▒▓███▓▒░▒▓██████▓▒░ ░▒▓█▓▒░░▒▓██████▓▒░   ░▒▓█▓▒░   ░▒▓██████▓▒░ ░▒▓███████▓▒░░▒▓██████▓▒░░▒▓████████▓▒░▒▓████████▓▒░▒▓██████▓▒░ ░▒▓███████▓▒░ ",
            "░▒▓█▓▒░░▒▓█▓▒░▒▓█▓▒░      ░▒▓█▓▒░      ░▒▓█▓▒░  ░▒▓█▓▒░   ░▒▓█▓▒░      ░▒▓█▓▒░░▒▓█▓▒░▒▓█▓▒░     ░▒▓█▓▒░░▒▓█▓▒░▒▓█▓▒░░▒▓█▓▒░▒▓█▓▒░       ░▒▓█▓▒░░▒▓█▓▒░ ",
            "░▒▓█▓▒░░▒▓█▓▒░▒▓█▓▒░      ░▒▓█▓▒░      ░▒▓█▓▒░  ░▒▓█▓▒░   ░▒▓█▓▒░      ░▒▓█▓▒░░▒▓█▓▒░▒▓█▓▒░     ░▒▓█▓▒░░▒▓█▓▒░▒▓█▓▒░░▒▓█▓▒░▒▓█▓▒░       ░▒▓█▓▒░░▒▓█▓▒░ ",
            " ░▒▓██████▓▒░░▒▓████████▓▒░▒▓█▓▒░▒▓███████▓▒░   ░▒▓█▓▒░   ░▒▓████████▓▒░▒▓█▓▒░░▒▓█▓▒░▒▓█▓▒░     ░▒▓█▓▒░░▒▓█▓▒░▒▓█▓▒░░▒▓█▓▒░▒▓████████▓▒░▒▓█▓▒░░▒▓█▓▒░ "
        }

        Dim credits As String = "by Noah, Jonas und Julian"

        ' Da das Logo 152 Zeichen breit ist, passen wir das Konsolenfenster dynamisch an,
        ' damit es zu keinen unschönen Zeilenumbrüchen kommt.
        Dim benötigteBreite As Integer = logo(0).Length + 4
        If Console.WindowWidth < benötigteBreite Then
            Console.WindowWidth = benötigteBreite
        End If

        Do
            Console.Clear()
            Console.ForegroundColor = ConsoleColor.White

            ' 1. LOGO AUSGEBEN UND HORIZONTAL ZENTRIEREN
            Console.WriteLine()
            Console.WriteLine()
            For i As Integer = 0 To logo.Length - 1
                Dim xPosLogo As Integer = (Console.WindowWidth - logo(i).Length) / 2
                Console.SetCursorPosition(xPosLogo, 2 + i)
                Console.Write(logo(i))
            Next

            ' 2. CREDITS DIREKT DARUNTER ZENTRIEREN
            Console.WriteLine()
            Dim xPosCredits As Integer = (Console.WindowWidth - credits.Length) / 2
            Console.SetCursorPosition(xPosCredits, 2 + logo.Length + 1)
            Console.ForegroundColor = ConsoleColor.Gray
            Console.Write(credits)
            Console.ForegroundColor = ConsoleColor.White

            ' 3. MENÜPUNKTE MITTIG POSITIONIEREN
            ' Wir starten ein paar Zeilen unter den Credits
            Dim startZeileMenü As Integer = 2 + logo.Length + 5

            For i As Integer = 0 To optionen.Length - 1
                Dim optText As String = optionen(i)

                ' Wenn der Punkt ausgewählt ist, fügen wir die Arcade-Pfeile hinzu
                If i = auswahl Then
                    optText = "   > " & optText & " <   "
                    Console.ForegroundColor = ConsoleColor.DarkYellow ' Microsofts Standard-Kombination für Orange
                Else
                    optText = "     " & optText & "     "
                    Console.ForegroundColor = ConsoleColor.White
                End If

                ' Berechne die Mitte basierend auf der aktuellen Fensterbreite
                Dim xPosOption As Integer = (Console.WindowWidth - optText.Length) / 2
                Console.SetCursorPosition(xPosOption, startZeileMenü + (i * 2)) ' (i * 2) sorgt für Leerzeilen zwischen den Punkten
                Console.Write(optText)
            Next

            ' Steuerungshinweis ganz unten zentriert platzieren
            Dim steuerung As String = "[ Mit Pfeiltasten steuern & Enter bestätigen ]"
            Dim xPosSteuerung As Integer = (Console.WindowWidth - steuerung.Length) / 2
            Console.SetCursorPosition(xPosSteuerung, Console.WindowHeight - 3)
            Console.ForegroundColor = ConsoleColor.DarkGray
            Console.Write(steuerung)

            ' Tasteneingabe lesen
            Dim taste As ConsoleKey = Console.ReadKey(True).Key

            ' Menü-Navigation per Pfeiltasten
            Select Case taste
                Case ConsoleKey.UpArrow
                    auswahl -= 1
                    If auswahl < 0 Then auswahl = optionen.Length - 1
                Case ConsoleKey.DownArrow
                    auswahl += 1
                    If auswahl >= optionen.Length Then auswahl = 0
                Case ConsoleKey.Enter
                    Select Case auswahl
                        Case 0 : Return Hauptmenü.Spielen
                        Case 1 : Return Hauptmenü.Schwierigkeit
                        Case 2 : Return Hauptmenü.Scoreboard
                        Case 3 : Return Hauptmenü.AutoOptik
                        Case 4 : Return Hauptmenü.Anleitung
                        Case 5 : Return Hauptmenü.Beenden
                    End Select
            End Select

        Loop

    End Function


    '=========================================================================================================================
    'Grundlegender Spielablauf
    '=========================================================================================================================

    Sub Spielablauf()
        Dim leben As Integer
        Dim spielfeld(ZEILE_MAX, SPALTE_MAX) As Char
        Dim Zeile(SPALTE_MAX) As Char
        Dim z As Integer
        Dim s As Integer
        Dim Taste As Integer
        Dim SpielfigurPos As Integer
        Dim i As Integer
        Dim Wartezeit As Single
        Dim a_max As Single
        Dim idx = 0  'Zähler für die Spurenbegrenzung
        Dim auto_schicht As Integer = 3 'Zähler für die Autoabildung
        Dim autoInSpur(SPUREN_ANZAHL - 1) As Boolean 'Variable um zu entscheiden ob ein Auto in der Spur ist oder nicht, damit es nicht in jeder Zeile ein Auto gibt
        Dim spawnCooldown As Integer = 0 'Cooldown um zu verhindern dass in jeder Zeile ein Auto spawnt, eleminiert langweilige Optik von einem "Block" Gegner
        Dim unverwundbar As DateTime = DateTime.MinValue 'Variable um die Dauer der Unverwundbarkeit zu speichern
        Dim bierEffektBis As DateTime = DateTime.MinValue 'Speichert, bis wann die Steuerung vertauscht ist
        Dim speedBoostBis As DateTime = DateTime.MinValue 'Speichert, bis wann der Speedboost aktiv ist
        Dim score As Integer = 0 'Unser Punktezähler
        Dim itemWarscheinlichkeit As Single 'Variable um die Warscheinlichkeit zu speichern, mit der Items spawnen, wird in Schwierigkeit angepasst
        Dim minWartezeit As Integer 'Definiert die schnellste Geschwindigkeit des Games

        'Startwerte basierend auf der Schwierigkeit setzen
        Select Case aktuelleSchwierigkeit

            Case Schwierigkeit.Leicht
                leben = 7          ' Mehr Leben
                a_max = 1.5        ' Weniger Gegner am Anfang
                itemWarscheinlichkeit = 0.1 ' Chance auf Items
                Wartezeit = 250    ' Auto fährt langsamer am Anfang
                minWartezeit = 50

            Case Schwierigkeit.Mittel
                leben = 5
                a_max = A_MAX_START
                itemWarscheinlichkeit = 0.08 ' Chance auf Items
                Wartezeit = 200
                minWartezeit = 40

            Case Schwierigkeit.Schwer
                leben = 3          ' Weniger Leben
                a_max = 3.0        ' Höhere Gegnerdichte von Beginn an
                itemWarscheinlichkeit = 0.02 ' Geringere Chance auf Items
                Wartezeit = 130    ' Autos sind verdammt schnell!
                minWartezeit = 20

        End Select

        SpielfigurPos = SPALTE_MAX / 2

        'Hauptschleife des Spiels
        Do
            'neue Zeile erzeugen
            ZeilenErzeugung(Zeile, a_max, idx, auto_schicht, autoInSpur, spawnCooldown, itemWarscheinlichkeit)

            'Auto Schicht um auto von hinten auszubenen, damit es von oben nach unten fährt (ohne dass out of array fehler erzeugt wird)
            auto_schicht = auto_schicht - 1
            If auto_schicht <= -1 Then auto_schicht = 3


            idx = idx + 1
            If idx > 2 Then idx = 0

            'Alle Zeilen des Spielfelds um eine Zeile nach unten verschieben
            'Rückwärtschleife über zeilen
            For z = ZEILE_MAX To 1 Step -1
                'Vorwärtschleife über Spalten
                For s = 0 To SPALTE_MAX
                    'Eine Zelle nach unten kopieren
                    spielfeld(z, s) = spielfeld(z - 1, s)

                Next
            Next
            'Neue Zeile am oberen Rand des Spielfelds einfügen
            For s = 0 To SPALTE_MAX
                spielfeld(0, s) = Zeile(s)
            Next

            ' --- SPIELFELD MIT RAHMEN ZEICHNEN (VORDERGRUND-EFFEKTE) ---
            Console.SetCursorPosition(0, 0)
            Console.BackgroundColor = ConsoleColor.Black ' Hintergrund bleibt immer sauber schwarz

            ' Globale Textfarbe basierend auf aktiven Effekten bestimmen
            Dim effektFarbe As ConsoleColor = ConsoleColor.White

            If DateTime.Now < unverwundbar Then
                effektFarbe = ConsoleColor.Yellow       ' Gelber Text bei Stern (Unverwundbar)
            ElseIf DateTime.Now < bierEffektBis Then
                effektFarbe = ConsoleColor.Green        ' Grüner Text bei Bier (Betrunken)
            ElseIf DateTime.Now < speedBoostBis Then
                effektFarbe = ConsoleColor.Cyan         ' Cyanfarbener Text bei Speed-Boost
            End If

            ' 1. Oberer Rahmen (Leuchtet in der Effektfarbe, wenn einer aktiv ist, sonst Cyan)
            If effektFarbe <> ConsoleColor.White Then
                Console.ForegroundColor = effektFarbe
            Else
                Console.ForegroundColor = ConsoleColor.Cyan
            End If
            Console.Write("╔" & New String("═", SPALTE_MAX + 1) & "╗")
            Console.WriteLine()

            ' 2. Spielfeld-Inhalt mit Seitenrändern
            For z = 0 To ZEILE_MAX - 2
                ' Linker Rahmen
                If effektFarbe <> ConsoleColor.White Then
                    Console.ForegroundColor = effektFarbe
                Else
                    Console.ForegroundColor = ConsoleColor.Cyan
                End If
                Console.Write("║")

                ' Inhalt zeichnen
                For s = 0 To SPALTE_MAX
                    If spielfeld(z, s) = "|" Then
                        ' Spurmarkierungen leicht abdunkeln, außer ein Effekt ist aktiv
                        If effektFarbe <> ConsoleColor.White Then
                            Console.ForegroundColor = effektFarbe
                        Else
                            Console.ForegroundColor = ConsoleColor.DarkGray
                        End If
                        Console.Write(spielfeld(z, s))
                    Else
                        ' Normaler Spielinhalt (Gegner, Straße) nimmt die Effektfarbe an
                        Console.ForegroundColor = effektFarbe
                        Console.Write(spielfeld(z, s))
                    End If
                Next

                ' Rechter Rahmen
                If effektFarbe <> ConsoleColor.White Then
                    Console.ForegroundColor = effektFarbe
                Else
                    Console.ForegroundColor = ConsoleColor.Cyan
                End If
                Console.WriteLine("║")
            Next

            ' 3. Unterer Rahmen
            If effektFarbe <> ConsoleColor.White Then
                Console.ForegroundColor = effektFarbe
            Else
                Console.ForegroundColor = ConsoleColor.Cyan
            End If
            Console.Write("╚" & New String("═", SPALTE_MAX + 1) & "╝")
            Console.WriteLine()

            ' Farbe für den nachfolgenden Code zurücksetzen
            Console.ForegroundColor = ConsoleColor.White
            ' -----------------------------------------------------------

            For i = 1 To SPIELFIGUR

                'Tastatur abfragen
                Taste = Tastatur_Abfrage()
                'Console.WriteLine("Taste: " & Taste)

                ' === PAUSENMENÜ ===
                If Taste = SPACE_BAR Then
                    Dim pauseStart As DateTime = DateTime.Now ' Zeit merken, wann Pause gedrückt wurde

                    Dim istBetrunken As Boolean = (pauseStart < bierEffektBis)
                    Dim istImBoost As Boolean = (pauseStart < speedBoostBis)

                    Dim zumHauptmenüAbbrechen As Boolean = PausenMenüAnzeigen(score, leben, istBetrunken, istImBoost)

                    If zumHauptmenüAbbrechen Then
                        Exit Sub
                    End If

                    ' Differenz berechnen, wie lange das Spiel pausiert war
                    Dim pauseDauer As TimeSpan = DateTime.Now - pauseStart

                    ' Alle aktiven Timer um die Dauer der Pause nach hinten verschieben
                    If unverwundbar > pauseStart Then unverwundbar = unverwundbar.Add(pauseDauer)
                    If bierEffektBis > pauseStart Then bierEffektBis = bierEffektBis.Add(pauseDauer)
                    If speedBoostBis > pauseStart Then speedBoostBis = speedBoostBis.Add(pauseDauer)
                End If

                'Spielfigur an alter Position löschen
                For h As Integer = 1 To 4
                    Console.SetCursorPosition(SpielfigurPos + 1, ZEILE_MAX - h)
                    Console.Write("         ")
                Next

                ' Position der Spielfigur ermitteln (mit Bier-Effekt-Prüfung)
                If DateTime.Now < bierEffektBis Then
                    ' EFFEKT AKTIV: Steuerbefehle sind vertauscht!
                    If Taste = CURSOR_LEFT Then
                        SpielfigurPos += 1 ' Eigentlich links gedrückt, aber Auto fährt nach RECHTS
                    End If

                    If Taste = CURSOR_RIGHT Then
                        SpielfigurPos -= 1 ' Eigentlich rechts gedrückt, aber Auto fährt nach LINKS
                    End If
                Else
                    ' NORMALER ZUSTAND: Alles wie gewohnt
                    If Taste = CURSOR_LEFT Then
                        SpielfigurPos -= 1
                    End If

                    If Taste = CURSOR_RIGHT Then
                        SpielfigurPos += 1
                    End If
                End If

                'Begrenzung der Spielfigur auf dem Spielfeld
                If SpielfigurPos < 0 Then
                    SpielfigurPos = 0
                End If

                'If SpielfigurPos > SPALTE_MAX Then
                '    SpielfigurPos = SPALTE_MAX

                If SpielfigurPos > SPALTE_MAX - 8 Then
                    SpielfigurPos = SPALTE_MAX - 8
                End If

                'Kollisionserkennung
                Dim kollisionErkannt As Boolean = False

                ' Prüfen, ob aktuell IRGENDEIN Statuseffekt aktiv ist
                Dim effektAktiv As Boolean = (DateTime.Now < unverwundbar) OrElse
                                             (DateTime.Now < bierEffektBis) OrElse
                                             (DateTime.Now < speedBoostBis)

                For f As Integer = 1 To 4
                    For h As Integer = 1 To 8
                        Dim symbol As Char = spielfeld(ZEILE_MAX - f, SpielfigurPos + h)

                        If symbol = " " Or symbol = "|" Or symbol = Chr(0) Then
                            'Keine Kollision, weitergehen

                        ElseIf symbol = "+" Then
                            leben = leben + 1
                            spielfeld(ZEILE_MAX - f, SpielfigurPos + h) = " " ' Item auf dem Feld löschen

                        ElseIf symbol = "B" Then
                            ' Nur einsammeln, wenn kein anderer Effekt aktiv ist
                            If Not effektAktiv Then
                                'Bier -> Item 3 (Kontrollen vertauscht)
                                bierEffektBis = DateTime.Now.AddSeconds(15) ' Effekt für 15 Sekunden aktivieren
                                spielfeld(ZEILE_MAX - f, SpielfigurPos + h) = " " ' Item auf dem Feld löschen
                            End If

                        ElseIf symbol = "*" Then
                            ' Nur einsammeln, wenn kein anderer Effekt aktiv ist
                            If Not effektAktiv Then
                                'Stern -> Unverwundbarkeit (Item 1)
                                unverwundbar = DateTime.Now.AddSeconds(10)
                                spielfeld(ZEILE_MAX - f, SpielfigurPos + h) = " " ' Item auf dem Feld löschen
                            End If

                        ElseIf symbol = "S" Then
                            ' Nur einsammeln, wenn kein anderer Effekt aktiv ist
                            If Not effektAktiv Then
                                'Speed Boost -> Item 2
                                speedBoostBis = DateTime.Now.AddSeconds(10)
                                spielfeld(ZEILE_MAX - f, SpielfigurPos + h) = " " ' Item auf dem Feld löschen
                            End If

                            ' Wenn es kein Item, keine Leerstelle und keine Wand ist, ist es ein gegnerisches Auto
                        Else
                            kollisionErkannt = True
                        End If
                    Next
                Next

                ' =========================================================================================
                ' Spurbasierte Löschung bei Crash (erwischt Autos oben drüber, links & rechts)
                ' =========================================================================================
                If kollisionErkannt Then

                    ' prüfen, welche der x Spuren (0 bis SPUREN_ANZAHL - 1) die Spielfigur gerade berührt
                    For i_spur As Integer = 0 To SPUREN_ANZAHL - 1
                        Dim spurStart As Integer = 1 + (SPUR_BREITE * i_spur)
                        Dim spurEnde As Integer = spurStart + SPUR_BREITE - 2 ' Ein Auto ist ca. 8-9 Zeichen breit

                        ' Wenn sich die Spielfigur (Breite 8) im Bereich dieser Spur befindet
                        If (SpielfigurPos + 8 >= spurStart) AndAlso (SpielfigurPos <= spurEnde) Then

                            ' Lösche die betroffene Spur im unteren Bereich (die untersten 8 Zeilen) komplett.
                            For z_del As Integer = 1 To 12
                                For s_del As Integer = spurStart To spurStart + 9
                                    If s_del <= SPALTE_MAX Then
                                        spielfeld(ZEILE_MAX - z_del, s_del) = " "
                                    End If
                                Next
                            Next

                        End If
                    Next

                    ' Schaden berechnen: Nur abziehen, wenn der Spieler NICHT unverwundbar ist
                    If DateTime.Now >= unverwundbar Then
                        leben = leben - 1
                        ' --- ROTES AUFLEUCHTEN START ---
                        Console.BackgroundColor = ConsoleColor.Red ' Hintergrund auf Rot setzen
                        Console.Clear()
                        Console.Beep()                              ' Dein Schadens-Sound
                        Threading.Thread.Sleep(10)                  ' 50 Millisekunden warten (sehr kurzes Aufblitzen)
                        Console.BackgroundColor = ConsoleColor.Black ' Hintergrund wieder zurück auf Schwarz setzen
                        Console.Clear()
                        ' --- ROTES AUFLEUCHTEN ENDE ---
                    Else
                        ' Wenn der Spieler unverwundbar ist, erhält er stattdessen Punkte für das "Durchfahren" des Autos
                        score += 10
                    End If
                End If

                'Spielfigur auf der Konsole ausgeben (mit Farbe)

                Console.ForegroundColor = gewaehlteAutoFarbe

                Console.SetCursorPosition(SpielfigurPos + 2, ZEILE_MAX - 1)
                Console.Write("`'   `'")
                Console.SetCursorPosition(SpielfigurPos + 1, ZEILE_MAX - 2)
                Console.Write("(0[###]0)")
                Console.SetCursorPosition(SpielfigurPos + 2, ZEILE_MAX - 3)
                Console.Write("/_..._\")
                Console.SetCursorPosition(SpielfigurPos + 3, ZEILE_MAX - 4)
                Console.Write("_____")

                Console.ForegroundColor = ConsoleColor.White

                'ANzeigen der Leben und Statuseffekte mit Timer
                Dim restSekunden As Integer = 0
                Dim statusText As String = ""

                If DateTime.Now < unverwundbar Then
                    Console.ForegroundColor = ConsoleColor.Yellow
                    restSekunden = CInt(Math.Ceiling((unverwundbar - DateTime.Now).TotalSeconds))
                    statusText = $"[ UNVERWUNDBAR ({restSekunden}s) ]"

                ElseIf DateTime.Now < bierEffektBis Then
                    Console.ForegroundColor = ConsoleColor.Green
                    restSekunden = CInt(Math.Ceiling((bierEffektBis - DateTime.Now).TotalSeconds))
                    statusText = $"[ STEUERUNG VERTREHT! ({restSekunden}s) ]"

                ElseIf DateTime.Now < speedBoostBis Then
                    Console.ForegroundColor = ConsoleColor.Cyan
                    restSekunden = CInt(Math.Ceiling((speedBoostBis - DateTime.Now).TotalSeconds))
                    statusText = $"[ SPEED BOOST (2x PUNKTE)! ({restSekunden}s) ]"
                Else
                    Console.ForegroundColor = ConsoleColor.White
                End If

                ' Saubere Ausgabe in der Statuszeile (alte Reste werden mit Leerzeichen überschrieben)
                Console.SetCursorPosition(0, ZEILE_MAX + 1)
                Console.Write("Leben: " & leben & " | Punkte: " & score & "   " & statusText & "                                                                  ")
                Console.ForegroundColor = ConsoleColor.White

                ' Warten (wird durch Speedboost beeinflusst)
                If DateTime.Now < speedBoostBis Then
                    ' Halbe Wartezeit = Doppelte Geschwindigkeit!
                    Threading.Thread.Sleep((Wartezeit / SPIELFIGUR) * 0.1)
                Else
                    ' Normale Geschwindigkeit
                    Threading.Thread.Sleep(Wartezeit / SPIELFIGUR)
                End If

            Next
            'Tastaturpuffer leeren
            Do
                Taste = Tastatur_Abfrage()
            Loop Until Taste = NO_KEY

            'Wartezeit verkürzen
            If Wartezeit > minWartezeit Then
                Wartezeit = Wartezeit * 0.99
                Console.WriteLine("Neue Wartezeit: " & Wartezeit & " ")

            End If
            'Console.SetCursorPosition(15, ZEILE_MAX)
            'Console.Write("Wartezeit: " & Wartezeit)

            'Hindernissdichte erhöhen
            If a_max < 10 Then
                a_max = a_max * 1.01
            End If

            ' Punkte vergeben (Am Ende der Hauptschleife einfügen)
            If DateTime.Now < speedBoostBis Then
                score += 2 ' Doppelter Punktgewinn im Boost!
            Else
                score += 1 ' Normaler Punktgewinn
            End If

        Loop Until leben <= 0

        Gameover(score)




    End Sub

    '=========================================================================================================================
    'Sub der die Schwierigkeit des Spiels anpasst
    '=========================================================================================================================

    Sub SchwierigkeitAnpassen()

        Dim taste As ConsoleKey
        Do
            Console.Clear()
            Console.ForegroundColor = ConsoleColor.White
            Console.WriteLine("=====================================")
            Console.WriteLine("        SCHWIERIGKEIT EINSTELLEN     ")
            Console.WriteLine("=====================================")
            Console.WriteLine()

            ' Textfarbe auf Orange für die aktive Auswahl setzen, danach wieder auf Weiß
            If aktuelleSchwierigkeit = Schwierigkeit.Leicht Then
                Console.ForegroundColor = ConsoleColor.DarkYellow
                Console.Write("  > [ LEICHT ] <")
                Console.ForegroundColor = ConsoleColor.White
                Console.WriteLine("    Mittel      Schwer")

            ElseIf aktuelleSchwierigkeit = Schwierigkeit.Mittel Then
                Console.Write("    Leicht    ")
                Console.ForegroundColor = ConsoleColor.DarkYellow
                Console.Write("> [ MITTEL ] <")
                Console.ForegroundColor = ConsoleColor.White
                Console.WriteLine("    Schwer")

            ElseIf aktuelleSchwierigkeit = Schwierigkeit.Schwer Then
                Console.Write("    Leicht      Mittel    ")
                Console.ForegroundColor = ConsoleColor.DarkYellow
                Console.WriteLine("> [ SCHWER ] <")
                Console.ForegroundColor = ConsoleColor.White
            End If

            Console.WriteLine()
            Console.WriteLine("=====================================")
            Console.WriteLine("[ Pfeiltasten Links/Rechts zum Ändern")
            Console.WriteLine("  Enter zum Bestätigen ]")

            taste = Console.ReadKey(True).Key

            ' KORREKTUR: Auswahl per Pfeiltasten mit ElseIf, um den Domino-Effekt zu verhindern
            If taste = ConsoleKey.LeftArrow Then
                If aktuelleSchwierigkeit = Schwierigkeit.Schwer Then
                    aktuelleSchwierigkeit = Schwierigkeit.Mittel
                ElseIf aktuelleSchwierigkeit = Schwierigkeit.Mittel Then
                    aktuelleSchwierigkeit = Schwierigkeit.Leicht
                ElseIf aktuelleSchwierigkeit = Schwierigkeit.Leicht Then
                    aktuelleSchwierigkeit = Schwierigkeit.Schwer
                End If

            ElseIf taste = ConsoleKey.RightArrow Then
                If aktuelleSchwierigkeit = Schwierigkeit.Leicht Then
                    aktuelleSchwierigkeit = Schwierigkeit.Mittel
                ElseIf aktuelleSchwierigkeit = Schwierigkeit.Mittel Then
                    aktuelleSchwierigkeit = Schwierigkeit.Schwer
                ElseIf aktuelleSchwierigkeit = Schwierigkeit.Schwer Then
                    aktuelleSchwierigkeit = Schwierigkeit.Leicht
                End If
            End If

        Loop Until taste = ConsoleKey.Enter

        Console.Clear()
    End Sub

    '=========================================================================================================================
    ' Scoreboard-Logik: Laden, Speichern (nach Punkten sortiert) und Anzeigen (3 Spalten nebeneinander)
    '=========================================================================================================================

    Sub ScoreboardSpeichern(ByVal dateiName As String, ByVal spielerName As String, ByVal score As Integer)
        Dim eintraege As New List(Of String)()

        ' 1. Bestehende Einträge laden, falls die Datei schon existiert
        If File.Exists(dateiName) Then
            eintraege.AddRange(File.ReadAllLines(dateiName))
        End If

        ' 2. Neuen Eintrag im Format "Name - Punkte" hinzufügen
        eintraege.Add(spielerName & " - " & score)

        ' 3. Nach Punkten sortieren (Absteigend) und nur die besten 10 behalten
        Dim sortierteTop10 = eintraege.
            OrderByDescending(Function(zeile)
                                  Dim teile = zeile.Split(New String() {" - "}, StringSplitOptions.None)
                                  Dim punkte As Integer = 0
                                  If teile.Length > 1 Then Integer.TryParse(teile(1), punkte)
                                  Return punkte
                              End Function).
            Take(10).
            ToList()

        ' 4. Die saubere Top 10 zurück in die Datei schreiben
        File.WriteAllLines(dateiName, sortierteTop10)
    End Sub

    '=========================================================================================================================
    ' Sub zur Auswahl der Autofarbe im Hauptmenü
    '=========================================================================================================================
    Sub AutoOptikAnpassen()
        Dim farben() As ConsoleColor = {ConsoleColor.White, ConsoleColor.Red, ConsoleColor.Blue, ConsoleColor.Green, ConsoleColor.Yellow, ConsoleColor.Magenta}
        Dim farbNamen() As String = {"Weiß", "Rot", "Blau", "Grün", "Gelb", "Magenta"}

        ' Aktuelle Position in der Auswahl finden
        Dim auswahl As Integer = Array.IndexOf(farben, gewaehlteAutoFarbe)
        If auswahl = -1 Then auswahl = 0

        Dim taste As ConsoleKey
        Do
            Console.Clear()
            Console.ForegroundColor = ConsoleColor.White
            Console.WriteLine("=====================================")
            Console.WriteLine("          AUTO OPTIK WÄHLEN          ")
            Console.WriteLine("=====================================")
            Console.WriteLine()

            ' Zeige die Farben nebeneinander an
            For i As Integer = 0 To farben.Length - 1
                If i = auswahl Then
                    Console.ForegroundColor = farben(i)
                    Console.Write($"> [ {farbNamen(i)} ] <  ")
                Else
                    Console.ForegroundColor = ConsoleColor.Gray
                    Console.Write($"  {farbNamen(i)}    ")
                End If
            Next

            Console.ForegroundColor = ConsoleColor.White
            Console.WriteLine()
            Console.WriteLine()
            Console.WriteLine("Vorschau deines Rennwagens:")
            Console.ForegroundColor = farben(auswahl)
            Console.WriteLine("    _____")
            Console.WriteLine("   /_..._\")
            Console.WriteLine("  (0[###]0)")
            Console.WriteLine("   `'   `'")

            Console.ForegroundColor = ConsoleColor.White
            Console.WriteLine("=====================================")
            Console.WriteLine("[ Pfeiltasten Links/Rechts zum Ändern | Enter zum Bestätigen ]")

            taste = Console.ReadKey(True).Key

            If taste = ConsoleKey.LeftArrow Then
                auswahl -= 1
                If auswahl < 0 Then auswahl = farben.Length - 1
            ElseIf taste = ConsoleKey.RightArrow Then
                auswahl += 1
                If auswahl >= farben.Length Then auswahl = 0
            End If

        Loop Until taste = ConsoleKey.Enter

        ' Farbe fest speichern
        gewaehlteAutoFarbe = farben(auswahl)
        Console.Clear()
    End Sub

    Sub ScoreboardAnzeigen()
        Console.Clear()
        Console.ForegroundColor = ConsoleColor.Cyan
        Console.WriteLine("=========================================================================================")
        Console.WriteLine("                                    ARCADE HIGHSCORES                                    ")
        Console.WriteLine("=========================================================================================")
        Console.WriteLine()

        ' Spaltenüberschriften positionieren
        Console.ForegroundColor = ConsoleColor.Green
        Console.SetCursorPosition(2, 4) : Console.Write("--- LEICHT ---")
        Console.ForegroundColor = ConsoleColor.Yellow
        Console.SetCursorPosition(32, 4) : Console.Write("--- MITTEL ---")
        Console.ForegroundColor = ConsoleColor.Red
        Console.SetCursorPosition(62, 4) : Console.Write("--- SCHWER ---")
        Console.ForegroundColor = ConsoleColor.White

        ' Alle drei Dateien einlesen (Falls sie existieren, sonst leeres Array)
        Dim leicht() As String = If(File.Exists("highscores_leicht.txt"), File.ReadAllLines("highscores_leicht.txt"), New String() {})
        Dim mittel() As String = If(File.Exists("highscores_mittel.txt"), File.ReadAllLines("highscores_mittel.txt"), New String() {})
        Dim schwer() As String = If(File.Exists("highscores_schwer.txt"), File.ReadAllLines("highscores_schwer.txt"), New String() {})

        ' Herausfinden, welche Datei die meisten Einträge hat
        Dim maxEintraege As Integer = Math.Max(leicht.Length, Math.Max(mittel.Length, schwer.Length))

        ' Wenn überhaupt keine Einträge existieren
        If maxEintraege = 0 Then
            Console.SetCursorPosition(2, 6)
            Console.WriteLine("Noch keine Einträge vorhanden! Fahr ein paar Rennen!")
        Else
            ' Zeige maximal die Top 10 Zeilen an
            Dim zeilenAnzahl As Integer = Math.Min(maxEintraege, 10)

            For i As Integer = 0 To zeilenAnzahl - 1
                Dim startZeile As Integer = 6 + i ' Startet bei Zeile 6 der Konsole

                ' Spalte 1: Leicht
                If i < leicht.Length Then
                    Console.SetCursorPosition(2, startZeile)
                    Console.Write($"{i + 1}. {leicht(i)}")
                End If

                ' Spalte 2: Mittel
                If i < mittel.Length Then
                    Console.SetCursorPosition(32, startZeile)
                    Console.Write($"{i + 1}. {mittel(i)}")
                End If

                ' Spalte 3: Schwer
                If i < schwer.Length Then
                    Console.SetCursorPosition(62, startZeile)
                    Console.Write($"{i + 1}. {schwer(i)}")
                End If
            Next
        End If

        ' Fußzeile ausgeben
        Console.ForegroundColor = ConsoleColor.Cyan
        Console.SetCursorPosition(0, 8 + Math.Min(maxEintraege, 10))
        Console.WriteLine("=========================================================================================")
        Console.ForegroundColor = ConsoleColor.Gray
        Console.WriteLine("[ Drücke Enter für das Hauptmenü ]")
        Console.ReadLine()
    End Sub

    Sub AnleitungAnzeigen()
        Console.Clear()
        Console.ForegroundColor = ConsoleColor.Yellow
        Console.WriteLine("=========================================================================================")
        Console.WriteLine("                                         ANLEITUNG                                         ")
        Console.WriteLine("=========================================================================================")
        Console.WriteLine()
        Console.ForegroundColor = ConsoleColor.White
        Console.WriteLine("Ziel des Spiels:")
        Console.WriteLine("Steuere deinen Wagen durch den dichten Verkehr und erreiche so viele Punkte wie möglich!")
        Console.WriteLine()
        Console.WriteLine("Steuerung:")
        Console.WriteLine("- Benutze die Pfeiltasten LINKS und RECHTS, um deinen Wagen zu bewegen.")
        Console.WriteLine("- Weiche den Autos aus und sammle Power-Ups ein!")
        Console.WriteLine("- Drücke -LEER- um das Spiel zu pausieren und zum Hauptmenü zurückzukehren!")
        Console.WriteLine()
        Console.WriteLine("Power-Ups:")
        Console.WriteLine("- Stern (*) : Unverwundbarkeit für 10 Sekunden.")
        Console.WriteLine("- Bier (B) : Vertauscht die Steuerung für 15 Sekunden (Vorsicht!).")
        Console.WriteLine("- Speed Boost (S) : Verdoppelt deine Punktzahl für 10 Sekunden!")
        Console.WriteLine("- Herz (+) : Gibt dir ein zusätzliches Leben.")
        Console.WriteLine()
        Console.WriteLine("Punktevergabe:")
        Console.WriteLine("- Du erhältst Punkte, indem du so lange wie möglich überlebst.")
        Console.WriteLine("- Jedes Auto, den du im unverwundbaren Zustand durchfährst, bringt zusätzliche Punkte!")
        Console.WriteLine()
        Console.ForegroundColor = ConsoleColor.Cyan
        Console.WriteLine("=========================================================================================")
        Console.ForegroundColor = ConsoleColor.Gray
        Console.WriteLine("[ Drücke Enter für das Hauptmenü ]")
        Console.ReadLine()
    End Sub

    '=========================================================================================================================
    ' Funktion für das Pausenmenü 
    '=========================================================================================================================
    Function PausenMenüAnzeigen(ByVal aktuellerScore As Integer, ByVal aktuelleLeben As Integer, ByVal bierAktiv As Boolean, ByVal boostAktiv As Boolean) As Boolean
        Dim auswahl As Integer = 0
        Dim optionen() As String = {"WEITERSPIELEN", "ZURÜCK ZUM HAUPTMENÜ"}
        Dim taste As ConsoleKey

        Do
            Console.Clear()
            Console.ForegroundColor = ConsoleColor.Cyan
            Console.WriteLine("=====================================")
            Console.WriteLine("            SPIEL PAUSIERT           ")
            Console.WriteLine("=====================================")
            Console.ForegroundColor = ConsoleColor.White
            Console.WriteLine()
            Console.WriteLine($"  Aktueller Score: {aktuellerScore}")
            Console.WriteLine($"  Verbleibende Leben: {aktuelleLeben}")

            ' Zusatz-Idee: Statusanzeige in der Pause
            Console.Write("  Status: ")
            If boostAktiv Then
                Console.ForegroundColor = ConsoleColor.Cyan : Console.WriteLine("[ SPEED BOOST! ]")
            ElseIf bierAktiv Then
                Console.ForegroundColor = ConsoleColor.Green : Console.WriteLine("[ BETRUNKEN ]")
            Else
                Console.ForegroundColor = ConsoleColor.Gray : Console.WriteLine("[ Normal ]")
            End If

            Console.ForegroundColor = ConsoleColor.White
            Console.WriteLine()
            Console.WriteLine("=====================================")
            Console.WriteLine()

            ' Optionen zeichnen
            For i As Integer = 0 To optionen.Length - 1
                Dim optText As String = optionen(i)
                If i = auswahl Then
                    optText = "   > " & optText & " <   "
                    Console.ForegroundColor = ConsoleColor.DarkYellow
                Else
                    optText = "     " & optText & "     "
                    Console.ForegroundColor = ConsoleColor.White
                End If

                Dim xPos As Integer = (37 - optText.Length) / 2 ' Zentriert im 37 Zeichen breiten Menü
                Console.SetCursorPosition(If(xPos < 0, 0, xPos), 9 + (i * 2))
                Console.Write(optText)
            Next

            taste = Console.ReadKey(True).Key
            If taste = ConsoleKey.UpArrow Or taste = ConsoleKey.LeftArrow Then
                auswahl -= 1
                If auswahl < 0 Then auswahl = optionen.Length - 1
            ElseIf taste = ConsoleKey.DownArrow Or taste = ConsoleKey.RightArrow Then
                auswahl += 1
                If auswahl >= optionen.Length Then auswahl = 0
            ElseIf taste = ConsoleKey.Enter Then
                If auswahl = 0 Then
                    ' Weiterspielen
                    Console.Clear()
                    Return False
                Else
                    ' Zurück zum Hauptmenü
                    Return True
                End If
            End If
        Loop
    End Function

    '=========================================================================================================================
    ' NFI Intro-Animation (Komplett bereinigt und kopierbar)
    '=========================================================================================================================
    Sub IntroAnimation()
        Console.Clear()
        Console.CursorVisible = False

        Dim auto() As String = {
            "   _____  ",
            "  /_..._\ ",
            " (0[###]0)",
            "  `'   `' "
        }

        Dim breite As Integer = Console.WindowWidth
        Dim startZeile As Integer = Console.WindowHeight / 2 - 2
        Dim aktuelleWartezeit As Integer = 100

        'Hinweis auf Möglichkeit zu überspringen
        Console.ForegroundColor = ConsoleColor.Red
        Console.SetCursorPosition((breite - 34) / 2, startZeile - 2)
        Console.WriteLine("BITTE VOR START BILDSCHIRM AUF MAXIMIEREN!")
        Thread.Sleep(5000) ' Kurze Pause, damit der Hinweis gelesen werden kann
        Console.SetCursorPosition((breite - 34) / 2, startZeile - 2)
        Console.WriteLine("                                      ") ' Hinweis löschen
        Console.SetCursorPosition((breite - 34) / 2, startZeile - 2)
        Console.WriteLine("[ Drücke eine beliebige Taste zum Überspringen ]")
        Thread.Sleep(500) ' Kurze Pause, damit der Hinweis gelesen werden kann
        Console.ForegroundColor = ConsoleColor.White

        Threading.Thread.Sleep(500)

        ' Das Auto fährt von links nach rechts
        For x As Integer = 0 To breite + 10

            ' Wenn IRGENDEINE Taste gedrückt wird, bricht das Intro SOFORT ab
            If Console.KeyAvailable Then
                Console.ReadKey(True) ' Tastendruck abfangen/löschen
                Console.Clear()
                Exit Sub              ' Springt direkt ins Hauptmenü
            End If

            ' Alte Position löschen
            For i As Integer = 0 To auto.Length - 1
                If x > 0 AndAlso (x - 1) < breite Then
                    Console.SetCursorPosition(x - 1, startZeile + i)
                    Console.Write(" ")
                End If
            Next

            ' Neue Position zeichnen
            Console.ForegroundColor = ConsoleColor.DarkYellow
            For i As Integer = 0 To auto.Length - 1
                If x < breite - auto(i).Length Then
                    Console.SetCursorPosition(x, startZeile + i)
                    Console.Write(auto(i))
                End If
            Next

            If x > breite * 0.3 AndAlso aktuelleWartezeit > 10 Then
                aktuelleWartezeit = CInt(aktuelleWartezeit * 0.85)
            End If

            Threading.Thread.Sleep(aktuelleWartezeit)
        Next

        ' Titel-Bildschirm (wird übersprungen, wenn man eine Taste drückt)
        Console.Clear()
        Console.ForegroundColor = ConsoleColor.White
        Console.SetCursorPosition((breite - 39) / 2, startZeile)
        Console.WriteLine("N E E D   F O R   I N F O P R O J E K T")
        Console.SetCursorPosition((breite - 43) / 2, startZeile + 3)
        Console.WriteLine("[ Drücke eine beliebige Taste zum Starten ]")

        Console.ReadKey(True)
        Console.Clear()
    End Sub

    '=========================================================================================================================
    'Sub der den Hauptablauf des Spiels steuert, von der Zeilenerzeugung über die Kollisionserkennung bis hin zum Gameover Screen
    '=========================================================================================================================

    Sub Main()

        Console.CursorVisible = False

        IntroAnimation()

        'Hauptmenü anzeigen

        Dim aktion As Hauptmenü

        Do

            aktion = Startmenü()


            If aktion = Hauptmenü.Spielen Then
                Console.Clear()
                Spielablauf()

            ElseIf aktion = Hauptmenü.Schwierigkeit Then
                SchwierigkeitAnpassen()

            ElseIf aktion = Hauptmenü.Scoreboard Then
                ScoreboardAnzeigen()

            ElseIf aktion = Hauptmenü.AutoOptik Then
                AutoOptikAnpassen()

            ElseIf aktion = Hauptmenü.Anleitung Then
                AnleitungAnzeigen()

            ElseIf aktion = Hauptmenü.Beenden Then
                Exit Do

            End If

        Loop

        Console.Clear()
        Console.WriteLine("Spiel beendet.")

    End Sub

End Module
