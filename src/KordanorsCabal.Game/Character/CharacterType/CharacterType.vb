Public Class CharacterType
    Inherits BaseThingie
    Implements ICharacterType
    ReadOnly Property Name As String Implements ICharacterType.Name
        Get
            Return WorldData.CharacterType.ReadName(Id)
        End Get
    End Property
    ReadOnly Property XPValue As Long Implements ICharacterType.XPValue
        Get
            Return If(WorldData.CharacterType.ReadXPValue(Id), 0L)
        End Get
    End Property
    ReadOnly Property IsUndead As Boolean Implements ICharacterType.IsUndead
        Get
            Return If(WorldData.CharacterType.ReadIsUndead(Id), 0) > 0
        End Get
    End Property
    Friend Sub New(worldData As IWorldData, characterTypeId As Long)
        MyBase.New(worldData, characterTypeId)
    End Sub

    Public ReadOnly Property Spawning As ICharacterTypeSpawning Implements ICharacterType.Spawning
        Get
            Return CharacterTypeSpawning.FromId(WorldData, Id)
        End Get
    End Property

    Public ReadOnly Property Combat As ICharacterTypeCombat Implements ICharacterType.Combat
        Get
            Return CharacterTypeCombat.FromId(WorldData, Id)
        End Get
    End Property
    Function RollMoneyDrop() As Long Implements ICharacterType.RollMoneyDrop
        Return RNG.RollDice(WorldData.CharacterType.ReadMoneyDropDice(Id))
    End Function
    Shared Function FromId(worldData As IWorldData, characterTypeId As Long) As ICharacterType
        Return New CharacterType(worldData, characterTypeId)
    End Function
End Class
