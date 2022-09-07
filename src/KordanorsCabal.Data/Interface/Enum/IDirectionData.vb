Public Interface IDirectionData
    Inherits INameCacheData
    Function ReadAbbreviation(direction As Long) As String
    Function ReadAll() As IEnumerable(Of Long)
    Function ReadIsCardinal(direction As Long) As Boolean
    Function ReadName(direction As Long) As String
    Function ReadNext(direction As Long) As Long?
    Function ReadOpposite(direction As Long) As Long?
    Function ReadPrevious(direction As Long) As Long?
End Interface
