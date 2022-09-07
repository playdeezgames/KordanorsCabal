Public Interface IDungeonLevelData
    Function ReadName(dungeonLevelId As Long) As String
    Function ReadAll() As IEnumerable(Of Long)
End Interface
