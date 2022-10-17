Public Class CharacterEquipSlotData
    Inherits BaseData
    Implements ICharacterEquipSlotData
    Public Sub New(store As IStore, world As IWorldData)
        MyBase.New(store, world)
    End Sub
    Public Function Read(characterId As Long) As IEnumerable(Of Tuple(Of Long, Long))
        Return Store.Record.WithValue(Of Long, Long, Long)(
            AddressOf NoInitializer,
            CharacterEquipSlots,
            (EquipSlotIdColumn, ItemIdColumn),
            (CharacterIdColumn, characterId))
    End Function

    Public Sub Clear(characterId As Long, equipSlot As Long) Implements ICharacterEquipSlotData.Clear
        Store.Clear.ForValues(
            AddressOf NoInitializer,
            CharacterEquipSlots,
            (CharacterIdColumn, characterId),
            (EquipSlotIdColumn, equipSlot))
    End Sub

    Public Sub Write(characterId As Long, equipSlot As Long, itemId As Long) Implements ICharacterEquipSlotData.Write
        Store.Replace.Entry(
            AddressOf NoInitializer,
            CharacterEquipSlots,
            (CharacterIdColumn, characterId),
            (EquipSlotIdColumn, equipSlot),
            (ItemIdColumn, itemId))
    End Sub

    Public Sub ClearForCharacter(characterId As Long) Implements ICharacterEquipSlotData.ClearForCharacter
        Store.Clear.ForValue(
            AddressOf NoInitializer,
            CharacterEquipSlots,
            (CharacterIdColumn, characterId))
    End Sub

    Public Sub ClearForItem(itemId As Long) Implements ICharacterEquipSlotData.ClearForItem
        Store.Clear.ForValue(
            AddressOf NoInitializer,
            CharacterEquipSlots,
            (ItemIdColumn, itemId))
    End Sub

    Public Function ReadForCharacterEquipSlot(characterId As Long, equipSlotId As Long) As Long? Implements ICharacterEquipSlotData.ReadForCharacterEquipSlot
        Return Store.Column.ReadValue(Of Long, Long, Long)(
            AddressOf NoInitializer,
            CharacterEquipSlots,
            ItemIdColumn,
            (CharacterIdColumn, characterId),
            (EquipSlotIdColumn, equipSlotId))
    End Function
    Public Function ReadItemsForCharacter(characterId As Long) As IEnumerable(Of Long) Implements ICharacterEquipSlotData.ReadItemsForCharacter
        Return Store.Record.WithValues(Of Long, Long)(AddressOf NoInitializer, CharacterEquipSlots, ItemIdColumn, (CharacterIdColumn, characterId))
    End Function
    Public Function ReadEquipSlotsForCharacter(characterId As Long) As IEnumerable(Of Long) Implements ICharacterEquipSlotData.ReadEquipSlotsForCharacter
        Return Store.Record.WithValues(Of Long, Long)(AddressOf NoInitializer, CharacterEquipSlots, EquipSlotIdColumn, (CharacterIdColumn, characterId))
    End Function
End Class
