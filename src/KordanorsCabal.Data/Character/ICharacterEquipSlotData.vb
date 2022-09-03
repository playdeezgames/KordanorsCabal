Public Interface ICharacterEquipSlotData
    Sub ClearForItem(itemId As Long)
    Sub ClearForCharacter(characterId As Long)
    Sub Write(characterId As Long, equipSlotId As Long, itemId As Long)
    Function ReadForCharacterEquipSlot(characterId As Long, equipSlotId As Long) As Long?
    Function ReadItemsForCharacter(characterId As Long) As IEnumerable(Of Long)
    Function ReadEquipSlotsForCharacter(characterId As Long) As IEnumerable(Of Long)
    Sub Clear(characterId As Long, equipSlotId As Long)
End Interface
