Public Interface ICharacterEquipSlotData
    Sub Clear(characterId As Long, equipSlotId As Long)
    Sub ClearForCharacter(characterId As Long)
    Sub ClearForItem(itemId As Long)
    Function ReadEquipSlotsForCharacter(characterId As Long) As IEnumerable(Of Long)
    Function ReadForCharacterEquipSlot(characterId As Long, equipSlotId As Long) As Long?

    Sub Write(characterId As Long, equipSlotId As Long, itemId As Long)
    Function ReadItemsForCharacter(characterId As Long) As IEnumerable(Of Long)
End Interface
