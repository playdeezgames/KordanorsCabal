Friend Class RottenFoodDescriptor
    Inherits ItemTypeDescriptor

    Sub New()
        MyBase.New(
            "Food",,,,,,,,,,,,,,,,,
            Sub(item)
                StaticWorldData.World.Item.WriteItemType(item.Id, ItemType.Food)
            End Sub,
            Function(character) True)
    End Sub

    Public Overrides Sub Use(character As Character)
        If RNG.RollDice("1d2") = 1 Then
            UseRotten(character)
        Else
            UseFresh(character)
        End If
    End Sub

    Private Sub UseRotten(character As Character)
        character.Hunger \= 2
        character.FoodPoisoning = 10
        character.EnqueueMessage(
                $"Food was rotten!",
                $"You got food poisoning!")
    End Sub

    Private Shared Sub UseFresh(character As Character)
        Dim healRoll = 1
        character.ChangeStatistic(CharacterStatisticType.Wounds, -healRoll)
        character.Hunger = 0
        character.EnqueueMessage(
                $"Food heals up to {healRoll} HP!",
                $"You now have {character.CurrentHP} HP!")
    End Sub
End Class
