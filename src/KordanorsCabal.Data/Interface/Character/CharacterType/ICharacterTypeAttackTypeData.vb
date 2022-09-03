Public Interface ICharacterTypeAttackTypeData
    Function Read(characterTypeId As Long) As IReadOnlyDictionary(Of Long, Integer)
End Interface
