Public Class CharacterSpellbook
    Inherits SubcharacterBase
    Implements ICharacterSpellbook

    Public Sub New(worldData As IWorldData, character As ICharacter)
        MyBase.New(worldData, character)
    End Sub

    Shared Function FromCharacter(worldData As IWorldData, character As ICharacter) As ICharacterSpellbook
        Return If(character IsNot Nothing, New CharacterSpellbook(worldData, character), Nothing)
    End Function
    ReadOnly Property Spells As IReadOnlyDictionary(Of Long, Long) Implements ICharacterSpellbook.Spells
        Get
            Return WorldData.CharacterSpell.
                ReadForCharacter(Id).
                ToDictionary(Function(x) x.Item1, Function(x) x.Item2)
        End Get
    End Property
    ReadOnly Property HasSpells As Boolean Implements ICharacterSpellbook.HasSpells
        Get
            Return Spells.Any
        End Get
    End Property
    Sub Learn(spellType As ISpellType) Implements ICharacterSpellbook.Learn
        If Not CanLearn(spellType) Then
            character.EnqueueMessage(Nothing, $"You cannot learn {spellType.Name} at this time!")
            Return
        End If
        Dim nextLevel = If(WorldData.CharacterSpell.Read(Id, spellType.Id), 0) + 1
        character.EnqueueMessage(Nothing, $"You now know {spellType.Name} at level {nextLevel}.")
        WorldData.CharacterSpell.Write(Id, spellType.Id, nextLevel)
    End Sub
    Function CanLearn(spellType As ISpellType) As Boolean Implements ICharacterSpellbook.CanLearn
        Dim nextLevel = If(WorldData.CharacterSpell.Read(Id, spellType.Id), 0) + 1
        If nextLevel > spellType.MaximumLevel Then
            Return False
        End If
        Return If(Character.Statistics.GetStatistic(CharacterStatisticType.FromId(WorldData, StatisticTypePower)), 0) >= spellType.RequiredPower(nextLevel)
    End Function
    ReadOnly Property Power As Long
        Get
            Return Character.Statistics.GetStatistic(CharacterStatisticType.FromId(WorldData, StatisticTypePower)).Value
        End Get
    End Property
    Function RollSpellDice(spellType As ISpellType) As Long Implements ICharacterSpellbook.RollSpellDice
        If Not Spells.ContainsKey(spellType.Id) Then
            Return 0
        End If
        Return RollDice(Power + Spells(spellType.Id))
    End Function
    Public Function CanCastSpell(spellType As ISpellType) As Boolean Implements ICharacterSpellbook.CanCastSpell
        Return spellType.CanCast(character)
    End Function
    Public Sub Cast(spellType As ISpellType) Implements ICharacterSpellbook.Cast
        If Not CanCastSpell(spellType) Then
            character.EnqueueMessage(Nothing, $"You cannot cast {spellType.Name} at this time.")
            Return
        End If
        spellType.Cast(character)
    End Sub
    Public Function RollPower() As Long Implements ICharacterSpellbook.RollPower
        Return RollDice(If(Character.Statistics.GetStatistic(CharacterStatisticType.FromId(WorldData, StatisticTypePower)), 0) + NegativeInfluence())
    End Function
End Class
