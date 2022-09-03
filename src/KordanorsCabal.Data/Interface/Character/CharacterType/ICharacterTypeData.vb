Public Interface ICharacterTypeData
    Function ReadIsUndead(characterTypeId As Long) As Long?
    Function ReadXPValue(characterTypeId As Long) As Long?
    Function ReadMoneyDropDice(characterTypeId As Long) As String
    Function ReadName(characterTypeId As Long) As String
    Function ReadAll() As IEnumerable(Of Long)
End Interface
