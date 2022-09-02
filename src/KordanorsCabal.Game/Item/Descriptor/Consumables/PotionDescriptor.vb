Friend Class PotionDescriptor
    Inherits ItemTypeDescriptor

    Sub New()
        MyBase.New(
            StaticWorldData.World,
            ItemType.Potion,
            2,,,,,,,,,
            15,
            MakeList(ShoppeType.Healer),,,,
            Function(character) True,
            Sub(character)
                Dim healRoll = RNG.RollDice("2d4")
                character.ChangeStatistic(CharacterStatisticType.FromId(StaticWorldData.World, 12L), -healRoll)
                character.Inventory.Add(Item.Create(StaticWorldData.World, ItemType.Bottle))
                character.EnqueueMessage(
                $"Potion heals up to {healRoll} HP!",
                $"You now have {character.CurrentHP} HP!")
            End Sub)
    End Sub
End Class
