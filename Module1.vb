Module Module1
    Sub ZeilenErzeugung()

        'Deklarieren der Variablen
        Dim A As Integer    'Anzahl der Hindernisblocks
        Dim X As Single
        Dim i As Integer
        Dim G As Integer    'Größe des Hindernisblocks
        Dim P As Integer    'Position des Hindernisblocks
        Dim Zeile(79) As Char   'Zeilenvektor mit 80 Zeichen (0-79)

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

        'Ausgabe zum Test
        For i = 0 To 79
            Console.Write(Zeile(i))
        Next
        Console.WriteLine()


    End Sub


    Sub Main()
        Dim i As Integer

        For i = 1 To 20
            ZeilenErzeugung()
            Threading.Thread.Sleep(100)
        Next

    End Sub

End Module
