Public Class CharacterRepair
    Inherits SubcharacterBase
    Implements ICharacterRepair

    Public Sub New(worldData As IWorldData, character As ICharacter)
        MyBase.New(worldData, character)
    End Sub

    Shared Function FromCharacter(worldData As IWorldData, character As ICharacter) As ICharacterRepair
        Return If(character IsNot Nothing, New CharacterRepair(worldData, character), Nothing)
    End Function
    Public ReadOnly Property ItemsToRepair(shoppeType As IShoppeType) As IEnumerable(Of IItem) Implements ICharacterRepair.ItemsToRepair
        Get
            Dim items As New List(Of IItem)
            items.AddRange(character.Items.Inventory.Items.Where(Function(x) x.Repair.IsNeeded))
            items.AddRange(EquippedItems.Where(Function(x) x.Repair.IsNeeded))
            Return items.Where(Function(x) shoppeType.WillRepair(x.ItemType))
        End Get
    End Property
    Public Function HasItemsToRepair(shoppeType As IShoppeType) As Boolean Implements ICharacterRepair.HasItemsToRepair
        Return ItemsToRepair(shoppeType).Any
    End Function
End Class
