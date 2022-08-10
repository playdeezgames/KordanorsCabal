Public Enum OldDungeonLevel
    None
    Level1
    Level2
    Level3
    Level4
    Level5
    Moon
End Enum
Public Class DungeonLevel
    ReadOnly Property Id As Long
    ReadOnly Property Name As String
        Get
            Return StaticWorldData.World.DungeonLevel.ReadName(Id)
        End Get
    End Property
    Sub New(dungeonLevelId As Long)
        Id = dungeonLevelId
    End Sub
End Class

