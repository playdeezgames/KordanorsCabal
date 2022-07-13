Public MustInherit Class SpellDescriptor
    MustOverride ReadOnly Property Name As String
End Class
Friend Module SpellDescriptorUtility
    Friend ReadOnly SpellDescriptors As IReadOnlyDictionary(Of SpellType, SpellDescriptor) =
        New Dictionary(Of SpellType, SpellDescriptor) From
        {
            {SpellType.HolyBolt, New HolyBoltDescriptor}
        }
End Module
