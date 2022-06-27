Imports KordanorsCabal.Game
Imports Microsoft.Xna.Framework
Imports Microsoft.Xna.Framework.Audio
Imports Microsoft.Xna.Framework.Graphics
Imports Microsoft.Xna.Framework.Input
Imports Microsoft.Xna.Framework.Media

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
    Private ScreenSize As Integer = 4
    Private minorTheme As Song
    Private ReadOnly Property WindowWidth As Integer
        Get
            Return ViewWidth * ScreenSize * 2
        End Get
    End Property
    Private ReadOnly Property WindowHeight As Integer
        Get
            Return ViewHeight * ScreenSize
        End Get
    End Property
    Private Function GetScreenSize() As Integer
        Return ScreenSize
    End Function
    Private Sub SetScreenSize(size As Integer)
        ScreenSize = size
        UpdateWindowSize()
        Dim config = UIConfig.Load
        config.ScreenSize = size
        UIConfig.Save(config)
    End Sub

    Private Sub UpdateWindowSize()
        graphics.PreferredBackBufferWidth = WindowWidth
        graphics.PreferredBackBufferHeight = WindowHeight
        graphics.ApplyChanges()
    End Sub

    Sub New()
        graphics = New GraphicsDeviceManager(Me)
    End Sub
    Protected Overrides Sub Initialize()
        MyBase.Initialize()
        Window.Title = "Kordanor's Cabal"
        'TODO: load in config for saved off screen size and master volume
        Dim config = UIConfig.Load()
        ScreenSize = config.ScreenSize
        UpdateWindowSize()
        MediaPlayer.Volume = config.MuxVolume
        Content.RootDirectory = "Content"
        ScreenSizerProcessor.GetCurrentScreenSize = AddressOf GetScreenSize
        ScreenSizerProcessor.SetCurrentScreenSize = AddressOf SetScreenSize
        MuxVolumizerProcessor.GetCurrentMuxVolume = AddressOf GetMuxVolume
        MuxVolumizerProcessor.SetCurrentMuxVolume = AddressOf SetMuxVolume
        SfxVolumizerProcessor.GetCurrentSfxVolume = AddressOf GetSfxVolume
        SfxVolumizerProcessor.SetCurrentSfxVolume = AddressOf SetSfxVolume
        MediaPlayer.IsRepeating = True
        MediaPlayer.Play(minorTheme)
        AddHandler Game.SfxPlayer.PlaySfx, AddressOf HandleSfx
    End Sub

    Private Sub HandleSfx(sfx As Sfx)
        If soundEffects.ContainsKey(sfx) Then
            Dim volume = GetSfxVolume()
            soundEffects(sfx).Play(volume, 0.0, 0.0)
        End If
    End Sub

    Private Sub SetSfxVolume(volume As Single)
        Dim config = UIConfig.Load
        config.SfxVolume = volume
        UIConfig.Save(config)
        Game.SfxPlayer.Play(Sfx.CharacterCreation)
    End Sub

    Private Function GetSfxVolume() As Single
        Return UIConfig.Load.SfxVolume
    End Function

    Private Sub SetMuxVolume(volume As Single)
        MediaPlayer.Volume = volume
        Dim config = UIConfig.Load
        config.MuxVolume = volume
        UIConfig.Save(config)
    End Sub

    Private Function GetMuxVolume() As Single
        Return MediaPlayer.Volume
    End Function

    Private Shared soundEffectFiles As IReadOnlyDictionary(Of Sfx, String) =
        New Dictionary(Of Sfx, String) From
        {
            {Sfx.CharacterCreation, "Content/RollDice.wav"}
        }

    Private soundEffects As IReadOnlyDictionary(Of Sfx, SoundEffect)

    Protected Overrides Sub LoadContent()
        spriteBatch = New SpriteBatch(GraphicsDevice)
        screenTexture = New Texture2D(GraphicsDevice, ViewWidth, ViewHeight)
        renderer = New Renderer(Of Color)((BorderWidth, BorderHeight), (CellColumns, CellRows), (CellWidth, CellHeight), HueColors)
        minorTheme = Song.FromUri("MinorTheme", New Uri("Content/MinorTheme.ogg", UriKind.Relative))
        'minorTheme = Song.FromUri("Music", New Uri("Content/Music.ogg", UriKind.Relative))
        soundEffects = soundEffectFiles.ToDictionary(Function(x) x.Key, Function(x) SoundEffect.FromFile(x.Value))
    End Sub
    Protected Overrides Sub Update(gameTime As GameTime)
        ProcessInput()
        If uiState = UIState.None Then
            [Exit]()
        End If
        UpdateOutput()
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
