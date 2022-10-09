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
                End Function},
            {"CharacterCanAcceptCellarRatsQuest",
                Function(worldData, parms)
                    Dim characterId = parms(0)
                    Dim character = Game.Character.FromId(worldData, characterId)
                    Return Not character.HasQuest(QuestType.FromId(worldData, 1L))
                End Function},
            {"CharacterCanCompleteCellarRatsQuest",
                Function(worldData, parms)
                    Dim characterId = parms(0)
                    Dim character = Game.Character.FromId(worldData, characterId)
                    Return character.Inventory.ItemsOfType(ItemType.FromId(worldData, 21L)).Any()
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
                End Sub},
            {"CharacterAcceptCellarRatsQuest",
                Sub(worldData, parms)
                    Dim characterId = parms(0)
                    Dim character = Game.Character.FromId(worldData, characterId)
                    character.EnqueueMessage("You accept the quest!")
                    worldData.CharacterQuest.Write(character.Id, 1L)
                    Dim ratCount = If(worldData.CharacterQuestCompletion.Read(character.Id, 1L), 0) + 1
                    Dim location = Game.Location.FromLocationType(worldData, LocationType.FromId(worldData, 7L)).Single
                    Dim initialStatistics = CharacterType.FromId(worldData, 13).Spawning.InitialStatistics()
                    While ratCount > 0
                        Game.Character.Create(worldData, CharacterType.FromId(worldData, 13), location, initialStatistics)
                        ratCount -= 1
                    End While
                End Sub},
            {"CharacterCompleteCellarRatsQuest",
                Sub(worldData, parms)
                    Dim characterId = parms(0)
                    Dim character = Game.Character.FromId(worldData, characterId)
                    character.EnqueueMessage("You complete the quest!")
                    Dim ratTails = character.Inventory.ItemsOfType(ItemType.FromId(worldData, 21L)).Take(10)
                    For Each ratTail In ratTails
                        character.Money += 1
                        ratTail.Destroy()
                    Next
                    worldData.CharacterQuest.Clear(character.Id, 1L)
                    worldData.CharacterQuestCompletion.Write(
                        character.Id,
                        1L,
                        If(worldData.CharacterQuestCompletion.Read(character.Id, 1L), 0) + 1)
                End Sub}
        }
    Public Sub Perform(worldData As IWorldData, eventName As String, ParamArray parms() As Long) Implements IEventData.Perform
        actionTable(eventName)(worldData, parms)
    End Sub

    Public Function Test(worldData As IWorldData, eventName As String, ParamArray parms() As Long) As Boolean Implements IEventData.Test
        Return checkerTable(eventName)(worldData, parms)
    End Function
End Class
