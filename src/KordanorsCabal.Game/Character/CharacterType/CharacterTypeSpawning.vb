Friend Class CharacterTypeSpawning
    Inherits BaseThingie
    Implements ICharacterTypeSpawning

    Public Sub New(worldData As IWorldData, id As Long)
        MyBase.New(worldData, id)
    End Sub
    Shared Function FromId(worldData As IWorldData, id As Long) As ICharacterTypeSpawning
        Return New CharacterTypeSpawning(worldData, id)
    End Function
End Class
