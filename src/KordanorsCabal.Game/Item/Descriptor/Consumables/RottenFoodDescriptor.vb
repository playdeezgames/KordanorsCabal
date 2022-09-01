Friend Class RottenFoodDescriptor
    Inherits ItemTypeDescriptor

    Sub New()
        MyBase.New(
            StaticWorldData.World,
            ItemType.RottenFood,,,,,,,,,,,,,,,
            Sub(item)
                StaticWorldData.World.Item.WriteItemType(item.Id, ItemType.Food)
            End Sub,
            Function(character) True,
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
            End Sub)
    End Sub
End Class
