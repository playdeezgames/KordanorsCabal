Public Interface ISpellTypeData
    Function ReadName(spellTypeId As Long) As String
    Function ReadMaximumLevel(spellTypeId As Long) As Long?
    Function ReadCastCheck(spellTypeId As Long) As String
End Interface
