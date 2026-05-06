Imports System.Linq.Expressions

Module Module1

    Const NO_KEY = 0
    Const CURSOR_LEFT = 1
    Const CURSOR_RIGHT = 2
    Const UNKNOWN_KEY = 99

    Const Spalte_Max = 79
    Const Zeile_Max = 24
    Const A_Min = 1
    Const A_Max_Start = 2
    Const G_MIN = 1
    Const G_MAX = 9
    Const P_Min = 0
    Const P_Max = Spalte_Max

    Const Bewegung_Spielfigur = 10
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
        For i = 0 To Spalte_Max
            Zeile(i) = " "
        Next

        'Anzahl A der HIndernisblocks zufällig ermitteln
        Randomize()
        X = VBMath.Rnd

        A = (5 - 1) * X + 1
        'Console.WriteLine(A)

        'Für jeden der A Hindernisblocks:
        For i = 1 To A

            'Größe G des Hindernisblocks zufällig ermitteln
            Randomize()
            X = VBMath.Rnd

            G = (G_MAX - 1) * X + 1
            'console.WriteLine("G: " & G)

            'Startposition P des Hindernisblocks zufällig ermitteln
            Randomize()
            X = VBMath.Rnd

            P = (Spalte_Max - 0) * X + 0
            'Console.WriteLine("P: " & P)

            'Für jedes der G Einzelhindernisse:
            For j = 1 To G

                'Prüfen ob Hinderniss innerhalb des Wertebereichs ist
                If P + j - 1 <= Spalte_Max Then

                    'Hinderniss an Position P+j-1 in den Zeilenvektor eintragen
                    Zeile(P + j - 1) = "x"

                End If

            Next

        Next

        ''Ausgabe zum Test
        'For i = 0 To Spalte_Max
        '    Console.Write(Zeile(i))
        'Next
        'Console.WriteLine()


    End Sub

    Sub Spielablauf()
        Dim leben As Integer
        Dim spielfeld(Zeile_Max, Spalte_Max) As Char
        Dim Zeile(Spalte_Max) As Char
        Dim z As Integer
        Dim s As Integer
        Dim Taste As Integer
        Dim SpielfigurPos As Integer
        Dim i As Integer
        Dim wartezeit As Single
        Dim spielfigur_spalte As Integer
        Dim a_max As Single

        'Startwerte setzen:
        leben = 12
        SpielfigurPos = Spalte_Max / 2
        wartezeit = 200
        a_max = A_Max_Start

        'Hauptschleife des Spiels
        Do
            'neue Zeile erzeugen
            ZeilenErzeugung(Zeile, a_max)

            'Alle Zeilen des Spielfelds um eine Zeile nach unten verschieben
            'Rückwärtschleife über zeilen
            For z = Zeile_Max To 1 Step -1
                'Vorwärtschleife über Spalten
                For s = 0 To Spalte_Max
                    'Eine Zelle nach unten kopieren
                    spielfeld(z, s) = spielfeld(z - 1, s)

                Next
            Next
            'Neue Zeile am oberen Rand des Spielfelds einfügen
            For s = 0 To Spalte_Max
                spielfeld(0, s) = Zeile(s)
            Next

            'Spielfeld auf der Konsole ausgeben
            Console.SetCursorPosition(0, 0)
            For z = 0 To Zeile_Max - 2
                For s = 0 To Spalte_Max
                    Console.Write(spielfeld(z, s))
                Next
                Console.WriteLine()
            Next

            For i = 1 To 10

                'Tastatur abfragen
                Taste = Tastatur_Abfrage()
                'Console.WriteLine("Taste: " & Taste)

                'Spielfigur an alter Position löschen
                Console.SetCursorPosition(SpielfigurPos, Zeile_Max - 1)
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

                If SpielfigurPos > Spalte_Max Then
                    SpielfigurPos = Spalte_Max
                End If



                'Kollisionserkennung

                'Spielfigur auf der Konsole ausgeben
                Console.SetCursorPosition(SpielfigurPos, Zeile_Max - 1)
                Console.Write("O")

                'Kollisionsprüfung:
                If spielfeld(22, spielfigur_spalte) = "X" Then
                    'Kollision erkannt:
                    leben = leben - 1
                    Console.Beep()

                    'Hindernis löschen:
                    spielfeld(22, spielfigur_spalte) = " "
                End If

                'Anzeige der Leben-Anzahl:
                Console.SetCursorPosition(0, Zeile_Max)
                Console.Write("Leben: " & leben)




                'Warten
                Threading.Thread.Sleep(wartezeit / 10)
            Next
            'Tastaturpuffer leeren
            Do
                Taste = Tastatur_Abfrage()
            Loop Until Taste = NO_KEY

            'Wartezeit verringern:
            wartezeit = wartezeit * 0.99
            If wartezeit < 0 Then wartezeit = 0
            'Console.SetCursorPosition(15, Zeile_Max)
            'Console.Write(wartezeit)

            'Hindernisdichte erhöhen: 
            a_max = a_max * 1.03
            'Console.SetCursorPosition(25, 14)
            'Console.Write(a_max)

        Loop Until leben <= 0

        Console.BackgroundColor = ConsoleColor.Blue
        Console.ForegroundColor = ConsoleColor.White
        Console.Clear()

        Console.SetCursorPosition(0, 10)

        Console.WriteLine("  ________                        ________                     
 /  _____/_____    _____   ____   \_____  \___  __ ___________ 
/   \  ___\__  \  /     \_/ __ \   /   |   \  \/ // __ \_  __ \
\    \_\  \/ __ \|  Y Y  \  ___/  /    |    \   /\  ___/|  | \/
 \______  (____  /__|_|  /\___  > \_______  /\_/  \___  >__|   
        \/     \/      \/     \/          \/          \/       ")
        Console.ReadLine()


    End Sub


    Sub Main()
        Console.CursorVisible = False

        Spielablauf()



    End Sub

End Module
