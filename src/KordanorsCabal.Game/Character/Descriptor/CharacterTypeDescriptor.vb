Friend MustInherit Class CharacterTypeDescriptor
    MustOverride ReadOnly Property InitialStatistics As IReadOnlyDictionary(Of StatisticType, Long)
End Class
Friend Module CharacterTypeDescriptorUtility
    Friend ReadOnly CharacterTypeDescriptors As IReadOnlyDictionary(Of CharacterType, CharacterTypeDescriptor) =
        New Dictionary(Of CharacterType, CharacterTypeDescriptor) From
        {
            {CharacterType.N00b, New NoobDescriptor}
        }
End Module
