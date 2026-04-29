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

    Sub ZeilenErzeugung(ByRef Zeile() As Char)

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

        'Anzahl A der HIndernisblocks zufällig ermitteln
        X = VBMath.Rnd

        A = (5 - 1) * X + 1
        'Console.WriteLine(A)

        'Für jeden der A Hindernisblocks:
        For i = 1 To A

            'Größe G des Hindernisblocks zufällig ermitteln
            X = VBMath.Rnd

            G = (9 - 1) * X + 1
            'console.WriteLine("G: " & G)

            'Startposition P des Hindernisblocks zufällig ermitteln
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

        'Startwerte setzen
        leben = 5
        SpielfigurPos = 40

        'Hauptschleife des Spiels
        Do
            'neue Zeile erzeugen
            ZeilenErzeugung(Zeile)

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

                'Spielfigur auf der Konsole ausgeben
                Console.SetCursorPosition(SpielfigurPos, 23)
                Console.Write("O")

                'Warten
                Threading.Thread.Sleep(200 / 10)
            Next
            'Tastaturpuffer leeren
            Do
                Taste = Tastatur_Abfrage()
            Loop Until Taste = NO_KEY



        Loop Until leben = 0


    End Sub


    Sub Main()
        Console.CursorVisible = False

        Spielablauf()



    End Sub

End Module
