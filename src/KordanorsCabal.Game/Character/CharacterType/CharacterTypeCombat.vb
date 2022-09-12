Public Class CharacterTypeCombat
    Inherits BaseThingie
    Implements ICharacterTypeCombat

    Public Sub New(worldData As IWorldData, id As Long)
        MyBase.New(worldData, id)
    End Sub

    Friend Shared Function FromId(worldData As IWorldData, id As Long) As ICharacterTypeCombat
        Return New CharacterTypeCombat(worldData, id)
    End Function
End Class
