Public Interface ICharacterTypeData
    Function ReadAll() As IEnumerable(Of Long)
    Function ReadIsUndead(characterTypeId As Long) As Long?
    Function ReadMoneyDropDice(characterTypeId As Long) As String
    Function ReadName(characterTypeId As Long) As String
    Function ReadXPValue(characterTypeId As Long) As Long?
End Interface
