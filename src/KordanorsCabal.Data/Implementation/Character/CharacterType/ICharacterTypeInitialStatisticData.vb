Public Interface ICharacterTypeInitialStatisticData
    Function ReadAllForCharacterType(id As Long) As List(Of Tuple(Of Long, Long))
End Interface
