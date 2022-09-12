Public Class CharacterTypeSpawning
    Inherits BaseThingie
    Implements ICharacterTypeSpawning

    Public Sub New(worldData As IWorldData, id As Long)
        MyBase.New(worldData, id)
    End Sub
    Public Shared Function FromId(worldData As IWorldData, id As Long) As ICharacterTypeSpawning
        Return New CharacterTypeSpawning(worldData, id)
    End Function
    Function CanSpawn(locationType As ILocationType, level As IDungeonLevel) As Boolean Implements ICharacterTypeSpawning.CanSpawn
        Return WorldData.CharacterTypeSpawnLocation.Read(Id, level.Id, locationType.Id)
    End Function
End Class
