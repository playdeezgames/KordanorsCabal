Imports Microsoft.Xna.Framework
Imports Microsoft.Xna.Framework.Graphics
Imports Microsoft.Xna.Framework.Input

Public Class Root
    Inherits Microsoft.Xna.Framework.Game
    Private graphics As GraphicsDeviceManager
    Private spriteBatch As SpriteBatch
    Private screenTexture As Texture2D
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
    End Sub
    Protected Overrides Sub Update(gameTime As GameTime)
        If Keyboard.GetState().IsKeyDown(Keys.Escape) Then
            [Exit]()
        End If
        FrameBuffer.ClearBorder(HueColors(Hue.Cyan))
        FrameBuffer.ClearScreen(HueColors(Hue.White))
        FrameBuffer.PutCellPattern(Pattern.DownwardDiagonal, (0, 0), (HueColors(Hue.Blue), HueColors(Hue.White)))
        FrameBuffer.PutCellPattern(Pattern.DownwardDiagonal, (1, 1), (HueColors(Hue.Blue), HueColors(Hue.White)))
        FrameBuffer.PutCellPattern(Pattern.DownwardDiagonal, (2, 2), (HueColors(Hue.Blue), HueColors(Hue.White)))
        FrameBuffer.PutCellPattern(Pattern.DownwardDiagonal, (3, 3), (HueColors(Hue.Blue), HueColors(Hue.White)))
        FrameBuffer.PutCellPattern(Pattern.TopLeftCorner, (4, 4), (HueColors(Hue.Blue), HueColors(Hue.White)))
        FrameBuffer.PutCellPattern(Pattern.Horizontal1, (5, 4), (HueColors(Hue.Blue), HueColors(Hue.White)))
        FrameBuffer.PutCellPattern(Pattern.Horizontal1, (6, 4), (HueColors(Hue.Blue), HueColors(Hue.White)))
        FrameBuffer.PutCellPattern(Pattern.Horizontal1, (7, 4), (HueColors(Hue.Blue), HueColors(Hue.White)))
        FrameBuffer.PutCellPattern(Pattern.Horizontal1, (8, 4), (HueColors(Hue.Blue), HueColors(Hue.White)))
        FrameBuffer.PutCellPattern(Pattern.Horizontal1, (9, 4), (HueColors(Hue.Blue), HueColors(Hue.White)))
        FrameBuffer.PutCellPattern(Pattern.Horizontal1, (10, 4), (HueColors(Hue.Blue), HueColors(Hue.White)))
        FrameBuffer.PutCellPattern(Pattern.Horizontal1, (11, 4), (HueColors(Hue.Blue), HueColors(Hue.White)))
        FrameBuffer.PutCellPattern(Pattern.Horizontal1, (12, 4), (HueColors(Hue.Blue), HueColors(Hue.White)))
        FrameBuffer.PutCellPattern(Pattern.Horizontal1, (13, 4), (HueColors(Hue.Blue), HueColors(Hue.White)))
        FrameBuffer.PutCellPattern(Pattern.Horizontal1, (14, 4), (HueColors(Hue.Blue), HueColors(Hue.White)))
        FrameBuffer.PutCellPattern(Pattern.Horizontal1, (15, 4), (HueColors(Hue.Blue), HueColors(Hue.White)))
        FrameBuffer.PutCellPattern(Pattern.Horizontal1, (16, 4), (HueColors(Hue.Blue), HueColors(Hue.White)))
        FrameBuffer.PutCellPattern(Pattern.TopRightCorner, (17, 4), (HueColors(Hue.Blue), HueColors(Hue.White)))
        FrameBuffer.PutCellPattern(Pattern.UpwardDiagonal, (18, 3), (HueColors(Hue.Blue), HueColors(Hue.White)))
        FrameBuffer.PutCellPattern(Pattern.UpwardDiagonal, (19, 2), (HueColors(Hue.Blue), HueColors(Hue.White)))
        FrameBuffer.PutCellPattern(Pattern.UpwardDiagonal, (20, 1), (HueColors(Hue.Blue), HueColors(Hue.White)))
        FrameBuffer.PutCellPattern(Pattern.UpwardDiagonal, (21, 0), (HueColors(Hue.Blue), HueColors(Hue.White)))

        FrameBuffer.PutCellPattern(Pattern.Vertical1, (4, 5), (HueColors(Hue.Blue), HueColors(Hue.White)))
        FrameBuffer.PutCellPattern(Pattern.Vertical1, (4, 6), (HueColors(Hue.Blue), HueColors(Hue.White)))
        FrameBuffer.PutCellPattern(Pattern.Vertical1, (4, 7), (HueColors(Hue.Blue), HueColors(Hue.White)))
        FrameBuffer.PutCellPattern(Pattern.Vertical1, (4, 8), (HueColors(Hue.Blue), HueColors(Hue.White)))
        FrameBuffer.PutCellPattern(Pattern.Vertical1, (4, 9), (HueColors(Hue.Blue), HueColors(Hue.White)))
        FrameBuffer.PutCellPattern(Pattern.Vertical1, (4, 10), (HueColors(Hue.Blue), HueColors(Hue.White)))
        FrameBuffer.PutCellPattern(Pattern.Vertical1, (4, 11), (HueColors(Hue.Blue), HueColors(Hue.White)))
        FrameBuffer.PutCellPattern(Pattern.Vertical1, (4, 12), (HueColors(Hue.Blue), HueColors(Hue.White)))

        FrameBuffer.PutCellPattern(Pattern.Vertical8, (17, 5), (HueColors(Hue.Blue), HueColors(Hue.White)))
        FrameBuffer.PutCellPattern(Pattern.Vertical8, (17, 6), (HueColors(Hue.Blue), HueColors(Hue.White)))
        FrameBuffer.PutCellPattern(Pattern.Vertical8, (17, 7), (HueColors(Hue.Blue), HueColors(Hue.White)))
        FrameBuffer.PutCellPattern(Pattern.Vertical8, (17, 8), (HueColors(Hue.Blue), HueColors(Hue.White)))
        FrameBuffer.PutCellPattern(Pattern.Vertical8, (17, 9), (HueColors(Hue.Blue), HueColors(Hue.White)))
        FrameBuffer.PutCellPattern(Pattern.Vertical8, (17, 10), (HueColors(Hue.Blue), HueColors(Hue.White)))
        FrameBuffer.PutCellPattern(Pattern.Vertical8, (17, 11), (HueColors(Hue.Blue), HueColors(Hue.White)))
        FrameBuffer.PutCellPattern(Pattern.Vertical8, (17, 12), (HueColors(Hue.Blue), HueColors(Hue.White)))

        FrameBuffer.PutCellPattern(Pattern.UpwardDiagonal, (0, 17), (HueColors(Hue.Blue), HueColors(Hue.White)))
        FrameBuffer.PutCellPattern(Pattern.UpwardDiagonal, (1, 16), (HueColors(Hue.Blue), HueColors(Hue.White)))
        FrameBuffer.PutCellPattern(Pattern.UpwardDiagonal, (2, 15), (HueColors(Hue.Blue), HueColors(Hue.White)))
        FrameBuffer.PutCellPattern(Pattern.UpwardDiagonal, (3, 14), (HueColors(Hue.Blue), HueColors(Hue.White)))
        FrameBuffer.PutCellPattern(Pattern.BottomLeftCorner, (4, 13), (HueColors(Hue.Blue), HueColors(Hue.White)))
        FrameBuffer.PutCellPattern(Pattern.Horizontal8, (5, 13), (HueColors(Hue.Blue), HueColors(Hue.White)))
        FrameBuffer.PutCellPattern(Pattern.Horizontal8, (6, 13), (HueColors(Hue.Blue), HueColors(Hue.White)))
        FrameBuffer.PutCellPattern(Pattern.Horizontal8, (7, 13), (HueColors(Hue.Blue), HueColors(Hue.White)))
        FrameBuffer.PutCellPattern(Pattern.Horizontal8, (8, 13), (HueColors(Hue.Blue), HueColors(Hue.White)))
        FrameBuffer.PutCellPattern(Pattern.Horizontal8, (9, 13), (HueColors(Hue.Blue), HueColors(Hue.White)))
        FrameBuffer.PutCellPattern(Pattern.Horizontal8, (10, 13), (HueColors(Hue.Blue), HueColors(Hue.White)))
        FrameBuffer.PutCellPattern(Pattern.Horizontal8, (11, 13), (HueColors(Hue.Blue), HueColors(Hue.White)))
        FrameBuffer.PutCellPattern(Pattern.Horizontal8, (12, 13), (HueColors(Hue.Blue), HueColors(Hue.White)))
        FrameBuffer.PutCellPattern(Pattern.Horizontal8, (13, 13), (HueColors(Hue.Blue), HueColors(Hue.White)))
        FrameBuffer.PutCellPattern(Pattern.Horizontal8, (14, 13), (HueColors(Hue.Blue), HueColors(Hue.White)))
        FrameBuffer.PutCellPattern(Pattern.Horizontal8, (15, 13), (HueColors(Hue.Blue), HueColors(Hue.White)))
        FrameBuffer.PutCellPattern(Pattern.Horizontal8, (16, 13), (HueColors(Hue.Blue), HueColors(Hue.White)))
        FrameBuffer.PutCellPattern(Pattern.BottomRightCorner, (17, 13), (HueColors(Hue.Blue), HueColors(Hue.White)))
        FrameBuffer.PutCellPattern(Pattern.DownwardDiagonal, (18, 14), (HueColors(Hue.Blue), HueColors(Hue.White)))
        FrameBuffer.PutCellPattern(Pattern.DownwardDiagonal, (19, 15), (HueColors(Hue.Blue), HueColors(Hue.White)))
        FrameBuffer.PutCellPattern(Pattern.DownwardDiagonal, (20, 16), (HueColors(Hue.Blue), HueColors(Hue.White)))
        FrameBuffer.PutCellPattern(Pattern.DownwardDiagonal, (21, 17), (HueColors(Hue.Blue), HueColors(Hue.White)))

        FrameBuffer.WriteString("HELLO, WORLD! I AM SHOUTING BECAUSE REASONS!", (0, 18), (HueColors(Hue.Black), HueColors(Hue.White)))

        FrameBuffer.PutCellPattern(Pattern.Heart, (0, 20), (HueColors(Hue.Red), HueColors(Hue.White)))

        screenTexture.SetData(FrameData)
        MyBase.Update(gameTime)
    End Sub
    Protected Overrides Sub Draw(gameTime As GameTime)
        MyBase.Draw(gameTime)
        spriteBatch.Begin(samplerState:=SamplerState.PointClamp)
        spriteBatch.Draw(screenTexture, New Rectangle(0, 0, WindowWidth, WindowHeight), Color.White)
        spriteBatch.End()
    End Sub
End Class
