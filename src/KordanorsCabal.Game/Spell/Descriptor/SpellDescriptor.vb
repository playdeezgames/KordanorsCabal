Public MustInherit Class SpellDescriptor
    MustOverride ReadOnly Property Name As String

    MustOverride ReadOnly Property MaximumLevel As Long

    MustOverride ReadOnly Property RequiredPower(level As Long) As Long

    MustOverride ReadOnly Property CanCast(character As ICharacter) As Boolean

    MustOverride Sub Cast(character As ICharacter)
End Class
Friend Module SpellDescriptorUtility
    Friend ReadOnly SpellDescriptors As IReadOnlyDictionary(Of SpellType, SpellDescriptor) =
        New Dictionary(Of SpellType, SpellDescriptor) From
        {
            {SpellType.HolyBolt, New HolyBoltDescriptor},
            {SpellType.Purify, New PurifyDescriptor}
        }
End Module
