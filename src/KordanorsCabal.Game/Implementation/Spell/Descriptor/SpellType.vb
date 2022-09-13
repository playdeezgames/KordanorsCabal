Public MustInherit Class SpellType
    MustOverride ReadOnly Property Name As String

    MustOverride ReadOnly Property MaximumLevel As Long

    MustOverride ReadOnly Property RequiredPower(level As Long) As Long

    MustOverride ReadOnly Property CanCast(character As ICharacter) As Boolean

    MustOverride Sub Cast(character As ICharacter)
End Class
Friend Module SpellDescriptorUtility
    Friend ReadOnly SpellDescriptors As IReadOnlyDictionary(Of OldSpellType, SpellType) =
        New Dictionary(Of OldSpellType, SpellType) From
        {
            {OldSpellType.HolyBolt, New HolyBoltDescriptor},
            {OldSpellType.Purify, New PurifyDescriptor}
        }
End Module
