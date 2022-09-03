Public Interface ICharacterSpellData
    Function Read(characterId As Long, spellType As Long) As Long?
    Sub Write(characterId As Long, spellType As Long, level As Long)
    Function ReadForCharacter(characterId As Long) As IEnumerable(Of Tuple(Of Long, Long))
    Sub ClearForCharacter(characterId As Long)
End Interface
