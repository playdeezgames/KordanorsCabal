Public Interface IDungeonLevelData
    Function ReadAll() As IEnumerable(Of Long)
    Function ReadName(dungeonLevelId As Long) As String
End Interface
