Public Class Form1

    Private ReadOnly TILE_SIZE As Integer = 12
    Private ReadOnly TIMER_INTERVAL As Integer = 16
    Private ReadOnly WAIT As Integer = 60

    Private ReadOnly MAP_WIDTH As Integer = 14
    Private ReadOnly MAP_HEIGHT As Integer = 25
    Private ReadOnly SCR_WIDTH As Integer = MAP_WIDTH * TILE_SIZE
    Private ReadOnly SCR_HEIGHT As Integer = MAP_HEIGHT * TILE_SIZE
    Private ReadOnly WND_WIDTH As Integer = SCR_WIDTH * 2
    Private ReadOnly WND_HEIGHT As Integer = SCR_HEIGHT * 2

    Private ReadOnly SPEED_TYOUSEI As Integer = 1       '   数値を大きくすれば早くなる。小さくすれば遅くなる。

    Private ReadOnly mBlock(,,) As Byte = {
        {
            {0, 0, 0, 0},
            {1, 1, 1, 1},
            {0, 0, 0, 0},
            {0, 0, 0, 0}
        }, {
            {0, 0, 0, 0},
            {0, 1, 1, 1},
            {0, 1, 0, 0},
            {0, 0, 0, 0}
        }, {
            {0, 0, 0, 0},
            {0, 1, 1, 0},
            {0, 1, 1, 0},
            {0, 0, 0, 0}
        }, {
            {0, 0, 0, 0},
            {1, 1, 0, 0},
            {0, 1, 1, 0},
            {0, 0, 0, 0}
        }, {
            {0, 0, 0, 0},
            {1, 1, 1, 0},
            {0, 1, 0, 0},
            {0, 0, 0, 0}
        }, {
            {0, 0, 0, 0},
            {1, 1, 1, 0},
            {0, 0, 1, 0},
            {0, 0, 0, 0}
        }, {
            {0, 0, 0, 0},
            {0, 0, 1, 1},
            {0, 1, 1, 0},
            {0, 0, 0, 0}
        }
    }

    Private mField(,) As Byte = {
        {9, 9, 9, 9, 9, 7, 7, 7, 7, 9, 9, 9, 9, 9},
        {9, 9, 9, 9, 9, 7, 7, 7, 7, 9, 9, 9, 9, 9},
        {9, 8, 8, 8, 8, 7, 7, 7, 7, 8, 8, 8, 8, 9},
        {9, 8, 7, 7, 7, 7, 7, 7, 7, 7, 7, 7, 8, 9},
        {9, 8, 7, 7, 7, 7, 7, 7, 7, 7, 7, 7, 8, 9},
        {9, 8, 7, 7, 7, 7, 7, 7, 7, 7, 7, 7, 8, 9},
        {9, 8, 7, 7, 7, 7, 7, 7, 7, 7, 7, 7, 8, 9},
        {9, 8, 7, 7, 7, 7, 7, 7, 7, 7, 7, 7, 8, 9},
        {9, 8, 7, 7, 7, 7, 7, 7, 7, 7, 7, 7, 8, 9},
        {9, 8, 7, 7, 7, 7, 7, 7, 7, 7, 7, 7, 8, 9},
        {9, 8, 7, 7, 7, 7, 7, 7, 7, 7, 7, 7, 8, 9},
        {9, 8, 7, 7, 7, 7, 7, 7, 7, 7, 7, 7, 8, 9},
        {9, 8, 7, 7, 7, 7, 7, 7, 7, 7, 7, 7, 8, 9},
        {9, 8, 7, 7, 7, 7, 7, 7, 7, 7, 7, 7, 8, 9},
        {9, 8, 7, 7, 7, 7, 7, 7, 7, 7, 7, 7, 8, 9},
        {9, 8, 7, 7, 7, 7, 7, 7, 7, 7, 7, 7, 8, 9},
        {9, 8, 7, 7, 7, 7, 7, 7, 7, 7, 7, 7, 8, 9},
        {9, 8, 7, 7, 7, 7, 7, 7, 7, 7, 7, 7, 8, 9},
        {9, 8, 7, 7, 7, 7, 7, 7, 7, 7, 7, 7, 8, 9},
        {9, 8, 7, 7, 7, 7, 7, 7, 7, 7, 7, 7, 8, 9},
        {9, 8, 7, 7, 7, 7, 7, 7, 7, 7, 7, 7, 8, 9},
        {9, 8, 7, 7, 7, 7, 7, 7, 7, 7, 7, 7, 8, 9},
        {9, 8, 7, 7, 7, 7, 7, 7, 7, 7, 7, 7, 8, 9},
        {9, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 9},
        {9, 9, 9, 9, 9, 9, 9, 9, 9, 9, 9, 9, 9, 9}
    }

    Private ReadOnly mScreen As Bitmap = New System.Drawing.Bitmap(SCR_WIDTH, SCR_HEIGHT)
    Private mTile As Bitmap()
    Private mRnd As Random = New Random
    Private mT As Byte
    Private mNext As Byte
    Private mGameOver As Boolean
    Private mTimer As Integer

    Private mX As Integer
    Private mY As Integer
    Private mA As Integer
    Private mWait As Integer
    Private mYs As Integer              '   スピード調整変数
    Private iDeleteLineCount As Integer '   消したﾗｲﾝの数

    Private mKeyL As Integer
    Private mKeyR As Integer
    Private mKeyD As Integer
    Private mKeyX As Integer
    Private mKeyZ As Integer

    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load

        Me.Width = WND_WIDTH + 100
        Me.Height = WND_HEIGHT
        Me.DoubleBuffered = True

        Dim bm As Bitmap = New Bitmap("tile.png")
        Dim len As Integer = bm.Width / TILE_SIZE

        mYs = 0
        iDeleteLineCount = 0

        ReDim Preserve mTile(len)
        For i = 0 To len - 1
            mTile(i) = bm.Clone(New Rectangle(i * TILE_SIZE, 0, TILE_SIZE, TILE_SIZE), bm.PixelFormat)
        Next

        mNext = mRnd.Next(7)

        OnNext()

        Task.Run(Sub()
                     mTimer = System.Environment.TickCount
                     While (True)
                         OnTimer()
                         mTimer += TIMER_INTERVAL
                         Task.Delay(Math.Max(1, mTimer - System.Environment.TickCount)).Wait()
                     End While
                 End Sub)

        Me.Refresh()

    End Sub

    Private Sub Form1_Paint(sender As Object, e As PaintEventArgs) Handles Me.Paint

        Dim g = Graphics.FromImage(mScreen)

        '   背景の描画
        For y = 0 To MAP_HEIGHT - 1
            For x = 0 To MAP_WIDTH - 1
                g.DrawImage(mTile(mField(y, x)), x * TILE_SIZE, y * TILE_SIZE)
            Next
        Next

        '   Game Over
        If mGameOver Then
            g.DrawString("GAME OVER", New Font("Meiryo", 16), Brushes.White, 16, 64)
        End If

        '   消したﾗｲﾝの総数
        g.DrawString(iDeleteLineCount.ToString("##0"), New Font("Meiryo", 16), Brushes.Blue, 144, 12)

        ''   スピード調査
        '        g.DrawString(CStr(mmY), New Font("Meiryo", 16), Brushes.Red, 16, 64)

        Dim sinResize As Single = Math.Min(ClientSize.Width / SCR_WIDTH, ClientSize.Height / SCR_HEIGHT)
        e.Graphics.DrawImage(mScreen, 0, 0, SCR_WIDTH * sinResize, SCR_HEIGHT * sinResize)

    End Sub

    Private Sub OnNext()

        mX = 5
        mY = 2
        mT = mNext
        mWait = WAIT

        mA = 0
        If (mKeyX > 0) Then mA = 3
        If (mKeyZ > 0) Then mA = 1

        If (OnPut(mX, mY, mT, mA, True, False) = False) Then mGameOver = True

        Call OnPut(5, -1, mNext, 0, False, False)
        mNext = mRnd.Next(7)
        Call OnPut(5, -1, mNext, 0, True, False)

    End Sub

    ''' <summary>
    ''' 移動できるかどうか、置けるかどうかを検証する
    ''' </summary>
    ''' <param name="x">X座標</param>
    ''' <param name="y">Y座標</param>
    ''' <param name="t">ミノの形状（番号）</param>
    ''' <param name="a">角度</param>
    ''' <param name="s">設置するかどうか</param>
    ''' <param name="test"></param>
    ''' <returns></returns>
    Private Function OnPut(x As Integer, y As Integer, t As Byte, a As Integer, s As Boolean, test As Boolean) As Boolean

        For j = 0 To 3
            For i = 0 To 3
                Dim p() As Integer = {i, 3 - j, 3 - i, j}   '   回転行列
                Dim q() As Integer = {j, i, 3 - j, 3 - i}
                If (mBlock(t, q(a), p(a)) = 0) Then Continue For

                Dim v As Byte = t

                If (s = False) Then
                    v = 7
                ElseIf (mField(y + j, x + i) <> 7) Then
                    Return False
                End If

                If (test = False) Then mField(y + j, x + i) = v
            Next
        Next

        Return True

    End Function

    Private Sub OnTimer()

        Call OnTick()

        If (mKeyL > 0) Then mKeyL += 1
        If (mKeyR > 0) Then mKeyR += 1
        If (mKeyX > 0) Then mKeyX += 1
        If (mKeyZ > 0) Then mKeyZ += 1
        If (mKeyD > 0) Then mKeyD += 1

        Invalidate()    '   再描画

    End Sub

    Private Sub OnTick()

        Dim a As Integer
        Dim x As Integer

        If mGameOver Then Return

        If (mWait <= WAIT / 2) Then
            OnWait()
            Return
        End If

        Call OnPut(mX, mY, mT, mA, False, False)

        a = mA
        If (mKeyX = 1) Then a -= 1
        If (mKeyZ = 1) Then a += 1
        a = a Mod 4
        If a < 0 Then a = 4 + a
        If OnPut(mX, mY, mT, a, True, True) Then mA = a

        x = mX
        If (mKeyL = 1 Or mKeyL > 20) Then x -= 1
        If (mKeyR = 1 Or mKeyR > 20) Then x += 1
        If OnPut(x, mY, mT, mA, True, True) Then mX = x

        mYs += mKeyD * 2
        mKeyD = 0

        If OnPut(mX, mY + 1, mT, mA, True, True) Then
            '   速度調整のため、修正      ---- Start ----
            mYs += SPEED_TYOUSEI
            mWait = WAIT
            If mYs >= 10 Then
                mY += mYs \ 10
                mYs = mYs Mod 10
            End If
            '            mY += 1
            '            mWait = WAIT
            '   速度調整のため、修正      ----  End  ----
        Else
            mWait -= 1
        End If
        Call OnPut(mX, mY, mT, mA, True, False)

    End Sub

    Private Sub OnWait()

        Dim n As Integer

        mWait -= 1
        If (mWait = 0) Then OnNext()

        If (mWait = WAIT / 2 - 1) Then
            For y = 22 To 3 Step -1
                n = 0
                For x = 2 To 11
                    If (mField(y, x)) < 7 Then
                        n += 1
                    End If
                Next
                If (n <> 10) Then Continue For
                For x = 2 To 11
                    mField(y, x) = 10
                Next
                iDeleteLineCount += 1       '   消した数のｶｳﾝﾄ
            Next
        End If

        If (mWait = 1) Then
            For y = 22 To 3 Step -1
                If (mField(y, 2) <> 10) Then Continue For
                mWait = WAIT / 2 - 2
                For i = y To 4 Step -1
                    For x = 2 To 11
                        mField(i, x) = mField(i - 1, x)
                    Next
                Next
                For x = 2 To 11
                    mField(3, x) = 7
                Next
                y += 1
            Next
        End If

    End Sub

    Private Sub Form1_KeyDown(sender As Object, e As KeyEventArgs) Handles MyBase.KeyDown

        If e.KeyCode = Keys.Left Then mKeyL += 1
        If e.KeyCode = Keys.Right Then mKeyR += 1
        If e.KeyCode = Keys.X Then mKeyX += 1
        If e.KeyCode = Keys.Z Then mKeyZ += 1
        If e.KeyCode = Keys.Down Then mKeyD += 1

    End Sub

    Private Sub Form1_KeyUp(sender As Object, e As KeyEventArgs) Handles MyBase.KeyUp

        If e.KeyCode = Keys.Left Then mKeyL = 0
        If e.KeyCode = Keys.Right Then mKeyR = 0
        If e.KeyCode = Keys.X Then mKeyX = 0
        If e.KeyCode = Keys.Z Then mKeyZ = 0
        If e.KeyCode = Keys.Down Then mKeyD = 0

    End Sub

End Class
