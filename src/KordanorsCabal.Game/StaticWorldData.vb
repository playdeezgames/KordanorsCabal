Public Module StaticWorldData
    Public ReadOnly World As IWorldData = New WorldData(New Store("boilerplate.db"), New Events)
End Module
