Friend Class FoodDescriptor
    Inherits ItemTypeDescriptor

    Sub New()
        MyBase.New(
            "Food",,,,,,,,,,,,,
            2,
            MakeList(ShoppeType.InnKeeper),,,,
            Function(character) True,
            Sub(character)
                Dim healRoll = 1
                character.ChangeStatistic(CharacterStatisticType.Wounds, -healRoll)
                character.Hunger = 0
                character.EnqueueMessage(
                $"Food heals up to {healRoll} HP!",
                $"You now have {character.CurrentHP} HP!")
            End Sub)
    End Sub
End Class
