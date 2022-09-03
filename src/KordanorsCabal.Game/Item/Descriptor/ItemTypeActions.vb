﻿Module ItemTypeActions
    Friend ReadOnly PurifyActions As IReadOnlyDictionary(Of String, Action(Of Item)) =
        New Dictionary(Of String, Action(Of Item)) From
        {
            {"PurifyFood", Sub(item)
                               StaticWorldData.World.Item.WriteItemType(item.Id, ItemType.Food)
                           End Sub}
        }
    Friend ReadOnly UseActions As IReadOnlyDictionary(Of String, Action(Of Character)) =
        New Dictionary(Of String, Action(Of Character)) From
        {
            {"LearnHolyBolt", Sub(character) character.Learn(SpellType.HolyBolt)},
            {"LearnPurify", Sub(character) character.Learn(SpellType.Purify)},
            {"EatFood",
                Sub(character)
                    Dim healRoll = 1
                    character.ChangeStatistic(CharacterStatisticType.FromId(StaticWorldData.World, 12L), -healRoll)
                    character.Hunger = 0
                    character.EnqueueMessage(
                        $"Food heals up to {healRoll} HP!",
                        $"You now have {character.CurrentHP} HP!")
                End Sub},
            {"UseHolyWater",
                Sub(character)
                    Dim sfx As Sfx? = Nothing
                    Dim damageRoll = RNG.RollDice("1d4")
                    Dim enemy = character.Location.Enemy(character)
                    Dim lines As New List(Of String) From {
                        $"{ItemType.HolyWater.Name} deals {damageRoll} HP to {enemy.Name}!"
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
                Sub(character)
                    Dim location = character.Location
                    Dim outDirection = Direction.FromId(StaticWorldData.World, 8L)
                    Dim inDirection = Direction.FromId(StaticWorldData.World, 7L)
                    location.DestroyRoute(outDirection)
                    Dim destination = Game.Location.FromLocationType(StaticWorldData.World, LocationType.FromId(StaticWorldData.World, 1L)).Single
                    destination.DestroyRoute(inDirection)
                    Route.Create(StaticWorldData.World, location, outDirection, RouteType.Portal, destination)
                    Route.Create(StaticWorldData.World, destination, inDirection, RouteType.Portal, location)
                    character.EnqueueMessage("A portal opens before you!")
                End Sub},
            {"UseAirShard",
                Sub(character)
                    character.CurrentMana -= 1
                    Dim level = character.Location.DungeonLevel
                    Dim locations = Location.FromLocationType(StaticWorldData.World, LocationType.FromId(StaticWorldData.World, 4L)).Where(Function(x) x.DungeonLevel.Id = level.Id)
                    character.Location = RNG.FromEnumerable(locations)
                    character.EnqueueMessage($"You use the {ItemType.AirShard.Name} and suddenly find yerself somewhere else!")
                End Sub},
            {"UseRottenEgg",
                Sub(character)
                    Dim enemy = character.Location.Enemy(character)
                    If enemy IsNot Nothing AndAlso enemy.CanBeBribedWith(ItemType.RottenEgg) Then
                        character.EnqueueMessage($"You give {enemy.Name} the {ItemType.RottenEgg.Name}, and they quickly wander off with a seeming great purpose.")
                        enemy.Destroy()
                        Return
                    End If
                    character.EnqueueMessage($"You cannot use that now!")
                End Sub},
            {"UsePr0n",
                        Sub(character)
                            Dim enemy = character.Location.Enemy(character)
                            If enemy IsNot Nothing AndAlso enemy.CanBeBribedWith(ItemType.Pr0n) Then
                                character.EnqueueMessage($"You give {enemy.Name} the {ItemType.Pr0n.Name}, and they quickly wander off with a seeming great purpose.")
                                enemy.Destroy()
                                Return
                            End If
                            If enemy IsNot Nothing Then
                                character.EnqueueMessage("Dude! now is not the time!")
                                Return
                            End If
                            Dim healRoll = RNG.RollDice("1d4")
                            character.ChangeStatistic(CharacterStatisticType.FromId(StaticWorldData.World, 13L), -healRoll)
                            Dim lines As New List(Of String)
                            lines.Add($"You make use of {ItemType.Pr0n.Name}, which cheers you up by {healRoll} {CharacterStatisticType.FromId(StaticWorldData.World, 7).Name}.")
                            lines.Add($"You now have {character.CurrentMP} {CharacterStatisticType.FromId(StaticWorldData.World, 7).Name}.")
                            Dim lotionItem = character.Inventory.ItemsOfType(ItemType.Lotion).FirstOrDefault
                            If lotionItem Is Nothing Then
                                lines.Add($"You also receive 10 {CharacterStatisticType.FromId(StaticWorldData.World, 22L).Name}. Try {ItemType.Lotion.Name} next time.")
                                character.Chafing = 10
                            Else
                                lines.Add($"You use a bit of {ItemType.Lotion.Name} to prevent {CharacterStatisticType.FromId(StaticWorldData.World, 22L).Name}.")
                                lotionItem.ReduceDurability(1)
                                If lotionItem.Durability = 0 Then
                                    lines.Add($"You ran out that bottle of {ItemType.Lotion.Name}.")
                                    lotionItem.Destroy()
                                    character.Inventory.Add(Item.Create(StaticWorldData.World, ItemType.Bottle))
                                End If
                            End If

                            character.EnqueueMessage(lines.ToArray)
                        End Sub},
            {"UseMagicEgg",
                Sub(character)
                    Dim table As IReadOnlyDictionary(Of ItemType, Integer) =
                        New Dictionary(Of ItemType, Integer) From
                        {
                            {ItemType.Beer, 500},
                            {ItemType.BrodeSode, 8},
                            {ItemType.ChainMail, 4},
                            {ItemType.Dagger, 250},
                            {ItemType.Food, 1000},
                            {ItemType.Helmet, 125},
                            {ItemType.HolyWater, 64},
                            {ItemType.MoonPortal, 1},
                            {ItemType.PlateMail, 2},
                            {ItemType.Potion, 125},
                            {ItemType.Shield, 64},
                            {ItemType.Shortsword, 16},
                            {ItemType.TownPortal, 8},
                            {ItemType.Trousers, 1}
                        }
                    Dim item = Game.Item.Create(StaticWorldData.World, RNG.FromGenerator(table))
                    character.EnqueueMessage($"You crack open the {ItemType.MagicEgg.Name} and find {item.Name} inside!")
                    character.Inventory.Add(item)
                End Sub},
            {"UseBeer",
                Sub(character)
                    Dim enemy = character.Location.Enemy(character)
                    If enemy IsNot Nothing AndAlso enemy.CanBeBribedWith(ItemType.Beer) Then
                        enemy.Destroy()
                        character.EnqueueMessage($"You give {enemy.Name} the {ItemType.Beer.Name}, and they wander off to get drunk.")
                        Return
                    End If
                    character.CurrentMP = character.GetStatistic(CharacterStatisticType.FromId(StaticWorldData.World, 7)).Value
                    character.Drunkenness += 10
                    character.Inventory.Add(Game.Item.Create(StaticWorldData.World, ItemType.Bottle))
                    character.EnqueueMessage("You drink the beer, and suddenly feel braver!")
                End Sub},
            {"UseMoonPortal",
                Sub(character)
                    Dim location = character.Location
                    Dim outDirection = Direction.FromId(StaticWorldData.World, 8L)
                    Dim inDirection = Direction.FromId(StaticWorldData.World, 7L)
                    location.DestroyRoute(outDirection)
                    Dim destination = RNG.FromEnumerable(Game.Location.FromLocationType(StaticWorldData.World, LocationType.FromId(StaticWorldData.World, 8L)))
                    destination.DestroyRoute(inDirection)
                    Route.Create(StaticWorldData.World, location, outDirection, RouteType.Portal, destination)
                    Route.Create(StaticWorldData.World, destination, inDirection, RouteType.Portal, location)
                    character.EnqueueMessage("A portal opens before you!")
                End Sub},
            {"UseFireShard",
                Sub(character)
                    character.DoFatigue(1)
                    Dim enemy = character.Location.Enemy(character)
                    Dim lines As New List(Of String)
                    Dim sfx As Sfx? = Nothing
                    lines.Add($"You use {ItemType.FireShard.Name} on {enemy.Name}!")
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
                Sub(character)
                    Dim healRoll = RNG.RollDice("2d4")
                    character.ChangeStatistic(CharacterStatisticType.FromId(StaticWorldData.World, 12L), -healRoll)
                    character.Inventory.Add(Item.Create(StaticWorldData.World, ItemType.Bottle))
                    character.EnqueueMessage(
                $"Potion heals up to {healRoll} HP!",
                $"You now have {character.CurrentHP} HP!")
                End Sub},
            {"UseHerb",
                Sub(character)
                    Dim delta = character.MaximumMana - character.CurrentMana
                    character.CurrentMana = character.MaximumMana
                    character.Highness += 10
                    character.EnqueueMessage($"You use yer {ItemType.Bong.Name} to smoke yer {ItemType.Herb.Name}.", $"You gain {delta} {CharacterStatisticType.FromId(StaticWorldData.World, 8L).Name}.")
                End Sub},
            {"UseEarthShard",
                Sub(character)
                    character.DoFatigue(1)
                    Dim enemy = character.Location.Enemy(character)
                    Dim lines As New List(Of String)
                    Dim sfx As Sfx? = Nothing
                    lines.Add($"You use {ItemType.EarthShard.Name} on {enemy.Name}!")
                    Dim immobilization As Long = character.RollSpellDice(SpellType.HolyBolt)
                    lines.Add($"You immobilize {enemy.Name} for {immobilization} turns!")
                    enemy.DoImmobilization(immobilization)
                    character.EnqueueMessage(sfx, lines.ToArray)
                    character.DoCounterAttacks()
                End Sub},
            {"UseWaterShard",
                Sub(character)
                    character.CurrentMana -= 1
                    character.Heal()
                    character.EnqueueMessage($"You use {ItemType.WaterShard.Name} to heal yer wounds!")
                End Sub},
            {"UseBottle",
                Sub(character)
                    Dim enemy = character.Location.Enemy(character)
                    character.EnqueueMessage($"You give the {ItemType.Bottle.Name} to the {enemy.Name}, and it wanders off happily.")
                    enemy.Destroy()
                End Sub},
            {"UseRottenFood",
                Sub(character)
                    If RNG.RollDice("1d2") = 1 Then
                        character.Hunger \= 2
                        character.FoodPoisoning = 10
                        character.EnqueueMessage(
                        $"Food was rotten!",
                        $"You got food poisoning!")
                    Else
                        Dim healRoll = 1
                        character.ChangeStatistic(CharacterStatisticType.FromId(StaticWorldData.World, 12L), -healRoll)
                        character.Hunger = 0
                        character.EnqueueMessage(
                        $"Food heals up to {healRoll} HP!",
                        $"You now have {character.CurrentHP} HP!")
                    End If
                End Sub}
        }
    Friend ReadOnly CanUseFunctions As IReadOnlyDictionary(Of String, Func(Of Character, Boolean)) =
        New Dictionary(Of String, Func(Of Character, Boolean)) From
        {
            {"CanLearnHolyBolt", Function(character) character.CanLearn(SpellType.HolyBolt)},
            {"CanUseBeer", Function(character)
                               Dim enemy = character.Location.Enemy(character)
                               Return enemy Is Nothing OrElse enemy.CanBeBribedWith(ItemType.Beer)
                           End Function},
            {"IsInDungeon", Function(character) character.Location.IsDungeon},
            {"AlwaysTrue", Function(character) True},
            {"IsFightingUndead", Function(character) character.CanFight AndAlso character.Location.Enemy(character).IsUndead},
            {"CanUseRottenEgg", Function(character)
                                    Dim enemy = character.Location.Enemy(character)
                                    Return (enemy IsNot Nothing AndAlso enemy.CanBeBribedWith(ItemType.RottenEgg))
                                End Function},
            {"HasBong", Function(character) character.Inventory.ItemsOfType(ItemType.Bong).Any},
            {"CanUseAirShard", Function(character) character.Location.IsDungeon AndAlso character.CurrentMana > 0},
            {"CanUseEarthShard", Function(character)
                                     Dim location = character.Location
                                     Return location.IsDungeon AndAlso location.Enemies(character).Any AndAlso character.CurrentMana > 0
                                 End Function},
            {"CanUsePr0n", Function(character)
                               Dim enemy = character.Location.Enemy(character)
                               Return (enemy IsNot Nothing AndAlso enemy.CanBeBribedWith(ItemType.Pr0n)) OrElse enemy Is Nothing
                           End Function},
            {"CanUseFireShard", Function(character)
                                    Dim location = character.Location
                                    Return location.IsDungeon AndAlso location.Enemies(character).Any AndAlso character.CurrentMana > 0
                                End Function},
            {"CanUseWaterShard", Function(character) character.Location.IsDungeon AndAlso character.CurrentMana > 0},
            {"CanUseBottle", Function(character)
                                 Dim enemy = character.Location.Enemy(character)
                                 Return enemy IsNot Nothing AndAlso enemy.CanBeBribedWith(ItemType.Bottle)
                             End Function},
            {"CanLearnPurify", Function(character) character.CanLearn(SpellType.Purify)}
        }
End Module