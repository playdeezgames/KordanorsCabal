Public Class DungeonLevel
    Inherits BaseThingie
    ReadOnly Property Name As String
        Get
            Return StaticWorldData.World.DungeonLevel.ReadName(Id)
        End Get
    End Property
    Sub New(worldData As WorldData, dungeonLevelId As Long)
        MyBase.New(worldData, dungeonLevelId)
    End Sub
    Private Sub New(worldData As WorldData, name As String)
        Me.New(worldData, worldData.DungeonLevel.ReadForName(name).Value)
    End Sub
    Public Shared Function FromId(worldData As WorldData, dungeonLevelId As Long?) As DungeonLevel
        Return If(dungeonLevelId.HasValue, New DungeonLevel(worldData, dungeonLevelId.Value), Nothing)
    End Function
End Class

