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
    Private Sub New(name As String)
        Me.New(StaticWorldData.World.DungeonLevel.ReadForName(name).Value)
    End Sub
    Public Shared Function FromId(dungeonLevelId As Long?) As DungeonLevel
        Return If(dungeonLevelId.HasValue, New DungeonLevel(dungeonLevelId.Value), Nothing)
    End Function

    Friend Shared Function FromName(name As String) As DungeonLevel
        Return New DungeonLevel(name)
    End Function
End Class

