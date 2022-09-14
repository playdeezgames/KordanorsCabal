Public Module StaticWorldData
    Public ReadOnly World As New WorldData(New Store("boilerplate.db"), New Checker)
End Module
