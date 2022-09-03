Public Class DungeonLevel
    Inherits BaseThingie
    ReadOnly Property Name As String
        Get
            Return WorldData.DungeonLevel.ReadName(Id)
        End Get
    End Property
    Sub New(worldData As IWorldData, dungeonLevelId As Long)
        MyBase.New(worldData, dungeonLevelId)
    End Sub
    Private Sub New(worldData As IWorldData, name As String)
        Me.New(worldData, worldData.DungeonLevel.ReadForName(name).Value)
    End Sub
    Public Shared Function FromId(worldData As IWorldData, dungeonLevelId As Long?) As DungeonLevel
        Return If(dungeonLevelId.HasValue, New DungeonLevel(worldData, dungeonLevelId.Value), Nothing)
    End Function
End Class

