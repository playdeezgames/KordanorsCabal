Public Class DungeonLevel
    Public ReadOnly Id As Long
    Public Sub New(dungeonLevelId As Long)
        Id = dungeonLevelId
    End Sub
    Public ReadOnly Property Name As String
        Get
            Return StaticWorldData.World.DungeonLevel.ReadName(Id)
        End Get
    End Property
    Public Shared Function FromId(dungeonLevelId As Long) As DungeonLevel
        Return New DungeonLevel(dungeonLevelId)
    End Function
    Public Shared ReadOnly Property All As IEnumerable(Of DungeonLevel)
        Get
            Return StaticWorldData.World.DungeonLevel.All.Select(AddressOf FromId)
        End Get
    End Property
End Class
