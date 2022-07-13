Public MustInherit Class SpellDescriptor
    MustOverride ReadOnly Property Name As String

    MustOverride ReadOnly Property MaximumLevel As Long

    MustOverride ReadOnly Property RequiredPower(level As Long) As Long

    MustOverride ReadOnly Property CanCast(character As Character) As Boolean

    MustOverride Sub Cast(character As Character)
End Class
Friend Module SpellDescriptorUtility
    Friend ReadOnly SpellDescriptors As IReadOnlyDictionary(Of SpellType, SpellDescriptor) =
        New Dictionary(Of SpellType, SpellDescriptor) From
        {
            {SpellType.HolyBolt, New HolyBoltDescriptor}
        }
End Module
