Public Class ItemTypeSpawn
    Inherits BaseThingie
    Implements IItemTypeSpawn
    Public Sub New(worldData As IWorldData, id As Long)
        MyBase.New(worldData, id)
    End Sub
    Public Shared Function FromId(worldData As IWorldData, id As Long?) As IItemTypeSpawn
        Return If(id.HasValue, New ItemTypeSpawn(worldData, id.Value), Nothing)
    End Function
    ReadOnly Property SpawnLocationTypes(dungeonLevel As IDungeonLevel) As HashSet(Of ILocationType) Implements IItemTypeSpawn.SpawnLocationTypes
        Get
            Dim results = WorldData.ItemTypeSpawnLocationType.ReadAll(Id, dungeonLevel.Id)
            If results Is Nothing Then
                Return New HashSet(Of ILocationType)
            End If
            Return New HashSet(Of ILocationType)(results.Select(Function(x) LocationType.FromId(WorldData, x)))
        End Get
    End Property
    Private ReadOnly Property SpawnCounts(dungeonLevel As IDungeonLevel) As String
        Get
            Return WorldData.ItemTypeSpawnCount.Read(Id, dungeonLevel.Id)
        End Get
    End Property
    Function RollSpawnCount(dungeonLevel As IDungeonLevel) As Long Implements IItemTypeSpawn.RollSpawnCount
        Return RNG.RollDice(SpawnCounts(dungeonLevel))
    End Function
End Class
