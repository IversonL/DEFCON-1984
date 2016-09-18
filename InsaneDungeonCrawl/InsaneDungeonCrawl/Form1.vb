Public Class Form1

    Dim Grid() As PictureBox

    Dim Enemies() As PictureBox

    Dim OverWorld(99) As Integer
    Dim OverWorldMap As String
    Dim OverWorldPosition As Integer = 0

    Dim Map As String
    Dim Direction As Integer
    Dim CurrentMap As Integer

    Dim EnemyDirection As Integer
    Dim EnemyAmount As Integer = 6
    Dim EnemyArray(EnemyAmount, 6) As String
    Dim TerminalText As String
    Dim TerminalCharPos As Integer = 0
    Dim TerminalEndPos As Integer = 0

    Dim PlayerStats(3) As Integer

    Dim Bullets() As PictureBox
    Dim BulletTrack(1, 1) As Integer
    Dim BulletTestDirection As Integer
    Dim BulletsFired As Integer = -1

    Dim Mapreader As System.IO.StreamReader
    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Grid = {Tile1, Tile2, Tile3, Tile4, Tile5, Tile6, Tile7, Tile8, Tile9, Tile10, Tile11, Tile12, Tile13,
        Tile14, Tile15, Tile16, Tile17, Tile18, Tile19, Tile20, Tile21, Tile22, Tile23, Tile24, Tile25, Tile26, Tile27, Tile28, Tile29,
        Tile30, Tile31, Tile32, Tile33, Tile34, Tile35, Tile36, Tile37, Tile38, Tile39, Tile40, Tile41, Tile42, Tile43, Tile44, Tile45, Tile46, Tile47, Tile48, Tile49, Tile50,
        Tile51, Tile52, Tile53, Tile54, Tile55, Tile56, Tile57, Tile58, Tile59, Tile60, Tile61, Tile62, Tile63, Tile64, Tile65, Tile66, Tile67, Tile68, Tile69, Tile70, Tile71,
        Tile72, Tile73, Tile74, Tile75, Tile76, Tile77, Tile78, Tile79, Tile80, Tile81, Tile82, Tile83, Tile84, Tile85, Tile86, Tile87, Tile88, Tile89, Tile90, Tile91, Tile92, Tile93, Tile94,
        Tile95, Tile96, Tile97, Tile98, Tile99, Tile100}
        EnemyAmount -= 1
        PlayerStats = {30, 13, 12}
        TerminalText = "Lambda Corp. Deckard OS Version. HL3"
        TerminalTimer.Start()
        GameLoop.Start()
        Enemies = {NPC1, NPC2, NPC3, NPC4, NPC5, NPC6}
        Bullets = {Bullet1,Bullet2, Bullet3}
        EnemyCreation()
        OverWorldCreation()
        MapRead()
        PlayerStatsUpdate()
    End Sub

    Private Sub Tile1_Click(sender As Object, e As EventArgs) Handles Tile1.Click, Tile2.Click, Tile3.Click, Tile4.Click, Tile5.Click, Tile6.Click, Tile7.Click, Tile8.Click, Tile9.Click, Tile10.Click, Tile11.Click,
            Tile12.Click, Tile13.Click, Tile14.Click, Tile15.Click, Tile16.Click, Tile17.Click, Tile18.Click, Tile19.Click, Tile20.Click, Tile21.Click, Tile22.Click, Tile23.Click, Tile24.Click, Tile25.Click, Tile26.Click,
            Tile27.Click, Tile28.Click, Tile29.Click, Tile30.Click, Tile31.Click, Tile32.Click, Tile33.Click, Tile34.Click, Tile35.Click, Tile36.Click, Tile37.Click, Tile38.Click, Tile39.Click, Tile40.Click, Tile41.Click, Tile42.Click,
            Tile43.Click, Tile44.Click, Tile45.Click, Tile46.Click, Tile47.Click, Tile48.Click, Tile49.Click, Tile50.Click, Tile51.Click, Tile52.Click, Tile53.Click, Tile54.Click, Tile55.Click, Tile56.Click, Tile57.Click, Tile58.Click, Tile59.Click,
            Tile60.Click, Tile61.Click, Tile62.Click, Tile63.Click, Tile64.Click, Tile65.Click, Tile66.Click, Tile67.Click, Tile68.Click, Tile69.Click, Tile70.Click, Tile71.Click, Tile72.Click, Tile73.Click, Tile74.Click, Tile75.Click, Tile76.Click,
            Tile77.Click, Tile78.Click, Tile79.Click, Tile80.Click, Tile81.Click, Tile82.Click, Tile83.Click, Tile84.Click, Tile85.Click, Tile86.Click, Tile87.Click, Tile88.Click, Tile89.Click, Tile90.Click, Tile91.Click, Tile92.Click, Tile93.Click,
            Tile94.Click, Tile95.Click, Tile96.Click, Tile97.Click, Tile98.Click, Tile99.Click, Tile100.Click

    End Sub

    Private Sub GameLoop_Tick(sender As Object, e As EventArgs) Handles GameLoop.Tick
        EnemyAI()
        BulletMovement()
    End Sub

    Private Sub TerminalTimer_Tick(sender As Object, e As EventArgs) Handles TerminalTimer.Tick
        Dim LengthCheck As String = Me.lblTerminal.Text
        If LengthCheck.Length > 200 Then
            Me.lblTerminal.Text = Nothing
        End If
        TerminalEndPos = TerminalText.Length
        If TerminalCharPos = TerminalEndPos Then
            Exit Sub
        End If
        Me.lblTerminal.Text += TerminalText.Chars(TerminalCharPos)
        TerminalCharPos += 1
    End Sub


    Private Sub Form1_KeyDown(sender As Object, e As KeyEventArgs) Handles Me.KeyDown
        If e.KeyCode = Keys.W Then
            Me.Player.Top -= 10
            Direction = 1
        ElseIf e.KeyCode = Keys.S Then
            Me.Player.Top += 10
            Direction = 3
        ElseIf e.KeyCode = Keys.A Then
            Me.Player.Left -= 10
            Direction = 4
        ElseIf e.KeyCode = Keys.D Then
            Me.Player.Left += 10
            Direction = 2
        End If
        CollisionDetection()
    End Sub

    Sub OverWorldCreation()
        Dim Mapreader As System.IO.StreamReader
        Mapreader = My.Computer.FileSystem.OpenTextFileReader("C:\Users\Liam\Desktop\Insane Dungeon Crawl\Maps\Overworld.txt")
        OverWorldMap = Nothing
        For x As Integer = 0 To 9
            OverWorldMap += Mapreader.ReadLine
        Next
        For x As Integer = 0 To 99
            OverWorld(x) = Val(OverWorldMap.Chars(x))
        Next
        Mapreader.Close()

    End Sub

    Sub MapRead()
        CurrentMap = OverWorld(OverWorldPosition)
        Mapreader = My.Computer.FileSystem.OpenTextFileReader("C:\Users\Liam\Desktop\Insane Dungeon Crawl\Maps\Map" & CurrentMap & ".txt")
        Map = Nothing
        For x As Integer = 0 To 9
            Map += Mapreader.ReadLine
        Next
        MapAssign()
        EnemySpawn()
        Mapreader.Close()
    End Sub

    Sub MapAssign()
        For x As Integer = 0 To 99
            Grid(x).Image = Nothing
            Grid(x).Tag = 0
            If Map.Chars(x) = "1" Then
                Grid(x).Image = My.Resources.Wall_Sprite
                Grid(x).Tag = 1
            ElseIf Map.Chars(x) = "2" Then
                Grid(x).Tag = 2
            End If
        Next
    End Sub

    Sub MapChange()
        If Direction = 1 Then
            OverWorldPosition -= 10
        ElseIf Direction = 2 Then
            OverWorldPosition += 1
        ElseIf Direction = 3 Then
            OverWorldPosition += 10
        ElseIf Direction = 4 Then
            OverWorldPosition -= 1
        End If
        MapRead()

    End Sub

    Sub PlayerPositionAdjust()
        If Direction = 1 Then
            Me.Player.Top += 200
        ElseIf Direction = 2 Then
            Me.Player.Left -= 200
        ElseIf Direction = 3 Then
            Me.Player.Top -= 200
        ElseIf Direction = 4 Then
            Me.Player.Left += 200
        End If
    End Sub

    Sub CollisionDetection()
        For x As Integer = 0 To 99
            If Grid(x).Tag = 1 Then
                If Player.Bounds.IntersectsWith(Grid(x).Bounds) Then
                    Call PlayerCollision()
                End If
            ElseIf Grid(x).Tag = 2 Then
                If Player.Bounds.IntersectsWith(Grid(x).Bounds) Then
                    Call PlayerPositionAdjust
                    Call MapChange()
                End If
            End If
        Next
        For x As Integer = 0 To 3
            If Player.Bounds.IntersectsWith(Enemies(x).Bounds) Then
                Call PlayerCollision()
            End If
        Next
    End Sub

    Sub PlayerCollision()
        If Direction = 1 Then
            Me.Player.Top += 10
        ElseIf Direction = 2 Then
            Me.Player.Left -= 10
        ElseIf Direction = 3 Then
            Me.Player.Top -= 10
        ElseIf Direction = 4 Then
            Me.Player.Left += 10
        End If
    End Sub

    Sub EnemyCreation()
        Dim EnemyInfo As String
        Dim EnemyType As String
        Dim EnemyX As Integer
        Dim EnemyY As Integer
        Dim Map As Integer
        Dim Enemyreader = My.Computer.FileSystem.OpenTextFileReader("C:\Users\Liam\Desktop\Insane Dungeon Crawl\EnemyFolder\EnemyMasterList.txt")
        For x As Integer = 0 To EnemyAmount
            EnemyInfo = Enemyreader.ReadLine()
            EnemyType = EnemyInfo.Substring(0, 1)
            EnemyX = EnemyInfo.Substring(2, 3)
            EnemyY = EnemyInfo.Substring(6, 3)
            Map = EnemyInfo.Substring(10, 1)
            EnemyArray(x, 1) = EnemyType
            EnemyArray(x, 2) = EnemyX
            EnemyArray(x, 3) = EnemyY
            EnemyArray(x, 4) = Map
            EnemyArray(x, 5) = "Alive"
            EnemyArray(x, 6) = CInt(Int((10 * Rnd()) + 3))
        Next
    End Sub

    Sub EnemySpawn()
        For x As Integer = 0 To EnemyAmount
            Enemies(x).Image = Nothing
            Enemies(x).Top = 0
            Enemies(x).Left = 0
        Next
        For x As Integer = 0 To EnemyAmount
            If EnemyArray(x, 4) = CurrentMap And EnemyArray(x, 5) = "Alive" Then
                Enemies(x).BringToFront()
                Enemies(x).Left = EnemyArray(x, 2)
                Enemies(x).Top = EnemyArray(x, 3)
                Enemies(x).Image = My.Resources.Goblin
                Randomize()
                Enemies(x).Tag = x
            End If
        Next
    End Sub

    Sub EnemyAI()
        For x As Integer = 0 To EnemyAmount
            If EnemyArray(x, 4) = CurrentMap And EnemyArray(x, 5) = "Alive" Then
                EnemyPathfinding(x)
                EnemyCollisionWithPlayer()
            End If
        Next
    End Sub

    Sub EnemyPathfinding(ByVal x As Integer)
        Dim PlayerX As Integer = Player.Left
        Dim PlayerY As Integer = Player.Top
        Dim EnemyX As Integer = Enemies(x).Left
        Dim EnemyY As Integer = Enemies(x).Top
        Dim XDifference As Integer = PlayerX - EnemyX
        Dim YDifference As Integer = PlayerY - EnemyY
        If (XDifference < 0) Then
            Enemies(x).Left -= 5
            EnemyDirection = 1
        ElseIf (XDifference > 0) Then
            Enemies(x).Left += 5
            EnemyDirection = 2
        End If

        If (YDifference < 0) Then
            Enemies(x).Top -= 5
            EnemyDirection = 3
        ElseIf (YDifference > 0) Then
            Enemies(x).Top += 5
            EnemyDirection = 4
        End If

        For y As Integer = 0 To 99
            If Grid(y).Tag = 1 Or Grid(y).Tag = 2 Then
                If Enemies(x).Bounds.IntersectsWith(Grid(y).Bounds) Then
                    If EnemyDirection = 1 Then
                        Me.Enemies(x).Top -= 10
                    ElseIf EnemyDirection = 2 Then
                        Me.Enemies(x).Left += 10
                    ElseIf EnemyDirection = 3 Then
                        Me.Enemies(x).Top += 10
                    ElseIf EnemyDirection = 4 Then
                        Me.Enemies(x).Left -= 10
                    End If
                End If
            End If
        Next
    End Sub

    Sub EnemyCollisionWithPlayer()
        For x As Integer = 0 To EnemyAmount
            If Enemies(x).Bounds.IntersectsWith(Player.Bounds) Then
                If Direction = 1 Then
                    Me.Enemies(x).Top -= 10
                ElseIf Direction = 2 Then
                    Me.Enemies(x).Left += 10
                ElseIf Direction = 3 Then
                    Me.Enemies(x).Top += 10
                ElseIf Direction = 4 Then
                    Me.Enemies(x).Left -= 10
                End If
                EnemyAttack()
            End If

        Next
    End Sub

    Sub PlayerAttack(ByVal EnemyId As Integer, ByVal ClickedBox As PictureBox)
        Dim EnemyAttack As Integer = CInt(Int((13 * Rnd()) + 3))
        EnemyArray(EnemyId, 6) -= EnemyAttack
        Me.lblTerminal.Text &= vbCrLf & "Enemy hit for: " & EnemyAttack
        If (EnemyArray(EnemyId, 6) < 1) Then
            EnemyDeath(EnemyId, ClickedBox)
        End If
        PlayerAttackShot(EnemyId)
    End Sub

    Sub EnemyAttack()
        Dim Attack As Integer = CInt(Int((20 * Rnd()) + 5))
        If Attack > PlayerStats(2) Then
            Dim Damage As Integer = CInt(Int((10 * Rnd()) + 1))
            PlayerStats(0) -= Damage
            Me.lblTerminal.Text &= vbCrLf & "Player hit for: " & Damage
        End If
        PlayerStatsUpdate()

    End Sub

    Sub Terminal(ByVal Text As String)
        TerminalCharPos = 0
        TerminalText = Text
        Me.lblTerminal.Text &= vbCrLf
    End Sub

    Sub PlayerStatsUpdate()
        If (PlayerStats(0)) <= 1 Then
            System.Windows.Forms.Application.Restart()
        End If
        Me.lblPlayerStats.Text = "Hit Points: " & PlayerStats(0) & vbCrLf & "Attack: " & PlayerStats(1) & vbCrLf & "Defense: " & PlayerStats(2) & vbCrLf

    End Sub

    Private Sub NPC1_Click(sender As Object, e As EventArgs) Handles NPC1.Click, NPC2.Click, NPC3.Click, NPC4.Click, NPC5.Click
        Dim ClickedBox As PictureBox
        ClickedBox = CType(sender, PictureBox)
        For x As Integer = 0 To EnemyAmount
            If EnemyArray(x, 4) = CurrentMap And EnemyArray(x, 5) = "Alive" Then
                PlayerAttack(ClickedBox.Tag, ClickedBox)
            End If

        Next
    End Sub

    Sub EnemyDeath(ByVal EnemyID As Integer, ByVal ClickedBox As PictureBox)
        EnemyArray(EnemyID, 5) = "Dead"
        ClickedBox.SendToBack()
        ClickedBox.Top = 0
        ClickedBox.Left = 0
    End Sub

    Sub PlayerAttackShot(ByRef EnemyID As Integer)
        Dim xdifference As Integer = Player.Left - Enemies(EnemyID).Left
        Dim ydiffernce As Integer = Player.Top - Enemies(EnemyID).Top
        If xdifference < 0 Then
            BulletTestDirection = 1
        ElseIf xdifference > 0 Then
            BulletTestDirection = 2

        End If
        Bullet1.BackColor = Color.Red
    End Sub

    Sub BulletMovement()
        If BulletTestDirection = 1 Then
            Bullet1.Left += 10
        ElseIf BulletTestDirection = 2 Then
            Bullet1.Left -= 10
        End If
    End Sub

    Sub PlayerDeath()
        System.Windows.Forms.Application.Restart()
    End Sub
End Class
