Imports Microsoft.Xna.Framework
Imports Microsoft.Xna.Framework.Graphics
Imports Microsoft.Xna.Framework.Input

Public Class Root
    Inherits Microsoft.Xna.Framework.Game
    Private graphics As GraphicsDeviceManager
    Private spriteBatch As SpriteBatch
    Private screenTexture As Texture2D
    Private renderer As Renderer(Of Color)
    Private pressedCommands As New HashSet(Of Command)
    Private uiState As UIState = UIState.TitleScreen
    Const BorderWidth = 16
    Const BorderHeight = 28
    Const CellWidth = 8
    Const CellHeight = 8
    Const CellColumns = 22
    Const CellRows = 23
    Const ViewWidth = CellWidth * CellColumns + BorderWidth * 2
    Const ViewHeight = CellHeight * CellRows + BorderHeight * 2
    Const WindowWidth = ViewWidth * 8
    Const WindowHeight = ViewHeight * 4
    Sub New()
        graphics = New GraphicsDeviceManager(Me)
    End Sub
    Protected Overrides Sub Initialize()
        MyBase.Initialize()
        Window.Title = "Kordanor's Cabal"
        graphics.PreferredBackBufferWidth = WindowWidth
        graphics.PreferredBackBufferHeight = WindowHeight
        graphics.ApplyChanges()
        Content.RootDirectory = "Content"
    End Sub
    Protected Overrides Sub LoadContent()
        spriteBatch = New SpriteBatch(GraphicsDevice)
        screenTexture = New Texture2D(GraphicsDevice, ViewWidth, ViewHeight)
        renderer = New Renderer(Of Color)((BorderWidth, BorderHeight), (CellColumns, CellRows), (CellWidth, CellHeight), HueColors)

    End Sub
    Protected Overrides Sub Update(gameTime As GameTime)
        ProcessInput()
        If uiState = UIState.None Then
            [Exit]()
        End If
        UpdateOutput()
        'renderer.PatternBuffer.Fill(Pattern.Space, False, Hue.Blue)
        'renderer.PatternBuffer.Cell(0, 0).Pattern = Pattern.DownwardDiagonal
        'renderer.PatternBuffer.Cell(1, 1).Pattern = Pattern.DownwardDiagonal
        'renderer.PatternBuffer.Cell(2, 2).Pattern = Pattern.DownwardDiagonal
        'renderer.PatternBuffer.Cell(3, 3).Pattern = Pattern.DownwardDiagonal
        'renderer.PatternBuffer.Cell(4, 4).Pattern = Pattern.TopLeftCorner
        'renderer.PatternBuffer.FillCells((5, 4), (12, 1), Pattern.Horizontal1, False, Hue.Blue)
        'renderer.PatternBuffer.Cell(17, 4).Pattern = Pattern.TopRightCorner
        'renderer.PatternBuffer.Cell(18, 3).Pattern = Pattern.UpwardDiagonal
        'renderer.PatternBuffer.Cell(19, 2).Pattern = Pattern.UpwardDiagonal
        'renderer.PatternBuffer.Cell(20, 1).Pattern = Pattern.UpwardDiagonal
        'renderer.PatternBuffer.Cell(21, 0).Pattern = Pattern.UpwardDiagonal

        'renderer.PatternBuffer.FillCells((4, 5), (1, 8), Pattern.Vertical1, False, Hue.Blue)

        'renderer.PatternBuffer.FillCells((17, 5), (1, 8), Pattern.Vertical8, False, Hue.Blue)

        'renderer.PatternBuffer.Cell(0, 17).Pattern = Pattern.UpwardDiagonal
        'renderer.PatternBuffer.Cell(1, 16).Pattern = Pattern.UpwardDiagonal
        'renderer.PatternBuffer.Cell(2, 15).Pattern = Pattern.UpwardDiagonal
        'renderer.PatternBuffer.Cell(3, 14).Pattern = Pattern.UpwardDiagonal
        'renderer.PatternBuffer.Cell(4, 13).Pattern = Pattern.BottomLeftCorner
        'renderer.PatternBuffer.FillCells((5, 13), (12, 1), Pattern.Horizontal8, False, Hue.Blue)
        'renderer.PatternBuffer.Cell(17, 13).Pattern = Pattern.BottomRightCorner
        'renderer.PatternBuffer.Cell(18, 14).Pattern = Pattern.DownwardDiagonal
        'renderer.PatternBuffer.Cell(19, 15).Pattern = Pattern.DownwardDiagonal
        'renderer.PatternBuffer.Cell(20, 16).Pattern = Pattern.DownwardDiagonal
        'renderer.PatternBuffer.Cell(21, 17).Pattern = Pattern.DownwardDiagonal

        'renderer.PatternBuffer.WriteText((0, 18), "HELLO, WORLD! I AM SHOUTING BECAUSE REASONS!", False, Hue.Black)
        'renderer.PatternBuffer.PutCell((0, 20), Pattern.Heart, False, Hue.Red)

        renderer.Update()
        renderer.FrameBuffer.UpdateTexture(screenTexture)
        MyBase.Update(gameTime)
    End Sub

    Private Sub UpdateOutput()
        MainProcessor.UpdateBuffer(uiState, renderer.PatternBuffer)
    End Sub

    Private Sub ProcessInput()
        Dim newPressedCommands = New HashSet(Of Command)(Keyboard.GetState().GetPressedKeys().Where(AddressOf KeyCommands.ContainsKey).Select(Function(x) KeyCommands(x)))
        For Each command In newPressedCommands.Where(Function(x) Not pressedCommands.Contains(x))
            uiState = MainProcessor.ProcessCommand(uiState, command)
        Next
        pressedCommands = newPressedCommands
    End Sub

    Protected Overrides Sub Draw(gameTime As GameTime)
        MyBase.Draw(gameTime)
        spriteBatch.Begin(samplerState:=SamplerState.PointClamp)
        spriteBatch.Draw(screenTexture, New Rectangle(0, 0, WindowWidth, WindowHeight), Color.White)
        spriteBatch.End()
    End Sub
End Class
