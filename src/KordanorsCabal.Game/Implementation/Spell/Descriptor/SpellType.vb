Public MustInherit Class SpellType
    Inherits BaseThingie
    Implements ISpellType

    Protected Sub New(worldData As IWorldData, id As Long)
        MyBase.New(worldData, id)
    End Sub

    MustOverride ReadOnly Property Name As String Implements ISpellType.Name

    MustOverride ReadOnly Property MaximumLevel As Long Implements ISpellType.MaximumLevel

    MustOverride ReadOnly Property RequiredPower(level As Long) As Long Implements ISpellType.RequiredPower

    MustOverride ReadOnly Property CanCast(character As ICharacter) As Boolean Implements ISpellType.CanCast

    MustOverride Sub Cast(character As ICharacter) Implements ISpellType.Cast
End Class
Friend Module SpellDescriptorUtility
    Friend ReadOnly SpellDescriptors As IReadOnlyDictionary(Of OldSpellType, SpellType) =
        New Dictionary(Of OldSpellType, SpellType) From
        {
            {OldSpellType.HolyBolt, New HolyBoltDescriptor(StaticWorldData.World, OldSpellType.HolyBolt)},
            {OldSpellType.Purify, New PurifyDescriptor(StaticWorldData.World, OldSpellType.Purify)}
        }
End Module
