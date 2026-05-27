Imports System.Linq.Expressions
Imports System.Net.Security

Module Module1

    Const NO_KEY = 0
    Const CURSOR_LEFT = 1
    Const CURSOR_RIGHT = 2
    Const UNKNOWN_KEY = 99
    Const SPALTE_MAX = 60
    Const ZEILE_MAX = 24
    Const A_MIN = 1
    Const A_MAX_START = 2
    Const G_MIN = 1
    Const G_Max = 9
    Const P_MIN = 0
    Const P_MAX = 79
    Const SPIELFIGUR = 10

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
            Else
                Return UNKNOWN_KEY
            End If
        End If
    End Function


    '=========================================================================================================================
    'Hindernis und Spielfeld -Generierung
    '=========================================================================================================================


    Sub ZeilenErzeugung(ByRef Zeile() As Char, ByVal a_max As Integer, ByVal idx As Integer, ByVal auto_schicht As Integer, ByRef autoInSpur() As Boolean, ByRef spawnCooldown As Integer)

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

        'Anzahl A der Hindernisblocks zufällig ermitteln
        Randomize()
        X = VBMath.Rnd

        'A = (a_max - A_MIN) * X + A_MIN
        'Console.WriteLine(A)

        'Für jeden der A Hindernisblocks:
        For i = 0 To 4

            'Spuren Begrenzung einfügen, alle 3 zeilen kommt kein strich

            P = 0 + (12 * i)

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

                P = 1 + (12 * i)

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
            If X < 0.1 Then '10% Chance auf Item

                P = 6 + (12 * i)

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

    Sub Gameover()
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

        Console.WriteLine("Drücke Enter, um zum Hauptmenü zurückzukehren...")
        Console.ReadLine()
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
        Beenden
    End Enum

    Function Startmenü() As Hauptmenü

        Dim auswahl As Integer = 0
        Dim optionen() As String = {"SPIEL STARTEN", "SCHWIERIGKEIT", "SCOREBOARD", "AUTO OPTIK", "SPIEL BEENDEN"}

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
                    ' Rückgabewerte passend zu deinem Enum mappen
                    Select Case auswahl
                        Case 0 : Return Hauptmenü.Spielen
                        Case 1 : Return Hauptmenü.Schwierigkeit
                        Case 2 : Return Hauptmenü.Scoreboard
                        Case 3 : Return Hauptmenü.AutoOptik
                        Case 4 : Return Hauptmenü.Beenden
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
        Dim Wartezeit As Integer
        Dim a_max As Single
        Dim idx = 0  'Zähler für die Spurenbegrenzung
        Dim auto_schicht As Integer = 3 'Zähler für die Autoabildung
        Dim autoInSpur(4) As Boolean 'Variable um zu entscheiden ob ein Auto in der Spur ist oder nicht, damit es nicht in jeder Zeile ein Auto gibt
        Dim spawnCooldown As Integer = 0 'Cooldown um zu verhindern dass in jeder Zeile ein Auto spawnt, eleminiert langweilige Optik von einem "Block" Gegner
        Dim unverwundbar As DateTime = DateTime.MinValue 'Variable um die Dauer der Unverwundbarkeit zu speichern
        Dim item As Integer


        'Startwerte setzen
        leben = 5
        SpielfigurPos = SPALTE_MAX / 2
        Wartezeit = 200
        a_max = A_MAX_START

        'Hauptschleife des Spiels
        Do
            'neue Zeile erzeugen
            ZeilenErzeugung(Zeile, a_max, idx, auto_schicht, autoInSpur, spawnCooldown)

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

            'Spielfeld auf der Konsole ausgeben
            Console.SetCursorPosition(0, 0)
            For z = 0 To ZEILE_MAX - 2
                For s = 0 To SPALTE_MAX
                    Console.Write(spielfeld(z, s))
                Next
                Console.WriteLine()
            Next

            For i = 1 To SPIELFIGUR

                'Tastatur abfragen
                Taste = Tastatur_Abfrage()
                'Console.WriteLine("Taste: " & Taste)

                'Spielfigur an alter Position löschen
                For h As Integer = 1 To 4
                    Console.SetCursorPosition(SpielfigurPos, ZEILE_MAX - h)
                    Console.Write("         ")
                Next

                'Position der Spielfigur ermitteln
                If Taste = CURSOR_LEFT Then
                    SpielfigurPos -= 1
                End If

                If Taste = CURSOR_RIGHT Then
                    SpielfigurPos += 1
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

                For f As Integer = 1 To 4
                    For h As Integer = 1 To 8
                        Dim symbol As Char = spielfeld(ZEILE_MAX - f, SpielfigurPos + h)

                        If symbol = " " Or symbol = "|" Or symbol = Chr(0) Then
                            'Keine Kollision, weitergehen

                        ElseIf symbol = "+" Then
                            leben = leben + 1
                            spielfeld(ZEILE_MAX - f, SpielfigurPos + h) = " " ' Item auf dem Feld löschen

                        ElseIf symbol = "B" Then
                            'Bier -> Item 3 (Kontrollen vertauscht)
                            spielfeld(ZEILE_MAX - f, SpielfigurPos + h) = " " ' Item auf dem Feld löschen

                        ElseIf symbol = "*" Then
                            'Stern -> Unverwundbarkeit (Item 1)
                            unverwundbar = DateTime.Now.AddSeconds(10)
                            spielfeld(ZEILE_MAX - f, SpielfigurPos + h) = " " ' Item auf dem Feld löschen

                        ElseIf symbol = "S" Then
                            'Speed Boost -> Item 2
                            spielfeld(ZEILE_MAX - f, SpielfigurPos + h) = " " ' Item auf dem Feld löschen

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

                    ' prüfen, welche der 5 Spuren (0 bis 4) die Spielfigur gerade berührt
                    For i_spur As Integer = 0 To 4
                        Dim spurStart As Integer = 1 + (12 * i_spur)
                        Dim spurEnde As Integer = spurStart + 10 ' Ein Auto ist ca. 8-9 Zeichen breit

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
                        Console.Beep()
                    End If
                End If

                'Spielfigur auf der Konsole ausgeben
                Console.SetCursorPosition(SpielfigurPos + 1, ZEILE_MAX - 1)
            Console.Write("`'   `'")
            Console.SetCursorPosition(SpielfigurPos + 0, ZEILE_MAX - 2)
            Console.Write("(0[###]0)")
            Console.SetCursorPosition(SpielfigurPos + 1, ZEILE_MAX - 3)
            Console.Write("/_..._\")
            Console.SetCursorPosition(SpielfigurPos + 2, ZEILE_MAX - 4)
            Console.Write("_____")

            'Anzeige der Leben
            Console.SetCursorPosition(0, ZEILE_MAX)
                    If DateTime.Now < unverwundbar Then
                        Console.Write("Leben: " & leben & " Unverwundbar")
                        Console.ForegroundColor = ConsoleColor.Yellow
                    Else
                        Console.Write("Leben: " & leben & "                  ")
                        Console.ForegroundColor = ConsoleColor.White
                    End If

                    'Warten
                    Threading.Thread.Sleep(Wartezeit / SPIELFIGUR)

                Next
                'Tastaturpuffer leeren
                Do
                Taste = Tastatur_Abfrage()
            Loop Until Taste = NO_KEY

            'Wartezeit verkürzen
            If Wartezeit > 50 Then
                Wartezeit = Wartezeit * 0.99
            End If
            'Console.SetCursorPosition(15, ZEILE_MAX)
            'Console.Write("Wartezeit: " & Wartezeit)

            'Hindernissdichte erhöhen
            If a_max < 10 Then
                a_max = a_max * 1.01
            End If



        Loop Until leben <= 0

        Gameover()



    End Sub


    '=========================================================================================================================
    'Sub der den Hauptablauf des Spiels steuert, von der Zeilenerzeugung über die Kollisionserkennung bis hin zum Gameover Screen
    '=========================================================================================================================

    Sub Main()

        Console.CursorVisible = False

        'Hauptmenü anzeigen

        Dim aktion As Hauptmenü

        Do

            aktion = Startmenü()

            If aktion = Hauptmenü.Spielen Then

                Console.Clear()
                Spielablauf()

            ElseIf aktion = Hauptmenü.Beenden Then

                Exit Do

            End If

        Loop

        Console.Clear()
        Console.WriteLine("Spiel beendet.")

    End Sub

End Module
