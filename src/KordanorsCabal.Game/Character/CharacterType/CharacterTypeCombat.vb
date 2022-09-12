Public Class CharacterTypeCombat
    Inherits BaseThingie
    Implements ICharacterTypeCombat

    Public Sub New(worldData As IWorldData, id As Long)
        MyBase.New(worldData, id)
    End Sub

    Shared Function FromId(worldData As IWorldData, id As Long) As ICharacterTypeCombat
        Return New CharacterTypeCombat(worldData, id)
    End Function
    Function CanBeBribedWith(itemType As OldItemType) As Boolean Implements ICharacterTypeCombat.CanBeBribedWith
        Return WorldData.CharacterTypeBribe.Read(Id, itemType)
    End Function
End Class
