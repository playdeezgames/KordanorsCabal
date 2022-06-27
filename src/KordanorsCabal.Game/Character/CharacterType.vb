Imports System.Runtime.CompilerServices

Public Enum CharacterType
    None
    N00b
End Enum
Module CharacterTypeExtensions
    <Extension>
    Function InitialStatistics(characterType As CharacterType) As IReadOnlyDictionary(Of CharacterStatisticType, Long)
        Return CharacterTypeDescriptors(characterType).InitialStatistics
    End Function
    <Extension>
    Function MaximumEncumbrance(characterType As CharacterType, character As Character) As Single
        Return CharacterTypeDescriptors(characterType).MaximumEncumbrance(character)
    End Function
End Module
