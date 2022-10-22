Public Module StaticWorldData
    Public ReadOnly WorldData As Data.IWorldData = New Data.WorldData(New SPLORR.Data.Store("boilerplate.db"), New Game.Events)
End Module
