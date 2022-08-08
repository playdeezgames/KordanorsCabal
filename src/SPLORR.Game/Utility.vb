Public Module Utility
    Public Function MakeHashSet(Of T)(ParamArray items() As T) As HashSet(Of T)
        Return New HashSet(Of T)(items)
    End Function
    Public Function MakeDictionary(Of T, U)(ParamArray entries() As (T, U)) As IReadOnlyDictionary(Of T, U)
        Return entries.ToDictionary(Of T, U)(
            Function(x) x.Item1, Function(x) x.Item2)
    End Function
End Module
