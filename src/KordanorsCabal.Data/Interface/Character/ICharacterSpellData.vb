Public Interface ICharacterSpellData
    Sub ClearForCharacter(characterId As Long)
    Function Read(characterId As Long, spellType As Long) As Long?
    Function ReadForCharacter(characterId As Long) As IEnumerable(Of Tuple(Of Long, Long))
    Sub Write(characterId As Long, spellType As Long, level As Long)
End Interface
