Module Module1

    Const NO_KEY = 0
    Const CURSOR_LEFT = 1
    Const CURSOR_RIGHT = 2
    Const CURSOR_UP = 3
    Const CURSOR_DOWN = 4
    Const CURSOR_ENTER = 5
    Const UNKNOWN_KEY = 99
    Const SPALTE_MAX = 79
    Const ZEILE_MAX = 24
    Const A_MAX_START = 2
    Const SPIELFIGUR = 10

    'Spieler-Farbe (anpassbar im Startmenü)
    Dim Spielerfarbe As ConsoleColor = ConsoleColor.Green

    'Abfrage der gedrückten Taste (Pfeiltasten und Enter) ohne Blockieren, wenn keine Taste gedrückt wurde
    Function Tastatur_Abfrage() As Integer
        Dim eingabe As New ConsoleKeyInfo()
        If Console.KeyAvailable = False Then
            Return NO_KEY
        Else
            eingabe = Console.ReadKey(True)
            If eingabe.Key = ConsoleKey.LeftArrow Then
                Return CURSOR_LEFT
            ElseIf eingabe.Key = ConsoleKey.RightArrow Then
                Return CURSOR_RIGHT
            ElseIf eingabe.Key = ConsoleKey.UpArrow Then
                Return CURSOR_UP
            ElseIf eingabe.Key = ConsoleKey.DownArrow Then
                Return CURSOR_DOWN
            ElseIf eingabe.Key = ConsoleKey.Enter Then
                Return CURSOR_ENTER
            Else
                Return UNKNOWN_KEY
            End If
        End If
    End Function

    Sub ZeilenErzeugung(ByRef Zeile() As Char, ByVal a_max As Integer)

        'Laufvariable; alle alten Variablen für die alte Hinderniserzeugung entfernt
        Dim i As Integer

        'Zeilenvektor mit Leerzeichen füllen
        For i = 0 To SPALTE_MAX
            Zeile(i) = " "
        Next

    End Sub


    'Anzeige des Gameover-Bildschirms mit ASCII-Art und rotem Hintergrund
    Sub Gameover()

        Console.BackgroundColor = ConsoleColor.Red
        Console.ForegroundColor = ConsoleColor.White
        Console.Clear()

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

    'Speichert den aktuellen Punktestand in einer Datei
    Sub SaveScore(ByVal punktzahl As Integer)
        'Try
        '    Dim path As String = "scores.txt"
        '    Dim scores As New System.Collections.Generic.List(Of Integer)
        '    If System.IO.File.Exists(path) Then
        '        For Each line In System.IO.File.ReadAllLines(path)
        '            Dim v As Integer
        '            If Integer.TryParse(line, v) Then scores.Add(v)
        '        Next
        '    End If
        '    scores.Add(punktzahl)
        '    scores.Sort()
        '    scores.Reverse()
        '    If scores.Count > 10 Then scores = scores.GetRange(0, 10)
        '    Dim out As New System.Collections.Generic.List(Of String)
        '    For Each v In scores
        '        out.Add(v.ToString())
        '    Next
        '    System.IO.File.WriteAllLines(path, out.ToArray())
        'Catch ex As Exception
        '    'Speicherfehler ignorieren, da es nicht kritisch ist
        'End Try
    End Sub

    'Anzeige des Scoreboards mit den Top 10 Punkteständen aus der Datei
    Sub ZeigeScoreboard()

        Dim dateipfad As String = "scores.txt"
        Dim zeilen = System.IO.File.ReadAllLines(dateipfad)
        Dim idx As Integer = 1

        Console.Clear()
        Console.WriteLine("--- Scoreboard (Top 10) ---")
        If Not System.IO.File.Exists(dateipfad) Then
            Console.WriteLine("Noch keine Scores vorhanden.")
        Else
            For Each l In zeilen
                Console.WriteLine(idx & ". " & l)
                idx += 1
                If idx > 10 Then Exit For
            Next
        End If
        Console.WriteLine()
        Console.WriteLine("Drücke eine Taste, um zurückzugehen...")
        Console.ReadKey(True)
    End Sub


    'FarbAnpassung: Ermöglicht dem Spieler, die Vordergrundfarbe seines Autos im Startmenü anzupassen
    Sub FarbenAnpassen()
        Dim col = System.Enum.GetValues(GetType(ConsoleColor))
        Dim inp = Console.ReadLine()
        Dim idx As Integer

        Console.Clear()
        Console.WriteLine("Wähle Vordergrundfarbe für dein Auto (Index eingeben):")
        For i As Integer = 0 To cols.Length - 1
            Console.WriteLine(i & ": " & cols.GetValue(i).ToString())
        Next
        Console.Write("Index: ")

        If Integer.TryParse(inp, idx) AndAlso idx >= 0 AndAlso idx < cols.Length Then
            Spielerfarbe = CType(cols.GetValue(idx), ConsoleColor)
        End If
        Console.WriteLine("Farbe gesetzt. Drücke Taste.")
        Console.ReadKey(True)
    End Sub

    Sub StartMenue()
        ' Menü mit schwebender Darstellung über laufendem Hintergrund
        ' einfache Hintergrund-Simulation: laufende Autos in Spuren
        Dim Spuren As Integer = 6
        Dim SpurSpalten(Spuren - 1) As Integer
        Dim Abstand As Integer = (SPALTE_MAX + 1) \ (Spuren + 1)
        For idxSpur As Integer = 0 To Spuren - 1
            SpurSpalten(idxSpur) = (idxSpur + 1) * Abstand
        Next

        Dim AnzahlLanes As Integer = Math.Max(1, Spuren)
        Dim LaneMitte(AnzahlLanes - 1) As Integer
        For li As Integer = 0 To AnzahlLanes - 1
            If li = 0 Then
                LaneMitte(li) = SpurSpalten(0) \ 2
            ElseIf li = AnzahlLanes - 1 Then
                LaneMitte(li) = (SpurSpalten(Spuren - 1) + SPALTE_MAX) \ 2
            Else
                LaneMitte(li) = (SpurSpalten(li - 1) + SpurSpalten(li)) \ 2
            End If
        Next

        Dim Gegner(AnzahlLanes - 1) As List(Of Integer)
        For idxLane As Integer = 0 To AnzahlLanes - 1
            Gegner(idxLane) = New List(Of Integer)()
        Next

        Dim FigurZeilen() As String = {
            "  _____  ",
            " /_..._\ ",
            "(0[###]0)",
            " `'   `' "
        }
        Dim FigurBreite As Integer = FigurZeilen(0).Length
        Dim FigurHoehe As Integer = FigurZeilen.Length

        Dim a_max As Single = A_MAX_START
        Dim SpawnWahrscheinlichkeit As Single
        Dim warteMenu As Integer = 80

        Dim options() As String = {"Spiel starten", "Scoreboard", "Auto-Farben anpassen", "Beenden"}
        Dim selected As Integer = 0

        Do
            ' Hintergrund-Spawn/Bewegung
            SpawnWahrscheinlichkeit = 0.01F * a_max
            Dim belegteLanes As Integer = 0
            For bi As Integer = 0 To AnzahlLanes - 1
                If Gegner(bi).Count > 0 Then belegteLanes += 1
            Next
            For sIdx As Integer = 0 To AnzahlLanes - 1
                If belegteLanes >= AnzahlLanes - 1 Then Exit For
                If VBMath.Rnd < SpawnWahrscheinlichkeit Then
                    Gegner(sIdx).Add(-FigurHoehe)
                    belegteLanes += 1
                End If
            Next
            For spurIndex As Integer = 0 To AnzahlLanes - 1
                For gegIndex As Integer = Gegner(spurIndex).Count - 1 To 0 Step -1
                    Gegner(spurIndex)(gegIndex) += 1
                    If Gegner(spurIndex)(gegIndex) > ZEILE_MAX Then Gegner(spurIndex).RemoveAt(gegIndex)
                Next
            Next

            ' Hintergrund zeichnen (Spuren)
            Console.SetCursorPosition(0, 0)
            For z As Integer = 0 To ZEILE_MAX - 2
                For s As Integer = 0 To SPALTE_MAX
                    Dim istSpur As Boolean = False
                    For si As Integer = 0 To Spuren - 1
                        If SpurSpalten(si) = s Then
                            istSpur = True
                            Exit For
                        End If
                    Next
                    If istSpur Then
                        If z Mod 2 = 0 Then
                            Console.Write("|")
                        Else
                            Console.Write(" ")
                        End If
                    Else
                        Console.Write(" ")
                    End If
                Next
                Console.WriteLine()
            Next

            ' Gegner im Hintergrund zeichnen
            For sIdx As Integer = 0 To AnzahlLanes - 1
                For Each gegTop In Gegner(sIdx)
                    For er As Integer = 0 To FigurHoehe - 1
                        Dim consoleRow As Integer = gegTop + er
                        If consoleRow >= 0 AndAlso consoleRow <= ZEILE_MAX - 2 Then
                            Dim colLeft As Integer = LaneMitte(sIdx) - FigurBreite \ 2
                            If colLeft < 0 Then colLeft = 0
                            If colLeft > SPALTE_MAX - (FigurBreite - 1) Then colLeft = SPALTE_MAX - (FigurBreite - 1)
                            Console.SetCursorPosition(colLeft, consoleRow)
                            Console.Write(FigurZeilen(er))
                        End If
                    Next
                Next
            Next

            ' Menü mittig zeichnen (Overlay)
            Dim menuWidth As Integer = 0
            For Each opt In options
                If opt.Length > menuWidth Then menuWidth = opt.Length
            Next
            menuWidth += 4
            Dim menuHeight As Integer = options.Length + 2
            Dim startX As Integer = (SPALTE_MAX + 1 - menuWidth) \ 2
            Dim startY As Integer = (ZEILE_MAX - menuHeight) \ 2

            Dim oldFG As ConsoleColor = Console.ForegroundColor
            Dim oldBG As ConsoleColor = Console.BackgroundColor
            Console.ForegroundColor = ConsoleColor.White
            Console.BackgroundColor = ConsoleColor.DarkBlue
            For my As Integer = 0 To menuHeight - 1
                Console.SetCursorPosition(startX, startY + my)
                Console.Write(New String(" "c, menuWidth))
            Next
            For idx As Integer = 0 To options.Length - 1
                Dim indicator As String = "  "
                If idx = selected Then indicator = "> "
                Console.SetCursorPosition(startX + 1, startY + 1 + idx)
                Console.Write(indicator & options(idx).PadRight(menuWidth - 3))
            Next
            Console.ForegroundColor = oldFG
            Console.BackgroundColor = oldBG

            ' Eingabe für Menü (non-blocking)
            Dim taste As Integer = Tastatur_Abfrage()
            If taste = CURSOR_UP Then
                selected -= 1
                If selected < 0 Then selected = options.Length - 1
            ElseIf taste = CURSOR_DOWN Then
                selected += 1
                If selected >= options.Length Then selected = 0
            ElseIf taste = CURSOR_ENTER Then
                Select Case selected
                    Case 0
                        Console.Clear()
                        Spielablauf()
                    Case 1
                        ZeigeScoreboard()
                    Case 2
                        FarbenAnpassen()
                    Case 3
                        Environment.Exit(0)
                End Select
            End If

            Threading.Thread.Sleep(warteMenu)
        Loop
    End Sub

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
        Dim Punkte As Integer = 0
        ' Spuren (Lanes) für Gegner
        Dim Spuren As Integer = 6
        Dim SpurSpalten(Spuren - 1) As Integer
        Dim Abstand As Integer = (SPALTE_MAX + 1) \ (Spuren + 1)
        For idxSpur As Integer = 0 To Spuren - 1
            SpurSpalten(idxSpur) = (idxSpur + 1) * Abstand
            If SpurSpalten(idxSpur) < 0 Then SpurSpalten(idxSpur) = 0
            If SpurSpalten(idxSpur) > SPALTE_MAX Then SpurSpalten(idxSpur) = SPALTE_MAX
        Next

        ' Lanes sind die Bereiche zwischen (und außerhalb) der gestrichelten Linien; berechne Mittelpunkte
        ' Wir erzeugen eine Lane zwischen jeder benachbarten Linie sowie links und rechts außerhalb,
        ' damit auch am Rand Gegner erscheinen können.
        Dim AnzahlLanes As Integer = Math.Max(1, Spuren)
        Dim LaneMitte(AnzahlLanes - 1) As Integer
        For li As Integer = 0 To AnzahlLanes - 1
            If li = 0 Then
                ' linke Außenlane: Mitte zwischen Spalte 0 und erster Spur
                LaneMitte(li) = SpurSpalten(0) \ 2
            ElseIf li = AnzahlLanes - 1 Then
                ' rechte Außenlane: Mitte zwischen letzter Spur und rechter Bildschirmkante
                LaneMitte(li) = (SpurSpalten(Spuren - 1) + SPALTE_MAX) \ 2
            Else
                ' mittlere Lanes: Mitte zwischen zwei benachbarten Spuren
                LaneMitte(li) = (SpurSpalten(li - 1) + SpurSpalten(li)) \ 2
            End If
        Next

        ' Gegner pro Lane: Liste von Top‑Zeilen (Integer). Top kann negativ sein (außerhalb oben)
        Dim Gegner(AnzahlLanes - 1) As List(Of Integer)
        For idxLane As Integer = 0 To AnzahlLanes - 1
            Gegner(idxLane) = New List(Of Integer)()
        Next

        ' Mehrzeilige ASCII‑Figur (Zeilen, Breite, Höhe)
        Dim FigurZeilen() As String = {
            "  _____  ",
            " /_..._\ ",
            "(0[###]0)",
            " `'   `' "
        }
        Dim FigurBreite As Integer = FigurZeilen(0).Length
        Dim FigurHoehe As Integer = FigurZeilen.Length
        Dim VorherigeLinks As Integer = -1
        Dim SpielfigurObereZeile As Integer = ZEILE_MAX - FigurHoehe ' Top-Position der Spielfigur
        Dim VorherigeObereZeile As Integer = -1

        'Startwerte setzen
        leben = 5
        SpielfigurPos = SPALTE_MAX \ 2
        SpielfigurObereZeile = ZEILE_MAX - FigurHoehe
        VorherigeObereZeile = -1
        Wartezeit = 50
        a_max = A_MAX_START

        'Hauptschleife des Spiels
        Do
            'neue Zeile erzeugen
            ZeilenErzeugung(Zeile, a_max)

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

            'Spielfeld auf der Konsole ausgeben (mit kosmetischen Spur-Markierungen)
            Console.SetCursorPosition(0, 0)
            For z = 0 To ZEILE_MAX - 2
                For s = 0 To SPALTE_MAX
                    ' Prüfe ob diese Spalte eine Spur ist
                    Dim istSpur As Boolean = False
                    For si As Integer = 0 To Spuren - 1
                        If SpurSpalten(si) = s Then
                            istSpur = True
                            Exit For
                        End If
                    Next
                    If istSpur Then
                        ' gestrichelte Markierung: jede zweite Zeile anzeigen
                        If z Mod 2 = 0 Then
                            Console.Write("|")
                        Else
                            Console.Write(" ")
                        End If
                    Else
                        Console.Write(spielfeld(z, s))
                    End If
                Next
                Console.WriteLine()
            Next

            ' Gegner spawnen und bewegen (ein Frame vor der Spieler-Subschleife)
            Randomize()
            Dim SpawnWahrscheinlichkeit As Single = 0.01F * a_max
            ' Initialisiere Spuren einmalig (falls noch nicht gesetzt)
            Static spurenInitialisiert As Boolean = False
            If Not spurenInitialisiert Then
                Dim AbstandInit As Integer = (SPALTE_MAX + 1) \ (Spuren + 1)
                For si As Integer = 0 To Spuren - 1
                    SpurSpalten(si) = (si + 1) * AbstandInit
                Next
                spurenInitialisiert = True
            End If

            ' Balance: sorge dafür, dass immer mindestens eine Lane frei bleibt
            Dim belegteLanes As Integer = 0
            For bi As Integer = 0 To AnzahlLanes - 1
                If Gegner(bi).Count > 0 Then belegteLanes += 1
            Next

            For sIdx As Integer = 0 To AnzahlLanes - 1
                ' Wenn bereits alle bis auf eine Lane belegt sind, überspringe das Spawnen
                If belegteLanes >= AnzahlLanes - 1 Then
                    Exit For
                End If
                If VBMath.Rnd < SpawnWahrscheinlichkeit Then
                    Gegner(sIdx).Add(-FigurHoehe)
                    belegteLanes += 1
                End If
            Next

            For spurIndex As Integer = 0 To AnzahlLanes - 1
                For gegIndex As Integer = Gegner(spurIndex).Count - 1 To 0 Step -1
                    Gegner(spurIndex)(gegIndex) += 1
                    If Gegner(spurIndex)(gegIndex) > ZEILE_MAX Then Gegner(spurIndex).RemoveAt(gegIndex)
                Next
            Next

            For i = 1 To SPIELFIGUR

                ' Tastatur abfragen
                Taste = Tastatur_Abfrage()

                ' Alte Spieler-Position löschen (falls vorhanden)
                If VorherigeLinks >= 0 AndAlso VorherigeObereZeile >= 0 Then
                    For r As Integer = 0 To FigurHoehe - 1
                        Dim konsoleZeileAlt As Integer = VorherigeObereZeile + r
                        If konsoleZeileAlt >= 0 Then
                            Console.SetCursorPosition(VorherigeLinks, konsoleZeileAlt)
                            Console.Write(New String(" "c, FigurBreite))
                        End If
                    Next
                End If

                ' Position ermitteln (inklusive Hoch/Runter)
                If Taste = CURSOR_LEFT Then SpielfigurPos -= 1
                If Taste = CURSOR_RIGHT Then SpielfigurPos += 1
                If Taste = CURSOR_UP Then SpielfigurObereZeile -= 1
                If Taste = CURSOR_DOWN Then SpielfigurObereZeile += 1

                ' Horizontale Begrenzung des Mittelpunkts
                If SpielfigurPos < 0 Then SpielfigurPos = 0
                If SpielfigurPos > SPALTE_MAX Then SpielfigurPos = SPALTE_MAX
                ' Vertikale Begrenzung (oben/unten im Bildschirmbereich)
                If SpielfigurObereZeile < 0 Then SpielfigurObereZeile = 0
                If SpielfigurObereZeile > ZEILE_MAX - FigurHoehe Then SpielfigurObereZeile = ZEILE_MAX - FigurHoehe

                Dim Links As Integer = SpielfigurPos - FigurBreite \ 2
                If Links < 0 Then Links = 0
                If Links > SPALTE_MAX - (FigurBreite - 1) Then Links = SPALTE_MAX - (FigurBreite - 1)

                ' (Alte Hindernis-Kollisionslogik entfernt — es gibt keine "x" Hindernisse mehr)

                ' Kollision mit Gegnern prüfen (genaue Sprite-Überlappung)
                Dim SpielerObereZeile As Integer = SpielfigurObereZeile
                For spurIndex As Integer = 0 To AnzahlLanes - 1
                    For gegIndex As Integer = Gegner(spurIndex).Count - 1 To 0 Step -1
                        Dim GegnerObereZeile As Integer = Gegner(spurIndex)(gegIndex)
                        Dim gekollidiert As Boolean = False
                        For gegReiheOffset As Integer = 0 To FigurHoehe - 1
                            Dim GegnerReihe As Integer = GegnerObereZeile + gegReiheOffset
                            Dim SpielerReiheRel As Integer = GegnerReihe - SpielerObereZeile
                            If SpielerReiheRel >= 0 AndAlso SpielerReiheRel <= FigurHoehe - 1 Then
                                For gegSpalteOffset As Integer = 0 To FigurBreite - 1
                                    Dim gegChar As Char = FigurZeilen(gegReiheOffset)(gegSpalteOffset)
                                    If gegChar = " "c Then Continue For
                                    Dim GegnerLinks As Integer = LaneMitte(spurIndex) - FigurBreite \ 2
                                    Dim AbsoluteSpalte As Integer = GegnerLinks + gegSpalteOffset
                                    Dim SpielerSpalteRel As Integer = AbsoluteSpalte - Links
                                    If SpielerSpalteRel >= 0 AndAlso SpielerSpalteRel <= FigurBreite - 1 Then
                                        Dim SpielerChar As Char = FigurZeilen(SpielerReiheRel)(SpielerSpalteRel)
                                        If SpielerChar <> " "c Then
                                            ' Kollision
                                            leben -= 1
                                            Console.Beep()
                                            Gegner(spurIndex).RemoveAt(gegIndex)
                                            gekollidiert = True
                                            Exit For
                                        End If
                                    End If
                                Next
                            End If
                            If gekollidiert Then Exit For
                        Next
                    Next
                Next

                ' Gegner zeichnen (in den Lanes zwischen den gestrichelten Linien)
                For spurIndex As Integer = 0 To AnzahlLanes - 1
                    For Each GegnerObereZeile In Gegner(spurIndex)
                        For er As Integer = 0 To FigurHoehe - 1
                            Dim consoleRow As Integer = GegnerObereZeile + er
                            If consoleRow >= 0 AndAlso consoleRow <= ZEILE_MAX - 2 Then
                                Dim SpalteLinks As Integer = LaneMitte(spurIndex) - FigurBreite \ 2
                                If SpalteLinks < 0 Then SpalteLinks = 0
                                If SpalteLinks > SPALTE_MAX - (FigurBreite - 1) Then SpalteLinks = SPALTE_MAX - (FigurBreite - 1)
                                Console.SetCursorPosition(SpalteLinks, consoleRow)
                                Console.Write(FigurZeilen(er))
                            End If
                        Next
                    Next
                Next

                'Spieler zeichnen (an aktueller Top-Position) mit einstellbarer Vordergrundfarbe
                Dim altFG As ConsoleColor = Console.ForegroundColor
                Console.ForegroundColor = Spielerfarbe
                For r As Integer = 0 To FigurHoehe - 1
                    Dim consoleRow As Integer = SpielfigurObereZeile + r
                    If consoleRow >= 0 AndAlso consoleRow <= ZEILE_MAX - 1 Then
                        Console.SetCursorPosition(Links, consoleRow)
                        Console.Write(FigurZeilen(r))
                    End If
                Next
                Console.ForegroundColor = altFG

                VorherigeLinks = Links
                VorherigeObereZeile = SpielfigurObereZeile

                'Anzeige der Leben
                Console.SetCursorPosition(0, ZEILE_MAX)
                Console.Write("Leben: " & leben & " ")

                'Warten
                Threading.Thread.Sleep(Wartezeit / SPIELFIGUR)

            Next
            'Punkte erhöhen (einfaches Scoring: Zeit/Frames überlebt)
            Punkte += 1
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

        ' Spiel beendet: Punkte speichern und Gameover anzeigen
        SaveScore(Punkte)
        Gameover()



    End Sub


    Sub Main()
        Console.CursorVisible = False
        StartMenue()



    End Sub

End Module
