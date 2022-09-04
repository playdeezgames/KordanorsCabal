Public Interface ICharacterTypeLootData
    Function Read(characterTypeId As Long) As IReadOnlyDictionary(Of Long, Integer)
End Interface
