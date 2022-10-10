Module ItemTypeActions
    Friend ReadOnly PurifyActions As IReadOnlyDictionary(Of String, Action(Of IWorldData, IItem)) =
        New Dictionary(Of String, Action(Of IWorldData, IItem)) From
        {
            {"PurifyFood", Sub(worldData, item)
                               worldData.Item.WriteItemType(item.Id, 24L)
                           End Sub}
        }
    Friend ReadOnly UseActions As IReadOnlyDictionary(Of String, Action(Of IWorldData, ICharacter)) =
        New Dictionary(Of String, Action(Of IWorldData, ICharacter)) From
        {
            {"LearnHolyBolt", Sub(worldData, character) character.Learn(SpellType.FromId(worldData, 1L))},
            {"LearnPurify", Sub(worldData, character) character.Learn(SpellType.FromId(worldData, 2L))},
            {"EatFood",
                Sub(worldData, character)
                    Dim healRoll = 1
                    character.ChangeStatistic(CharacterStatisticType.FromId(worldData, 12L), -healRoll)
                    character.Hunger = 0
                    character.EnqueueMessage(
                        $"Food heals up to {healRoll} HP!",
                        $"You now have {character.CurrentHP} HP!")
                End Sub},
            {"UseHolyWater",
                Sub(worldData, character)
                    Dim sfx As Sfx? = Nothing
                    Dim damageRoll = RNG.RollDice("1d4")
                    Dim enemy = character.Location.Enemy(character)
                    Dim lines As New List(Of String) From {
                        $"{ItemType.FromId(worldData, 22L).Name} deals {damageRoll} HP to {enemy.Name}!"
                    }
                    enemy.DoDamage(damageRoll)
                    If enemy.IsDead Then
                        Dim result = enemy.Kill(character)
                        sfx = If(result.Item1, sfx)
                        lines.AddRange(result.Item2)
                    End If
                    character.EnqueueMessage(sfx, lines.ToArray)
                    character.DoCounterAttacks()
                End Sub},
            {"UseTownPortal",
                Sub(worldData, character)
                    Dim location = character.Location
                    Dim outDirection = Direction.FromId(worldData, 8L)
                    Dim inDirection = Direction.FromId(worldData, 7L)
                    location.DestroyRoute(outDirection)
                    Dim destination = Game.Location.FromLocationType(worldData, LocationType.FromId(worldData, 1L)).Single
                    destination.DestroyRoute(inDirection)
                    Route.Create(worldData, location, outDirection, OldRouteType.Portal, destination)
                    Route.Create(worldData, destination, inDirection, OldRouteType.Portal, location)
                    character.EnqueueMessage("A portal opens before you!")
                End Sub},
            {"UseAirShard",
                Sub(worldData, character)
                    character.CurrentMana -= 1
                    Dim level = character.Location.DungeonLevel
                    Dim locations = Location.FromLocationType(worldData, LocationType.FromId(worldData, 4L)).Where(Function(x) x.DungeonLevel.Id = level.Id)
                    character.Location = RNG.FromEnumerable(locations)
                    character.EnqueueMessage($"You use the {ItemType.FromId(worldData, 14L).Name} and suddenly find yerself somewhere else!")
                End Sub},
            {"UseRottenEgg",
                Sub(worldData, character)
                    Dim enemy = character.Location.Enemy(character)
                    If enemy IsNot Nothing AndAlso enemy.CanBeBribedWith(Game.ItemType.FromId(worldData, 37)) Then
                        character.EnqueueMessage($"You give {enemy.Name} the {Game.ItemType.FromId(worldData, 37).Name}, and they quickly wander off with a seeming great purpose.")
                        enemy.Destroy()
                        Return
                    End If
                    character.EnqueueMessage($"You cannot use that now!")
                End Sub},
            {"UsePr0n",
                        Sub(worldData, character)
                            Dim enemy = character.Location.Enemy(character)
                            If enemy IsNot Nothing AndAlso enemy.CanBeBribedWith(ItemType.FromId(worldData, 28)) Then
                                character.EnqueueMessage($"You give {enemy.Name} the {ItemType.FromId(worldData, 28).Name}, and they quickly wander off with a seeming great purpose.")
                                enemy.Destroy()
                                Return
                            End If
                            If enemy IsNot Nothing Then
                                character.EnqueueMessage("Dude! now is not the time!")
                                Return
                            End If
                            Dim healRoll = RNG.RollDice("1d4")
                            character.ChangeStatistic(CharacterStatisticType.FromId(worldData, 13L), -healRoll)
                            Dim lines As New List(Of String)
                            lines.Add($"You make use of {ItemType.FromId(worldData, 28).Name}, which cheers you up by {healRoll} {CharacterStatisticType.FromId(worldData, 7).Name}.")
                            lines.Add($"You now have {character.CurrentMP} {CharacterStatisticType.FromId(worldData, 7).Name}.")
                            Dim lotionItem = character.Inventory.ItemsOfType(ItemType.FromId(worldData, 39)).FirstOrDefault
                            If lotionItem Is Nothing Then
                                lines.Add($"You also receive 10 {CharacterStatisticType.FromId(worldData, 22L).Name}. Try {ItemType.FromId(worldData, 39).Name} next time.")
                                character.Chafing = 10
                            Else
                                lines.Add($"You use a bit of {ItemType.FromId(worldData, 39).Name} to prevent {CharacterStatisticType.FromId(worldData, 22L).Name}.")
                                lotionItem.Durability.Reduce(1)
                                If lotionItem.Durability.Current = 0 Then
                                    lines.Add($"You ran out that bottle of {ItemType.FromId(worldData, 39).Name}.")
                                    lotionItem.Destroy()
                                    character.Inventory.Add(Item.Create(worldData, 30))
                                End If
                            End If

                            character.EnqueueMessage(lines.ToArray)
                        End Sub},
            {"UseMagicEgg",
                Sub(worldData, character)
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
                    character.EnqueueMessage($"You crack open the {ItemType.FromId(worldData, 25).Name} and find {item.Name} inside!")
                    character.Inventory.Add(item)
                End Sub},
            {"UseBeer",
                Sub(worldData, character)
                    Dim enemy = character.Location.Enemy(character)
                    If enemy IsNot Nothing AndAlso enemy.CanBeBribedWith(ItemType.FromId(worldData, 26)) Then
                        enemy.Destroy()
                        character.EnqueueMessage($"You give {enemy.Name} the {ItemType.FromId(worldData, 26).Name}, and they wander off to get drunk.")
                        Return
                    End If
                    character.CurrentMP = character.GetStatistic(CharacterStatisticType.FromId(worldData, 7)).Value
                    character.Drunkenness += 10
                    character.Inventory.Add(Game.Item.Create(worldData, 30))
                    character.EnqueueMessage("You drink the beer, and suddenly feel braver!")
                End Sub},
            {"UseMoonPortal",
                Sub(worldData, character)
                    Dim location = character.Location
                    Dim outDirection = Direction.FromId(worldData, 8L)
                    Dim inDirection = Direction.FromId(worldData, 7L)
                    location.DestroyRoute(outDirection)
                    Dim destination = RNG.FromEnumerable(Game.Location.FromLocationType(worldData, LocationType.FromId(worldData, 8L)))
                    destination.DestroyRoute(inDirection)
                    Route.Create(worldData, location, outDirection, OldRouteType.Portal, destination)
                    Route.Create(worldData, destination, inDirection, OldRouteType.Portal, location)
                    character.EnqueueMessage("A portal opens before you!")
                End Sub},
            {"UseFireShard",
                Sub(worldData, character)
                    character.DoFatigue(1)
                    Dim enemy = character.Location.Enemy(character)
                    Dim lines As New List(Of String)
                    Dim sfx As Sfx? = Nothing
                    lines.Add($"You use {ItemType.FromId(worldData, 13).Name} on {enemy.Name}!")
                    Dim damage As Long = RNG.RollDice("3d4")
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
            {"DrinkPotion",
                Sub(worldData, character)
                    Dim healRoll = RNG.RollDice("2d4")
                    character.ChangeStatistic(CharacterStatisticType.FromId(worldData, 12L), -healRoll)
                    character.Inventory.Add(Item.Create(worldData, 30))
                    character.EnqueueMessage(
                $"Potion heals up to {healRoll} HP!",
                $"You now have {character.CurrentHP} HP!")
                End Sub},
            {"UseHerb",
                Sub(worldData, character)
                    Dim delta = character.MaximumMana - character.CurrentMana
                    character.CurrentMana = character.MaximumMana
                    character.Highness += 10
                    character.EnqueueMessage($"You use yer {ItemType.FromId(worldData, 33).Name} to smoke yer {ItemType.FromId(worldData, 34).Name}.", $"You gain {delta} {CharacterStatisticType.FromId(worldData, 8L).Name}.")
                End Sub},
            {"UseEarthShard",
                Sub(worldData, character)
                    character.DoFatigue(1)
                    Dim enemy = character.Location.Enemy(character)
                    Dim lines As New List(Of String)
                    Dim sfx As Sfx? = Nothing
                    lines.Add($"You use {ItemType.FromId(worldData, 11).Name} on {enemy.Name}!")
                    Dim immobilization As Long = character.RollPower()
                    lines.Add($"You immobilize {enemy.Name} for {immobilization} turns!")
                    enemy.DoImmobilization(immobilization)
                    character.EnqueueMessage(sfx, lines.ToArray)
                    character.DoCounterAttacks()
                End Sub},
            {"UseWaterShard",
                Sub(worldData, character)
                    character.CurrentMana -= 1
                    character.Heal()
                    character.EnqueueMessage($"You use {ItemType.FromId(worldData, 12L).Name} to heal yer wounds!")
                End Sub},
            {"UseBottle",
                Sub(worldData, character)
                    Dim enemy = character.Location.Enemy(character)
                    character.EnqueueMessage($"You give the {ItemType.FromId(worldData, 30).Name} to the {enemy.Name}, and it wanders off happily.")
                    enemy.Destroy()
                End Sub},
            {"UseRottenFood",
                Sub(worldData, character)
                    If RNG.RollDice("1d2") = 1 Then
                        character.Hunger \= 2
                        character.FoodPoisoning = 10
                        character.EnqueueMessage(
                        $"Food was rotten!",
                        $"You got food poisoning!")
                    Else
                        Dim healRoll = 1
                        character.ChangeStatistic(CharacterStatisticType.FromId(worldData, 12L), -healRoll)
                        character.Hunger = 0
                        character.EnqueueMessage(
                        $"Food heals up to {healRoll} HP!",
                        $"You now have {character.CurrentHP} HP!")
                    End If
                End Sub}
        }
    Friend ReadOnly CanUseFunctions As IReadOnlyDictionary(Of String, Func(Of IWorldData, ICharacter, Boolean)) =
        New Dictionary(Of String, Func(Of IWorldData, ICharacter, Boolean)) From
        {
            {"CanLearnHolyBolt", Function(worldData, character) character.CanLearn(SpellType.FromId(worldData, 1L))},
            {"CanUseBeer", Function(worldData, character)
                               Dim enemy = character.Location.Enemy(character)
                               Return enemy Is Nothing OrElse enemy.CanBeBribedWith(ItemType.FromId(worldData, 26))
                           End Function},
            {"IsInDungeon", Function(worldData, character) character.Location.IsDungeon},
            {"AlwaysTrue", Function(worldData, character) True},
            {"IsFightingUndead", Function(worldData, character) character.CanFight AndAlso character.Location.Enemy(character).IsUndead},
            {"CanUseRottenEgg", Function(worldData, character)
                                    Dim enemy = character.Location.Enemy(character)
                                    Return (enemy IsNot Nothing AndAlso enemy.CanBeBribedWith(Game.ItemType.FromId(worldData, 37)))
                                End Function},
            {"HasBong", Function(worldData, character) character.Inventory.ItemsOfType(ItemType.FromId(worldData, 33)).Any},
            {"CanUseAirShard", Function(worldData, character) character.Location.IsDungeon AndAlso character.CurrentMana > 0},
            {"CanUseEarthShard", Function(worldData, character)
                                     Dim location = character.Location
                                     Return location.IsDungeon AndAlso location.Enemies(character).Any AndAlso character.CurrentMana > 0
                                 End Function},
            {"CanUsePr0n", Function(worldData, character)
                               Dim enemy = character.Location.Enemy(character)
                               Return (enemy IsNot Nothing AndAlso enemy.CanBeBribedWith(ItemType.FromId(worldData, 28))) OrElse enemy Is Nothing
                           End Function},
            {"CanUseFireShard", Function(worldData, character)
                                    Dim location = character.Location
                                    Return location.IsDungeon AndAlso location.Enemies(character).Any AndAlso character.CurrentMana > 0
                                End Function},
            {"CanUseWaterShard", Function(worldData, character) character.Location.IsDungeon AndAlso character.CurrentMana > 0},
            {"CanUseBottle", Function(worldData, character)
                                 Dim enemy = character.Location.Enemy(character)
                                 Return enemy IsNot Nothing AndAlso enemy.CanBeBribedWith(ItemType.FromId(worldData, 30))
                             End Function},
            {"CanLearnPurify", Function(worldData, character) character.CanLearn(SpellType.FromId(worldData, 2L))}
        }
End Module
