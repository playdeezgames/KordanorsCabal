Public Class CharacterSpellbook
    Inherits BaseThingie
    Implements ICharacterSpellbook
    Private character As ICharacter

    Public Sub New(worldData As IWorldData, character As ICharacter)
        MyBase.New(worldData, character.Id)
        Me.character = character
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
            Character.EnqueueMessage($"You cannot learn {spellType.Name} at this time!")
            Return
        End If
        Dim nextLevel = If(WorldData.CharacterSpell.Read(Id, spellType.Id), 0) + 1
        character.EnqueueMessage($"You now know {spellType.Name} at level {nextLevel}.")
        WorldData.CharacterSpell.Write(Id, spellType.Id, nextLevel)
    End Sub
    Function CanLearn(spellType As ISpellType) As Boolean Implements ICharacterSpellbook.CanLearn
        Dim nextLevel = If(WorldData.CharacterSpell.Read(Id, spellType.Id), 0) + 1
        If nextLevel > spellType.MaximumLevel Then
            Return False
        End If
        Return If(character.GetStatistic(CharacterStatisticType.FromId(WorldData, Constants.StatisticTypes.Power)), 0) >= spellType.RequiredPower(nextLevel)
    End Function
    ReadOnly Property Power As Long
        Get
            Return character.GetStatistic(CharacterStatisticType.FromId(WorldData, Constants.StatisticTypes.Power)).Value
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
            character.EnqueueMessage($"You cannot cast {spellType.Name} at this time.")
            Return
        End If
        spellType.Cast(character)
    End Sub
    Public Function RollPower() As Long Implements ICharacterSpellbook.RollPower
        Return RollDice(If(character.GetStatistic(CharacterStatisticType.FromId(WorldData, Constants.StatisticTypes.Power)), 0) + NegativeInfluence())
    End Function
    Private Function RollDice(dice As Long) As Long
        Dim result As Long = 0
        While dice > 0
            result += RNG.RollDice("1d6/6")
            dice -= 1
        End While
        Return result
    End Function
    Private Function NegativeInfluence() As Long
        Return If(character.Drunkenness > 0 OrElse character.Highness > 0 OrElse character.Chafing > 0, -1, 0)
    End Function
End Class
