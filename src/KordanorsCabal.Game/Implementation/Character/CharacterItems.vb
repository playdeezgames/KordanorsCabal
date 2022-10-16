Public Class CharacterItems
    Inherits SubcharacterBase
    Implements ICharacterItems

    Public Sub New(worldData As IWorldData, character As ICharacter)
        MyBase.New(worldData, character)
    End Sub

    Shared Function FromCharacter(worldData As IWorldData, character As ICharacter) As ICharacterItems
        Return If(character IsNot Nothing, New CharacterItems(worldData, character), Nothing)
    End Function
    Sub PurifyItems() Implements ICharacterItems.PurifyItems
        For Each item In Inventory.Items
            item.Purify()
        Next
        For Each item In EquippedItems
            item.Purify()
        Next
    End Sub
    ReadOnly Property CanBeBribedWith(itemType As IItemType) As Boolean Implements ICharacterItems.CanBeBribedWith
        Get
            Return character.CharacterType.Combat.CanBeBribedWith(itemType)
        End Get
    End Property
    ReadOnly Property Inventory As IInventory Implements ICharacterItems.Inventory
        Get
            Dim inventoryId As Long? = WorldData.Inventory.ReadForCharacter(Id)
            If Not inventoryId.HasValue Then
                inventoryId = WorldData.Inventory.CreateForCharacter(Id)
            End If
            Return Game.Inventory.FromId(WorldData, inventoryId.Value)
        End Get
    End Property
    Function HasItemType(itemType As IItemType) As Boolean Implements ICharacterItems.HasItemType
        Return Inventory.ItemsOfType(itemType).Any
    End Function
    Public Sub UseItem(item As IItem) Implements ICharacterItems.UseItem
        If item.Usage.CanUse(character) Then
            item.Usage.Use(character)
            If item.Usage.IsConsumed Then
                item.Destroy()
            End If
        End If
    End Sub
End Class
