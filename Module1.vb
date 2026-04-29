Module Module1
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
                    Zeile(P + j - 1) = "X"

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

        'Startwerte setzen
        leben = 5


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
            For z = 0 To 24
                For s = 0 To 79
                    Console.Write(spielfeld(z, s))
                Next
                Console.WriteLine()
            Next

            'Warten
            Threading.Thread.Sleep(1000)
        Loop Until leben = 0


    End Sub

    Sub Main()

        Spielablauf()

    End Sub

End Module
