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
    Sub DropLoot(location As ILocation) Implements ICharacterTypeCombat.DropLoot
        Dim lootTable = WorldData.CharacterTypeLoot.Read(Id)
        If lootTable Is Nothing OrElse Not lootTable.Any Then
            Return
        End If
        Dim itemType = CType(RNG.FromGenerator(lootTable), OldItemType)
        If itemType <> OldItemType.None Then
            location.Inventory.Add(Item.Create(WorldData, itemType))
        End If
    End Sub
End Class
