Public Class Events
    Implements IEventData
    Private ReadOnly checkerTable As IReadOnlyDictionary(Of String, Func(Of IWorldData, Long(), Boolean)) =
        New Dictionary(Of String, Func(Of IWorldData, Long(), Boolean)) From
        {
            {"CharacterCanCastHolyBolt",
                Function(worldData, parms)
                    Dim characterId = parms(0)
                    Dim character = Game.Character.FromId(worldData, characterId)
                    Dim enemy = character.Location.Enemy(character)
                    If enemy Is Nothing OrElse Not enemy.IsUndead Then
                        Return False
                    End If
                    Return character.CurrentMana > 0
                End Function},
            {"CharacterCanCastPurify",
                Function(worldData, parms)
                    Dim characterId = parms(0)
                    Dim character = Game.Character.FromId(worldData, characterId)
                    Return character.CurrentMana > 0
                End Function}
        }
    Private ReadOnly actionTable As IReadOnlyDictionary(Of String, Action(Of IWorldData, Long())) =
        New Dictionary(Of String, Action(Of IWorldData, Long())) From
        {
            {"CharacterCastHolyBolt",
                Sub(worldData, parms)
                    Dim characterId = parms(0)
                    Dim character = Game.Character.FromId(worldData, characterId)
                    Dim spellType = Game.SpellType.FromId(worldData, 1L)
                    If Not character.CanCastSpell(spellType) Then
                        character.EnqueueMessage($"You cannot cast {spellType.Name} now!")
                        Return
                    End If
                    Dim enemy = character.Location.Enemy(character)
                    Dim lines As New List(Of String)
                    Dim sfx As Sfx? = Nothing
                    lines.Add($"You cast {spellType.Name} on {enemy.Name}!")
                    character.DoFatigue(1)
                    Dim damage As Long = character.RollSpellDice(spellType)
                    lines.Add($"You do {damage} damage!")
                    enemy.DoDamage(damage)
                    If enemy.IsDead Then
                        Dim result = enemy.Kill(character)
                        sfx = If(result.Item1, sfx)
                        lines.AddRange(result.Item2)
                    End If
                    character.EnqueueMessage(sfx, lines.ToArray)
                    character.DoCounterAttacks()
                End Sub},
            {"CharacterCastPurify",
                Sub(worldData, parms)
                    Dim characterId = parms(0)
                    Dim character = Game.Character.FromId(worldData, characterId)
                    character.PurifyItems()
                    character.DoFatigue(1)
                    character.EnqueueMessage("You purify yer inventory!")
                End Sub}
        }
    Public Sub Perform(worldData As IWorldData, eventName As String, ParamArray parms() As Long) Implements IEventData.Perform
        actionTable(eventName)(worldData, parms)
    End Sub

    Public Function Test(worldData As IWorldData, eventName As String, ParamArray parms() As Long) As Boolean Implements IEventData.Test
        Return checkerTable(eventName)(worldData, parms)
    End Function
End Class
