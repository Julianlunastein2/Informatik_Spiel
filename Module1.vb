Imports System.Linq.Expressions

Module Module1

    Const NO_KEY = 0
    Const CURSOR_LEFT = 1
    Const CURSOR_RIGHT = 2
    Const UNKNOWN_KEY = 99
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

    Sub ZeilenErzeugung(ByRef Zeile() As Char, ByVal a_max As Integer)

        'Deklarieren der Variablen
        Dim A As Integer    'Anzahl der Hindernisblocks
        Dim X As Single
        Dim i As Integer
        Dim G As Integer    'Größe des Hindernisblocks
        Dim P As Integer    'Position des Hindernisblocks


        'Zeilenvektor mit Leerzeichen füllen
        For i = 0 To 79
            Zeile(i) = " "
        Next

        'Anzahl A der Hindernisblocks zufällig ermitteln
        Randomize()
        X = VBMath.Rnd

        A = (a_max - 1) * X + 1
        'Console.WriteLine(A)

        'Für jeden der A Hindernisblocks:
        For i = 1 To A

            'Größe G des Hindernisblocks zufällig ermitteln
            Randomize()
            X = VBMath.Rnd

            G = (9 - 1) * X + 1
            'console.WriteLine("G: " & G)

            'Startposition P des Hindernisblocks zufällig ermitteln
            Randomize()
            X = VBMath.Rnd

            P = (79 - 0) * X + 0
            'Console.WriteLine("P: " & P)

            'Für jedes der G Einzelhindernisse:
            For j = 1 To G

                'Prüfen ob Hinderniss innerhalb des Wertebereichs ist
                If P + j - 1 <= 79 Then

                    'Hinderniss an Position P+j-1 in den Zeilenvektor eintragen
                    Zeile(P + j - 1) = "x"

                End If

            Next

        Next

        ''Ausgabe zum Test
        'For i = 0 To 79
        '    Console.Write(Zeile(i))
        'Next
        'Console.WriteLine()


    End Sub

    Sub Spielablauf()
        Dim leben As Integer
        Dim spielfeld(24, 79) As Char
        Dim Zeile(79) As Char
        Dim z As Integer
        Dim s As Integer
        Dim Taste As Integer
        Dim SpielfigurPos As Integer
        Dim i As Integer
        Dim Wartezeit As Integer
        Dim a_max As Single

        'Startwerte setzen
        leben = 5
        SpielfigurPos = 40
        Wartezeit = 200
        a_max = 2

        'Hauptschleife des Spiels
        Do
            'neue Zeile erzeugen
            ZeilenErzeugung(Zeile, a_max)

            'Alle Zeilen des Spielfelds um eine Zeile nach unten verschieben
            'Rückwärtschleife über zeilen
            For z = 24 To 1 Step -1
                'Vorwärtschleife über Spalten
                For s = 0 To 79
                    'Eine Zelle nach unten kopieren
                    spielfeld(z, s) = spielfeld(z - 1, s)

                Next
            Next
            'Neue Zeile am oberen Rand des Spielfelds einfügen
            For s = 0 To 79
                spielfeld(0, s) = Zeile(s)
            Next

            'Spielfeld auf der Konsole ausgeben
            Console.SetCursorPosition(0, 0)
            For z = 0 To 22
                For s = 0 To 79
                    Console.Write(spielfeld(z, s))
                Next
                Console.WriteLine()
            Next

            For i = 1 To 10

                'Tastatur abfragen
                Taste = Tastatur_Abfrage()
                'Console.WriteLine("Taste: " & Taste)

                'Spielfigur an alter Position löschen
                Console.SetCursorPosition(SpielfigurPos, 23)
                Console.Write(" ")

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

                If SpielfigurPos > 79 Then
                    SpielfigurPos = 79
                End If

                'Kollisionserkennung
                If spielfeld(22, SpielfigurPos) = "x" Then
                    leben -= 1
                    Console.Beep()

                    'Hinderniss entfernen
                    spielfeld(22, SpielfigurPos) = " "
                End If


                'Spielfigur auf der Konsole ausgeben
                Console.SetCursorPosition(SpielfigurPos, 23)
                Console.Write("O")

                'Anzeige der Leben
                Console.SetCursorPosition(0, 24)
                Console.Write("Leben: " & leben)



                'Warten
                Threading.Thread.Sleep(Wartezeit / 10)

            Next
            'Tastaturpuffer leeren
            Do
                Taste = Tastatur_Abfrage()
            Loop Until Taste = NO_KEY

            'Wartezeit verkürzen
            If Wartezeit > 50 Then
                Wartezeit = Wartezeit * 0.99
            End If
            'Console.SetCursorPosition(15, 24)
            'Console.Write("Wartezeit: " & Wartezeit)

            'Hindernissdichte erhöhen
            If a_max < 10 Then
                a_max = a_max * 1.01
            End If



        Loop Until leben <= 0

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


    Sub Main()
        Console.CursorVisible = False

        Spielablauf()



    End Sub

End Module
