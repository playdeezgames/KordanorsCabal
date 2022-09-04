Public Interface IDungeonLevelData
    Inherits INameCacheData
    Function ReadName(dungeonLevelId As Long) As String
    Function ReadAll() As IEnumerable(Of Long)
End Interface
