Public MustInherit Class SpellType
    Inherits BaseThingie
    Implements ISpellType

    Protected Sub New(worldData As IWorldData, id As Long)
        MyBase.New(worldData, id)
    End Sub

    ReadOnly Property Name As String Implements ISpellType.Name
        Get
            Return WorldData.SpellType.ReadName(Id)
        End Get
    End Property

    ReadOnly Property MaximumLevel As Long Implements ISpellType.MaximumLevel
        Get
            Return If(WorldData.SpellType.ReadMaximumLevel(Id), 0)
        End Get
    End Property

    ReadOnly Property RequiredPower(level As Long) As Long Implements ISpellType.RequiredPower
        Get
            Return If(WorldData.SpellTypeRequiredPower.Read(Id, level), Long.MaxValue)
        End Get
    End Property

    MustOverride ReadOnly Property CanCast(character As ICharacter) As Boolean Implements ISpellType.CanCast

    MustOverride Sub Cast(character As ICharacter) Implements ISpellType.Cast
End Class
Friend Module SpellDescriptorUtility
    Friend ReadOnly SpellDescriptors As IReadOnlyDictionary(Of OldSpellType, Func(Of IWorldData, SpellType)) =
        New Dictionary(Of OldSpellType, Func(Of IWorldData, SpellType)) From
        {
            {OldSpellType.HolyBolt, Function(x) New HolyBoltDescriptor(x, OldSpellType.HolyBolt)},
            {OldSpellType.Purify, Function(x) New PurifyDescriptor(x, OldSpellType.Purify)}
        }
End Module
