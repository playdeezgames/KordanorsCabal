Public Class SpellType
    Inherits BaseThingie
    Implements ISpellType

    Protected Sub New(worldData As IWorldData, id As Long)
        MyBase.New(worldData, id)
    End Sub

    Public Shared Function FromId(worldData As IWorldData, id As Long) As ISpellType
        Return New SpellType(worldData, id)
    End Function

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

    ReadOnly Property CanCast(character As ICharacter) As Boolean Implements ISpellType.CanCast
        Get
            Return WorldData.Checker.Check(WorldData, WorldData.SpellType.ReadCastCheck(Id), character.Id)
        End Get
    End Property

    Sub Cast(character As ICharacter) Implements ISpellType.Cast
        WorldData.Checker.Act(WorldData, WorldData.SpellType.ReadCast(Id), character.Id)
    End Sub
End Class
