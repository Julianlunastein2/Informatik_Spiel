Imports System.Linq.Expressions
Imports System.Net.Security

Module Module1

    Const NO_KEY = 0
    Const CURSOR_LEFT = 1
    Const CURSOR_RIGHT = 2
    Const UNKNOWN_KEY = 99
    Const SPALTE_MAX = 79
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
    End Sub

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

                    If SpielfigurPos > SPALTE_MAX Then
                        SpielfigurPos = SPALTE_MAX
                    End If

                'Kollisionserkennung

                For h As Integer = 1 To 8
                    If spielfeld(ZEILE_MAX - 5, SpielfigurPos + h) = " " Or spielfeld(ZEILE_MAX - 5, SpielfigurPos + h) = "|" Or spielfeld(ZEILE_MAX - 5, SpielfigurPos + h) = Chr(0) Then
                        'Keine Kollision


                    Else

                        'Leben abziehen
                        leben = leben - 1
                        Console.Beep()

                        'Zwei Varianten zum Entfernen des Hindernisses nach Kontakt:
                        'I Bereich in der Größe eines Gegners "über" der Spielfigur löschen --> Nachteil: Gegner können "zerschnitten" werden

                        'For k As Integer = 0 To 8
                        '    For l As Integer = 0 To 3
                        '        spielfeld(ZEILE_MAX - 5 - l, SpielfigurPos + k) = " "
                        '    Next
                        'Next

                        'II Spur(en) in dem die Kollision anhand der Spielerposition erkennen und Gegner in dem Bereich des nächsten Gegners löschen

                        Dim spielfigurEnde As Integer = SpielfigurPos + 8

                        If SpielfigurPos Or spielfigurEnde > 0 And spielfigurEnde < 11 Then
                            For k As Integer = 0 To 11
                                For l As Integer = 0 To 3
                                    spielfeld(ZEILE_MAX - 5 - l, k) = "!"
                                Next
                            Next

                        End If
                    End If

                Next

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
                    Console.Write("Leben: " & leben)



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

        Spielablauf()



    End Sub

End Module
