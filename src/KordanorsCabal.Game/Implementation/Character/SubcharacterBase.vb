Public MustInherit Class SubcharacterBase
    Inherits BaseThingie
    Protected ReadOnly Property Character As ICharacter

    Protected Sub New(worldData As IWorldData, character As ICharacter)
        MyBase.New(worldData, character.Id)
        Me.Character = character
    End Sub
    Protected ReadOnly Property EquippedItems As IEnumerable(Of IItem)
        Get
            Return WorldData.CharacterEquipSlot.ReadItemsForCharacter(Id).Select(Function(x) Item.FromId(WorldData, x))
        End Get
    End Property
End Class
