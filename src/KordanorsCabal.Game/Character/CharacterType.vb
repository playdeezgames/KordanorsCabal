Imports System.Runtime.CompilerServices

Public Enum CharacterType
    None
    N00b
End Enum
Module CharacterTypeExtensions
    <Extension>
    Function InitialStatistics(characterType As CharacterType) As IReadOnlyDictionary(Of StatisticType, Long)
        Return CharacterTypeDescriptors(characterType).InitialStatistics
    End Function
End Module
