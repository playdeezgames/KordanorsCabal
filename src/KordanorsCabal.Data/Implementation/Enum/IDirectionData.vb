Public Interface IDirectionData
    Inherits INameCacheData
    Function ReadName(direction As Long) As String
    Function ReadPrevious(direction As Long) As Long?
    Function ReadOpposite(direction As Long) As Long?
    Function ReadNext(direction As Long) As Long?
    Function ReadIsCardinal(direction As Long) As Boolean
    Function ReadAll() As IEnumerable(Of Long)
    Function ReadAbbreviation(direction As Long) As String
End Interface
