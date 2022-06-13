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
        FrameBuffer.ClearBorder(CyanHue)
        FrameBuffer.ClearScreen(WhiteHue)
        FrameBuffer.PutCellPattern(PETSCII(&H4D), (0, 0), (BlueHue, WhiteHue))
        FrameBuffer.PutCellPattern(PETSCII(&H4D), (1, 1), (BlueHue, WhiteHue))
        FrameBuffer.PutCellPattern(PETSCII(&H4D), (2, 2), (BlueHue, WhiteHue))
        FrameBuffer.PutCellPattern(PETSCII(&H4D), (3, 3), (BlueHue, WhiteHue))
        FrameBuffer.PutCellPattern(PETSCII(&H4F), (4, 4), (BlueHue, WhiteHue))
        FrameBuffer.PutCellPattern(PETSCII(&H63), (5, 4), (BlueHue, WhiteHue))
        FrameBuffer.PutCellPattern(PETSCII(&H63), (6, 4), (BlueHue, WhiteHue))
        FrameBuffer.PutCellPattern(PETSCII(&H63), (7, 4), (BlueHue, WhiteHue))
        FrameBuffer.PutCellPattern(PETSCII(&H63), (8, 4), (BlueHue, WhiteHue))
        FrameBuffer.PutCellPattern(PETSCII(&H63), (9, 4), (BlueHue, WhiteHue))
        FrameBuffer.PutCellPattern(PETSCII(&H63), (10, 4), (BlueHue, WhiteHue))
        FrameBuffer.PutCellPattern(PETSCII(&H63), (11, 4), (BlueHue, WhiteHue))
        FrameBuffer.PutCellPattern(PETSCII(&H63), (12, 4), (BlueHue, WhiteHue))
        FrameBuffer.PutCellPattern(PETSCII(&H63), (13, 4), (BlueHue, WhiteHue))
        FrameBuffer.PutCellPattern(PETSCII(&H63), (14, 4), (BlueHue, WhiteHue))
        FrameBuffer.PutCellPattern(PETSCII(&H63), (15, 4), (BlueHue, WhiteHue))
        FrameBuffer.PutCellPattern(PETSCII(&H63), (16, 4), (BlueHue, WhiteHue))
        FrameBuffer.PutCellPattern(PETSCII(&H50), (17, 4), (BlueHue, WhiteHue))
        FrameBuffer.PutCellPattern(PETSCII(&H4E), (18, 3), (BlueHue, WhiteHue))
        FrameBuffer.PutCellPattern(PETSCII(&H4E), (19, 2), (BlueHue, WhiteHue))
        FrameBuffer.PutCellPattern(PETSCII(&H4E), (20, 1), (BlueHue, WhiteHue))
        FrameBuffer.PutCellPattern(PETSCII(&H4E), (21, 0), (BlueHue, WhiteHue))

        FrameBuffer.PutCellPattern(PETSCII(&H65), (4, 5), (BlueHue, WhiteHue))
        FrameBuffer.PutCellPattern(PETSCII(&H65), (4, 6), (BlueHue, WhiteHue))
        FrameBuffer.PutCellPattern(PETSCII(&H65), (4, 7), (BlueHue, WhiteHue))
        FrameBuffer.PutCellPattern(PETSCII(&H65), (4, 8), (BlueHue, WhiteHue))
        FrameBuffer.PutCellPattern(PETSCII(&H65), (4, 9), (BlueHue, WhiteHue))
        FrameBuffer.PutCellPattern(PETSCII(&H65), (4, 10), (BlueHue, WhiteHue))
        FrameBuffer.PutCellPattern(PETSCII(&H65), (4, 11), (BlueHue, WhiteHue))
        FrameBuffer.PutCellPattern(PETSCII(&H65), (4, 12), (BlueHue, WhiteHue))

        FrameBuffer.PutCellPattern(PETSCII(&H67), (17, 5), (BlueHue, WhiteHue))
        FrameBuffer.PutCellPattern(PETSCII(&H67), (17, 6), (BlueHue, WhiteHue))
        FrameBuffer.PutCellPattern(PETSCII(&H67), (17, 7), (BlueHue, WhiteHue))
        FrameBuffer.PutCellPattern(PETSCII(&H67), (17, 8), (BlueHue, WhiteHue))
        FrameBuffer.PutCellPattern(PETSCII(&H67), (17, 9), (BlueHue, WhiteHue))
        FrameBuffer.PutCellPattern(PETSCII(&H67), (17, 10), (BlueHue, WhiteHue))
        FrameBuffer.PutCellPattern(PETSCII(&H67), (17, 11), (BlueHue, WhiteHue))
        FrameBuffer.PutCellPattern(PETSCII(&H67), (17, 12), (BlueHue, WhiteHue))

        FrameBuffer.PutCellPattern(PETSCII(&H4E), (0, 17), (BlueHue, WhiteHue))
        FrameBuffer.PutCellPattern(PETSCII(&H4E), (1, 16), (BlueHue, WhiteHue))
        FrameBuffer.PutCellPattern(PETSCII(&H4E), (2, 15), (BlueHue, WhiteHue))
        FrameBuffer.PutCellPattern(PETSCII(&H4E), (3, 14), (BlueHue, WhiteHue))
        FrameBuffer.PutCellPattern(PETSCII(&H4C), (4, 13), (BlueHue, WhiteHue))
        FrameBuffer.PutCellPattern(PETSCII(&H64), (5, 13), (BlueHue, WhiteHue))
        FrameBuffer.PutCellPattern(PETSCII(&H64), (6, 13), (BlueHue, WhiteHue))
        FrameBuffer.PutCellPattern(PETSCII(&H64), (7, 13), (BlueHue, WhiteHue))
        FrameBuffer.PutCellPattern(PETSCII(&H64), (8, 13), (BlueHue, WhiteHue))
        FrameBuffer.PutCellPattern(PETSCII(&H64), (9, 13), (BlueHue, WhiteHue))
        FrameBuffer.PutCellPattern(PETSCII(&H64), (10, 13), (BlueHue, WhiteHue))
        FrameBuffer.PutCellPattern(PETSCII(&H64), (11, 13), (BlueHue, WhiteHue))
        FrameBuffer.PutCellPattern(PETSCII(&H64), (12, 13), (BlueHue, WhiteHue))
        FrameBuffer.PutCellPattern(PETSCII(&H64), (13, 13), (BlueHue, WhiteHue))
        FrameBuffer.PutCellPattern(PETSCII(&H64), (14, 13), (BlueHue, WhiteHue))
        FrameBuffer.PutCellPattern(PETSCII(&H64), (15, 13), (BlueHue, WhiteHue))
        FrameBuffer.PutCellPattern(PETSCII(&H64), (16, 13), (BlueHue, WhiteHue))
        FrameBuffer.PutCellPattern(PETSCII(&H7A), (17, 13), (BlueHue, WhiteHue))
        FrameBuffer.PutCellPattern(PETSCII(&H4D), (18, 14), (BlueHue, WhiteHue))
        FrameBuffer.PutCellPattern(PETSCII(&H4D), (19, 15), (BlueHue, WhiteHue))
        FrameBuffer.PutCellPattern(PETSCII(&H4D), (20, 16), (BlueHue, WhiteHue))
        FrameBuffer.PutCellPattern(PETSCII(&H4D), (21, 17), (BlueHue, WhiteHue))

        FrameBuffer.WriteString("HELLO, WORLD! I AM SHOUTING BECAUSE REASONS!", (0, 18), (BlackHue, WhiteHue))

        FrameBuffer.PutCellPattern(PETSCII(&H53), (0, 20), (RedHue, WhiteHue))

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
