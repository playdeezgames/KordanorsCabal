Public Class Events
    Implements IEventData
    Private ReadOnly checkerTable As IReadOnlyDictionary(Of String, Func(Of IWorldData, Long(), Boolean)) =
        New Dictionary(Of String, Func(Of IWorldData, Long(), Boolean)) From
        {
            {"CharacterCanCastHolyBolt",
                Function(worldData, parms)
                    Dim characterId = parms(0)
                    Dim character = Game.Character.FromId(worldData, characterId)
                    Dim enemy = character.Movement.Location.Factions.FirstEnemy(character)
                    If enemy Is Nothing OrElse Not enemy.Statuses.IsUndead Then
                        Return False
                    End If
                    Return character.Mana.CurrentMana > 0
                End Function},
            {"CharacterCanCastPurify",
                Function(worldData, parms)
                    Dim characterId = parms(0)
                    Dim character = Game.Character.FromId(worldData, characterId)
                    Return character.Mana.CurrentMana > 0
                End Function},
            {"CharacterCanAcceptCellarRatsQuest",
                Function(worldData, parms)
                    Dim characterId = parms(0)
                    Dim character = Game.Character.FromId(worldData, characterId)
                    Return Not character.Quest.Has(QuestType.FromId(worldData, 1L))
                End Function},
            {"CharacterCanCompleteCellarRatsQuest",
                Function(worldData, parms)
                    Dim characterId = parms(0)
                    Dim character = Game.Character.FromId(worldData, characterId)
                    Return character.Items.Inventory.ItemsOfType(ItemType.FromId(worldData, 21L)).Any()
                End Function},
            {"CanLearnHolyBolt", Function(worldData, parms) Character.FromId(worldData, parms(0)).Spellbook.CanLearn(SpellType.FromId(worldData, 1L))},
            {"CanUseBeer", Function(worldData, parms)
                               Dim character = Game.Character.FromId(worldData, parms(0))
                               Dim enemy = character.Movement.Location.Factions.FirstEnemy(character)
                               Return enemy Is Nothing OrElse enemy.Items.CanBeBribedWith(ItemType.FromId(worldData, 26))
                           End Function},
            {"IsInDungeon", Function(worldData, parms) Character.FromId(worldData, parms(0)).Movement.Location.LocationType.IsDungeon},
            {"AlwaysTrue", Function(worldData, parms) True},
            {"IsFightingUndead", Function(worldData, parms)
                                     Dim character = Game.Character.FromId(worldData, parms(0))
                                     Return character.PhysicalCombat.CanFight AndAlso character.Movement.Location.Factions.FirstEnemy(character).Statuses.IsUndead
                                 End Function},
            {"CanUseRottenEgg", Function(worldData, parms)
                                    Dim character = Game.Character.FromId(worldData, parms(0))
                                    Dim enemy = character.Movement.Location.Factions.FirstEnemy(character)
                                    Return (enemy IsNot Nothing AndAlso enemy.Items.CanBeBribedWith(Game.ItemType.FromId(worldData, 37)))
                                End Function},
            {"HasBong", Function(worldData, parms)
                            Dim character = Game.Character.FromId(worldData, parms(0))
                            Return character.Items.Inventory.ItemsOfType(ItemType.FromId(worldData, 33)).Any
                        End Function},
            {"CanUseAirShard", Function(worldData, parms)
                                   Dim character = Game.Character.FromId(worldData, parms(0))
                                   Return character.Movement.Location.LocationType.IsDungeon AndAlso character.Mana.CurrentMana > 0
                               End Function},
            {"CanUseEarthShard", Function(worldData, parms)
                                     Dim character = Game.Character.FromId(worldData, parms(0))
                                     Dim location = character.Movement.Location
                                     Return location.LocationType.IsDungeon AndAlso location.Factions.EnemiesOf(character).Any AndAlso character.Mana.CurrentMana > 0
                                 End Function},
            {"CanUsePr0n", Function(worldData, parms)
                               Dim character = Game.Character.FromId(worldData, parms(0))
                               Dim enemy = character.Movement.Location.Factions.FirstEnemy(character)
                               Return (enemy IsNot Nothing AndAlso enemy.Items.CanBeBribedWith(ItemType.FromId(worldData, 28))) OrElse enemy Is Nothing
                           End Function},
            {"CanUseFireShard", Function(worldData, parms)
                                    Dim character = Game.Character.FromId(worldData, parms(0))
                                    Dim location = character.Movement.Location
                                    Return location.LocationType.IsDungeon AndAlso location.Factions.EnemiesOf(character).Any AndAlso character.Mana.CurrentMana > 0
                                End Function},
            {"CanUseWaterShard", Function(worldData, parms)
                                     Dim character = Game.Character.FromId(worldData, parms(0))
                                     Return character.Movement.Location.LocationType.IsDungeon AndAlso character.Mana.CurrentMana > 0
                                 End Function},
            {"CanUseBottle", Function(worldData, parms)
                                 Dim character = Game.Character.FromId(worldData, parms(0))
                                 Dim enemy = character.Movement.Location.Factions.FirstEnemy(character)
                                 Return enemy IsNot Nothing AndAlso enemy.Items.CanBeBribedWith(ItemType.FromId(worldData, 30))
                             End Function},
            {"CanLearnPurify", Function(worldData, parms)
                                   Dim character = Game.Character.FromId(worldData, parms(0))
                                   Return character.Spellbook.CanLearn(SpellType.FromId(worldData, 2L))
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
                    If Not character.Spellbook.CanCastSpell(spellType) Then
                        character.EnqueueMessage(Nothing, $"You cannot cast {spellType.Name} now!")
                        Return
                    End If
                    Dim enemy = character.Movement.Location.Factions.FirstEnemy(character)
                    Dim lines As New List(Of String)
                    Dim sfx As Sfx? = Nothing
                    lines.Add($"You cast {spellType.Name} on {enemy.CharacterType.Name}!")
                    character.Mana.DoFatigue(1)
                    Dim damage As Long = character.Spellbook.RollSpellDice(spellType)
                    lines.Add($"You do {damage} damage!")
                    enemy.PhysicalCombat.DoDamage(damage)
                    If enemy.Health.IsDead Then
                        Dim result = enemy.PhysicalCombat.Kill(character)
                        sfx = If(result.Item1, sfx)
                        lines.AddRange(result.Item2)
                    End If
                    character.EnqueueMessage(sfx, lines.ToArray)
                    character.PhysicalCombat.DoCounterAttacks()
                End Sub},
            {"CharacterCastPurify",
                Sub(worldData, parms)
                    Dim characterId = parms(0)
                    Dim character = Game.Character.FromId(worldData, characterId)
                    character.Items.PurifyItems()
                    character.Mana.DoFatigue(1)
                    character.EnqueueMessage(Nothing, "You purify yer inventory!")
                End Sub},
            {"CharacterAcceptCellarRatsQuest",
                Sub(worldData, parms)
                    Dim characterId = parms(0)
                    Dim character = Game.Character.FromId(worldData, characterId)
                    character.EnqueueMessage(Nothing, "You accept the quest!")
                    worldData.CharacterQuest.Write(character.Id, 1L)
                    Dim ratCount = If(worldData.CharacterQuestCompletion.Read(character.Id, 1L), 0) + 1
                    Dim location = Game.Location.FromLocationType(worldData, LocationType.FromId(worldData, 7L)).Single
                    Dim initialStatistics = CharacterType.FromId(worldData, Constants.CharacterTypes.Rat).Spawning.InitialStatistics()
                    While ratCount > 0
                        Game.Character.Create(worldData, CharacterType.FromId(worldData, Constants.CharacterTypes.Rat), location, initialStatistics)
                        ratCount -= 1
                    End While
                End Sub},
            {"CharacterCompleteCellarRatsQuest",
                Sub(worldData, parms)
                    Dim characterId = parms(0)
                    Dim character = Game.Character.FromId(worldData, characterId)
                    character.EnqueueMessage(Nothing, "You complete the quest!")
                    Dim ratTails = character.Items.Inventory.ItemsOfType(ItemType.FromId(worldData, 21L)).Take(10)
                    For Each ratTail In ratTails
                        character.Statuses.Money += 1
                        ratTail.Destroy()
                    Next
                    worldData.CharacterQuest.Clear(character.Id, 1L)
                    worldData.CharacterQuestCompletion.Write(
                        character.Id,
                        1L,
                        If(worldData.CharacterQuestCompletion.Read(character.Id, 1L), 0) + 1)
                End Sub},
            {"PurifyFood",
                Sub(worldData, parms)
                    worldData.Item.WriteItemType(parms(0), 24L)
                End Sub},
            {"LearnHolyBolt",
                Sub(worldData, parms)
                    Dim character = Game.Character.FromId(worldData, parms(0))
                    character.Spellbook.Learn(SpellType.FromId(worldData, 1L))
                End Sub},
            {"LearnPurify",
                Sub(worldData, parms)
                    Dim character = Game.Character.FromId(worldData, parms(0))
                    character.Spellbook.Learn(SpellType.FromId(worldData, 2L))
                End Sub},
            {"EatFood",
                Sub(worldData, parms)
                    Dim character = Game.Character.FromId(worldData, parms(0))
                    Dim healRoll = 1
                    character.Statistics.ChangeStatistic(CharacterStatisticType.FromId(worldData, Constants.StatisticTypes.Wounds), -healRoll)
                    character.Statuses.Hunger = 0
                    character.EnqueueMessage(Nothing,
                        $"Food heals up to {healRoll} HP!",
                        $"You now have {character.Health.Current} HP!")
                End Sub},
            {"UseHolyWater",
                Sub(worldData, parms)
                    Dim character = Game.Character.FromId(worldData, parms(0))
                    Dim sfx As Sfx? = Nothing
                    Dim damageRoll = RNG.RollDice("1d4")
                    Dim enemy = character.Movement.Location.Factions.FirstEnemy(character)
                    Dim lines As New List(Of String) From {
                        $"{ItemType.FromId(worldData, 22L).Name} deals {damageRoll} HP to {enemy.CharacterType.Name}!"
                    }
                    enemy.PhysicalCombat.DoDamage(damageRoll)
                    If enemy.Health.IsDead Then
                        Dim result = enemy.PhysicalCombat.Kill(character)
                        sfx = If(result.Item1, sfx)
                        lines.AddRange(result.Item2)
                    End If
                    character.EnqueueMessage(sfx, lines.ToArray)
                    character.PhysicalCombat.DoCounterAttacks()
                End Sub},
            {"UseTownPortal",
                Sub(worldData, parms)
                    Dim character = Game.Character.FromId(worldData, parms(0))
                    Dim location = character.Movement.Location
                    Dim outDirection = Direction.FromId(worldData, 8L)
                    Dim inDirection = Direction.FromId(worldData, 7L)
                    location.Routes.DestroyRoute(outDirection)
                    Dim destination = Game.Location.FromLocationType(worldData, LocationType.FromId(worldData, 1L)).Single
                    destination.Routes.DestroyRoute(inDirection)
                    Route.Create(worldData, location, outDirection, RouteType.FromId(worldData, 10L), destination)
                    Route.Create(worldData, destination, inDirection, RouteType.FromId(worldData, 10L), location)
                    character.EnqueueMessage(Nothing, "A portal opens before you!")
                End Sub},
            {"UseAirShard",
                Sub(worldData, parms)
                    Dim character = Game.Character.FromId(worldData, parms(0))
                    character.Mana.CurrentMana -= 1
                    Dim level = character.Movement.Location.DungeonLevel
                    Dim locations = Location.FromLocationType(worldData, LocationType.FromId(worldData, 4L)).Where(Function(x) x.DungeonLevel.Id = level.Id)
                    character.Movement.Location = RNG.FromEnumerable(locations)
                    character.EnqueueMessage(Nothing, $"You use the {ItemType.FromId(worldData, 14L).Name} and suddenly find yerself somewhere else!")
                End Sub},
            {"UseRottenEgg",
                Sub(worldData, parms)
                    Dim character = Game.Character.FromId(worldData, parms(0))
                    Dim enemy = character.Movement.Location.Factions.FirstEnemy(character)
                    If enemy IsNot Nothing AndAlso enemy.Items.CanBeBribedWith(Game.ItemType.FromId(worldData, 37)) Then
                        character.EnqueueMessage(Nothing, $"You give {enemy.CharacterType.Name} the {Game.ItemType.FromId(worldData, 37).Name}, and they quickly wander off with a seeming great purpose.")
                        enemy.Destroy()
                        Return
                    End If
                    character.EnqueueMessage(Nothing, $"You cannot use that now!")
                End Sub},
            {"UsePr0n",
                Sub(worldData, parms)
                    Dim character = Game.Character.FromId(worldData, parms(0))
                    Dim enemy = character.Movement.Location.Factions.FirstEnemy(character)
                    If enemy IsNot Nothing AndAlso enemy.Items.CanBeBribedWith(ItemType.FromId(worldData, 28)) Then
                        character.EnqueueMessage(Nothing, $"You give {enemy.CharacterType.Name} the {ItemType.FromId(worldData, 28).Name}, and they quickly wander off with a seeming great purpose.")
                        enemy.Destroy()
                        Return
                    End If
                    If enemy IsNot Nothing Then
                        character.EnqueueMessage(Nothing, "Dude! now is not the time!")
                        Return
                    End If
                    Dim healRoll = RNG.RollDice("1d4")
                    character.Statistics.ChangeStatistic(CharacterStatisticType.FromId(worldData, Constants.StatisticTypes.Stress), -healRoll)
                    Dim lines As New List(Of String)
                    lines.Add($"You make use of {ItemType.FromId(worldData, Constants.StatisticTypes.ItemType28).Name}, which cheers you up by {healRoll} {CharacterStatisticType.FromId(worldData, 7).Name}.")
                    lines.Add($"You now have {character.MentalCombat.CurrentMP} {CharacterStatisticType.FromId(worldData, Constants.StatisticTypes.MP).Name}.")
                    Dim lotionItem = character.Items.Inventory.ItemsOfType(ItemType.FromId(worldData, 39)).FirstOrDefault
                    If lotionItem Is Nothing Then
                        lines.Add($"You also receive 10 {CharacterStatisticType.FromId(worldData, Constants.StatisticTypes.Chafing).Name}. Try {ItemType.FromId(worldData, 39).Name} next time.")
                        character.Statuses.Chafing = 10
                    Else
                        lines.Add($"You use a bit of {ItemType.FromId(worldData, 39).Name} to prevent {CharacterStatisticType.FromId(worldData, Constants.StatisticTypes.Chafing).Name}.")
                        lotionItem.Durability.Reduce(1)
                        If lotionItem.Durability.Current = 0 Then
                            lines.Add($"You ran out that bottle of {ItemType.FromId(worldData, 39).Name}.")
                            lotionItem.Destroy()
                            character.Items.Inventory.Add(Item.Create(worldData, 30))
                        End If
                    End If
                    character.EnqueueMessage(Nothing, lines.ToArray)
                End Sub},
            {"UseMagicEgg",
                Sub(worldData, parms)
                    Dim character = Game.Character.FromId(worldData, parms(0))
                    Dim table As IReadOnlyDictionary(Of Long, Integer) =
                        New Dictionary(Of Long, Integer) From
                        {
                            {26L, 500},
                            {19L, 8},
                            {17L, 4},
                            {10L, 250},
                            {24L, 1000},
                            {16L, 125},
                            {22L, 64},
                            {29L, 1},
                            {20L, 2},
                            {7L, 125},
                            {15L, 64},
                            {18L, 16},
                            {23L, 8},
                            {27L, 1}
                        }
                    Dim item = Game.Item.Create(worldData, RNG.FromGenerator(table))
                    character.EnqueueMessage(Nothing, $"You crack open the {ItemType.FromId(worldData, 25).Name} and find {item.Name} inside!")
                    character.Items.Inventory.Add(item)
                End Sub},
            {"UseBeer",
                Sub(worldData, parms)
                    Dim character = Game.Character.FromId(worldData, parms(0))
                    Dim enemy = character.Movement.Location.Factions.FirstEnemy(character)
                    If enemy IsNot Nothing AndAlso enemy.Items.CanBeBribedWith(ItemType.FromId(worldData, 26)) Then
                        enemy.Destroy()
                        character.EnqueueMessage(Nothing, $"You give {enemy.CharacterType.Name} the {ItemType.FromId(worldData, 26).Name}, and they wander off to get drunk.")
                        Return
                    End If
                    character.MentalCombat.CurrentMP = character.Statistics.GetStatistic(CharacterStatisticType.FromId(worldData, Constants.StatisticTypes.MP)).Value
                    character.Statuses.Drunkenness += 10
                    character.Items.Inventory.Add(Game.Item.Create(worldData, 30))
                    character.EnqueueMessage(Nothing, "You drink the beer, and suddenly feel braver!")
                End Sub},
            {"UseMoonPortal",
                Sub(worldData, parms)
                    Dim character = Game.Character.FromId(worldData, parms(0))
                    Dim location = character.Movement.Location
                    Dim outDirection = Direction.FromId(worldData, 8L)
                    Dim inDirection = Direction.FromId(worldData, 7L)
                    location.Routes.DestroyRoute(outDirection)
                    Dim destination = RNG.FromEnumerable(Game.Location.FromLocationType(worldData, LocationType.FromId(worldData, 8L)))
                    destination.Routes.DestroyRoute(inDirection)
                    Route.Create(worldData, location, outDirection, RouteType.FromId(worldData, 10L), destination)
                    Route.Create(worldData, destination, inDirection, RouteType.FromId(worldData, 10L), location)
                    character.EnqueueMessage(Nothing, "A portal opens before you!")
                End Sub},
            {"UseFireShard",
                Sub(worldData, parms)
                    Dim character = Game.Character.FromId(worldData, parms(0))
                    character.Mana.DoFatigue(1)
                    Dim enemy = character.Movement.Location.Factions.FirstEnemy(character)
                    Dim lines As New List(Of String)
                    Dim sfx As Sfx? = Nothing
                    lines.Add($"You use {ItemType.FromId(worldData, 13).Name} on {enemy.CharacterType.Name}!")
                    Dim damage As Long = RNG.RollDice("3d4")
                    lines.Add($"You do {damage} damage!")
                    enemy.PhysicalCombat.DoDamage(damage)
                    If enemy.Health.IsDead Then
                        Dim result = enemy.PhysicalCombat.Kill(character)
                        sfx = If(result.Item1, sfx)
                        lines.AddRange(result.Item2)
                    End If
                    character.EnqueueMessage(sfx, lines.ToArray)
                    character.PhysicalCombat.DoCounterAttacks()
                End Sub},
            {"DrinkPotion",
                Sub(worldData, parms)
                    Dim character = Game.Character.FromId(worldData, parms(0))
                    Dim healRoll = RNG.RollDice("2d4")
                    character.Statistics.ChangeStatistic(CharacterStatisticType.FromId(worldData, Constants.StatisticTypes.Wounds), -healRoll)
                    character.Items.Inventory.Add(Item.Create(worldData, 30))
                    character.EnqueueMessage(Nothing,
                $"Potion heals up to {healRoll} HP!",
                $"You now have {character.Health.Current} HP!")
                End Sub},
            {"UseHerb",
                Sub(worldData, parms)
                    Dim character = Game.Character.FromId(worldData, parms(0))
                    Dim delta = character.Mana.MaximumMana - character.Mana.CurrentMana
                    character.Mana.CurrentMana = character.Mana.MaximumMana
                    character.Statuses.Highness += 10
                    character.EnqueueMessage(Nothing, $"You use yer {ItemType.FromId(worldData, 33).Name} to smoke yer {ItemType.FromId(worldData, 34).Name}.", $"You gain {delta} {CharacterStatisticType.FromId(worldData, Constants.StatisticTypes.Mana).Name}.")
                End Sub},
            {"UseEarthShard",
                Sub(worldData, parms)
                    Dim character = Game.Character.FromId(worldData, parms(0))
                    character.Mana.DoFatigue(1)
                    Dim enemy = character.Movement.Location.Factions.FirstEnemy(character)
                    Dim lines As New List(Of String)
                    Dim sfx As Sfx? = Nothing
                    lines.Add($"You use {ItemType.FromId(worldData, 11).Name} on {enemy.CharacterType.Name}!")
                    Dim immobilization As Long = character.Spellbook.RollPower()
                    lines.Add($"You immobilize {enemy.CharacterType.Name} for {immobilization} turns!")
                    enemy.Statuses.DoImmobilization(immobilization)
                    character.EnqueueMessage(sfx, lines.ToArray)
                    character.PhysicalCombat.DoCounterAttacks()
                End Sub},
            {"UseWaterShard",
                Sub(worldData, parms)
                    Dim character = Game.Character.FromId(worldData, parms(0))
                    character.Mana.CurrentMana -= 1
                    character.Health.Heal()
                    character.EnqueueMessage(Nothing, $"You use {ItemType.FromId(worldData, 12L).Name} to heal yer wounds!")
                End Sub},
            {"UseBottle",
                Sub(worldData, parms)
                    Dim character = Game.Character.FromId(worldData, parms(0))
                    Dim enemy = character.Movement.Location.Factions.FirstEnemy(character)
                    character.EnqueueMessage(Nothing, $"You give the {ItemType.FromId(worldData, 30).Name} to the {enemy.CharacterType.Name}, and it wanders off happily.")
                    enemy.Destroy()
                End Sub},
            {"UseRottenFood",
                Sub(worldData, parms)
                    Dim character = Game.Character.FromId(worldData, parms(0))
                    If RNG.RollDice("1d2") = 1 Then
                        character.Statuses.Hunger \= 2
                        character.Statuses.FoodPoisoning = 10
                        character.EnqueueMessage(Nothing,
                        $"Food was rotten!",
                        $"You got food poisoning!")
                    Else
                        Dim healRoll = 1
                        character.Statistics.ChangeStatistic(CharacterStatisticType.FromId(worldData, Constants.StatisticTypes.Wounds), -healRoll)
                        character.Statuses.Hunger = 0
                        character.EnqueueMessage(Nothing,
                        $"Food heals up to {healRoll} HP!",
                        $"You now have {character.Health.Current} HP!")
                    End If
                End Sub},
            {"LocationDecayItems",
                Sub(worldData, parms)
                    Dim location = Game.Location.FromId(worldData, parms(0))
                    For Each item In location.Inventory.Items
                        item.Decay()
                    Next
                End Sub},
            {"FoodDecay",
                Sub(worldData, parms)
                    If RNG.RollDice("1d2") = 1 Then
                        worldData.Item.WriteItemType(parms(0), 35L)
                    End If
                End Sub}
        }
    Public Sub Perform(worldData As IWorldData, eventName As String, ParamArray parms() As Long) Implements IEventData.Perform
        actionTable(eventName)(worldData, parms)
    End Sub

    Public Function Test(worldData As IWorldData, eventName As String, ParamArray parms() As Long) As Boolean Implements IEventData.Test
        Return checkerTable(eventName)(worldData, parms)
    End Function
End Class
