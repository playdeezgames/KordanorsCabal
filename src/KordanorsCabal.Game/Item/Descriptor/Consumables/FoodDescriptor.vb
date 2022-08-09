Friend Class FoodDescriptor
    Inherits ItemTypeDescriptor

    Sub New()
        MyBase.New(
            "Food",,,,,,,,,,,,,,,,,,
            Function(character) True)
    End Sub
    Public Overrides Sub Use(character As Character)
        Dim healRoll = 1
        character.ChangeStatistic(CharacterStatisticType.Wounds, -healRoll)
        character.Hunger = 0
        character.EnqueueMessage(
                $"Food heals up to {healRoll} HP!",
                $"You now have {character.CurrentHP} HP!")
    End Sub
End Class
