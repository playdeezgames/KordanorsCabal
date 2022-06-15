Imports Microsoft.Xna.Framework
Imports Microsoft.Xna.Framework.Graphics
Imports Microsoft.Xna.Framework.Input

Public Class Root
    Inherits Microsoft.Xna.Framework.Game
    Private graphics As GraphicsDeviceManager
    Private spriteBatch As SpriteBatch
    Private screenTexture As Texture2D
    Private Renderer As Renderer
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
        IsMouseVisible = True
    End Sub
    Protected Overrides Sub LoadContent()
        spriteBatch = New SpriteBatch(GraphicsDevice)
        screenTexture = New Texture2D(GraphicsDevice, ViewWidth, ViewHeight)
        Renderer = New Renderer((16, 28), (22, 23), (8, 8))
    End Sub
    Protected Overrides Sub Update(gameTime As GameTime)
        If Keyboard.GetState().IsKeyDown(Keys.Escape) Then
            [Exit]()
        End If
        Renderer.PatternBuffer.Fill(Pattern.Space, False, Hue.Blue)
        Renderer.PatternBuffer.Cell(0, 0).Pattern = Pattern.DownwardDiagonal
        Renderer.PatternBuffer.Cell(1, 1).Pattern = Pattern.DownwardDiagonal
        Renderer.PatternBuffer.Cell(2, 2).Pattern = Pattern.DownwardDiagonal
        Renderer.PatternBuffer.Cell(3, 3).Pattern = Pattern.DownwardDiagonal
        Renderer.PatternBuffer.Cell(4, 4).Pattern = Pattern.TopLeftCorner
        Renderer.PatternBuffer.FillCells((5, 4), (12, 1), Pattern.Horizontal1, False, Hue.Blue)
        Renderer.PatternBuffer.Cell(17, 4).Pattern = Pattern.TopRightCorner
        Renderer.PatternBuffer.Cell(18, 3).Pattern = Pattern.UpwardDiagonal
        Renderer.PatternBuffer.Cell(19, 2).Pattern = Pattern.UpwardDiagonal
        Renderer.PatternBuffer.Cell(20, 1).Pattern = Pattern.UpwardDiagonal
        Renderer.PatternBuffer.Cell(21, 0).Pattern = Pattern.UpwardDiagonal

        Renderer.PatternBuffer.FillCells((4, 5), (1, 8), Pattern.Vertical1, False, Hue.Blue)

        Renderer.PatternBuffer.FillCells((17, 5), (1, 8), Pattern.Vertical8, False, Hue.Blue)

        Renderer.PatternBuffer.Cell(0, 17).Pattern = Pattern.UpwardDiagonal
        Renderer.PatternBuffer.Cell(1, 16).Pattern = Pattern.UpwardDiagonal
        Renderer.PatternBuffer.Cell(2, 15).Pattern = Pattern.UpwardDiagonal
        Renderer.PatternBuffer.Cell(3, 14).Pattern = Pattern.UpwardDiagonal
        Renderer.PatternBuffer.Cell(4, 13).Pattern = Pattern.BottomLeftCorner
        Renderer.PatternBuffer.FillCells((5, 13), (12, 1), Pattern.Horizontal8, False, Hue.Blue)
        Renderer.PatternBuffer.Cell(17, 13).Pattern = Pattern.BottomRightCorner
        Renderer.PatternBuffer.Cell(18, 14).Pattern = Pattern.DownwardDiagonal
        Renderer.PatternBuffer.Cell(19, 15).Pattern = Pattern.DownwardDiagonal
        Renderer.PatternBuffer.Cell(20, 16).Pattern = Pattern.DownwardDiagonal
        Renderer.PatternBuffer.Cell(21, 17).Pattern = Pattern.DownwardDiagonal

        Renderer.PatternBuffer.WriteText((0, 18), "HELLO, WORLD! I AM SHOUTING BECAUSE REASONS!", False, Hue.Black)
        Renderer.PatternBuffer.PutCell((0, 20), Pattern.Heart, False, Hue.Red)

        Renderer.Update()
        Renderer.FrameBuffer.UpdateTexture(screenTexture)
        MyBase.Update(gameTime)
    End Sub
    Protected Overrides Sub Draw(gameTime As GameTime)
        MyBase.Draw(gameTime)
        spriteBatch.Begin(samplerState:=SamplerState.PointClamp)
        spriteBatch.Draw(screenTexture, New Rectangle(0, 0, WindowWidth, WindowHeight), Color.White)
        spriteBatch.End()
    End Sub
End Class
