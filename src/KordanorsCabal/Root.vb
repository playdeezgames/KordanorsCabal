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
        FrameBuffer.ClearBorder(Color.Cyan)
        FrameBuffer.ClearScreen(WhiteHue)
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
