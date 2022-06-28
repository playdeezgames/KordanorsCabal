Friend MustInherit Class CharacterTypeDescriptor
    MustOverride ReadOnly Property InitialStatistics As IReadOnlyDictionary(Of CharacterStatisticType, Long)
    MustOverride ReadOnly Property MaximumEncumbrance(character As Character) As Single
    MustOverride Function IsEnemy(character As Character) As Boolean
End Class
Friend Module CharacterTypeDescriptorUtility
    Friend ReadOnly CharacterTypeDescriptors As IReadOnlyDictionary(Of CharacterType, CharacterTypeDescriptor) =
        New Dictionary(Of CharacterType, CharacterTypeDescriptor) From
        {
            {CharacterType.N00b, New NoobDescriptor}
        }
End Module
