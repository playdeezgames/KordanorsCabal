Public Class CharacterTypeSpawning
    Inherits BaseThingie
    Implements ICharacterTypeSpawning

    Public Sub New(worldData As IWorldData, id As Long)
        MyBase.New(worldData, id)
    End Sub
    Public Shared Function FromId(worldData As IWorldData, id As Long?) As ICharacterTypeSpawning
        Return If(id.HasValue, New CharacterTypeSpawning(worldData, id.Value), Nothing)
    End Function
    Function CanSpawn(locationType As ILocationType, level As IDungeonLevel) As Boolean Implements ICharacterTypeSpawning.CanSpawn
        Return WorldData.CharacterTypeSpawnLocation.Read(Id, level.Id, locationType.Id)
    End Function
    Function SpawnCount(level As IDungeonLevel) As Long Implements ICharacterTypeSpawning.SpawnCount
        Return If(WorldData.CharacterTypeSpawnCount.ReadSpawnCount(Id, level.Id), 0)
    End Function
    ReadOnly Property InitialStatistics As IReadOnlyList(Of (ICharacterStatisticType, Long)) Implements ICharacterTypeSpawning.InitialStatistics
        Get
            Dim results = WorldData.CharacterTypeInitialStatistic.ReadAllForCharacterType(Id)
            If results Is Nothing Then
                Return Nothing
            End If
            Return results.
            Select(Function(x) (CharacterStatisticType.FromId(WorldData, x.Item1), x.Item2)).ToList
        End Get
    End Property
End Class
