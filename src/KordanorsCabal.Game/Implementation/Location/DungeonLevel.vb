Public Class DungeonLevel
    Inherits BaseThingie
    Implements IDungeonLevel
    ReadOnly Property Name As String Implements IDungeonLevel.Name
        Get
            Return WorldData.DungeonLevel.ReadName(Id)
        End Get
    End Property
    Sub New(worldData As IWorldData, dungeonLevelId As Long)
        MyBase.New(worldData, dungeonLevelId)
    End Sub
    Public Shared Function FromId(worldData As IWorldData, dungeonLevelId As Long?) As IDungeonLevel
        Return If(dungeonLevelId.HasValue, New DungeonLevel(worldData, dungeonLevelId.Value), Nothing)
    End Function
End Class

