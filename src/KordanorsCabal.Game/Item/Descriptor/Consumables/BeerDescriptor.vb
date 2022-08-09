Friend Class BeerDescriptor
    Inherits ItemTypeDescriptor

    Sub New()
        MyBase.New(
            "Beer",
            2,,,,,,,,,,,,,,,,,
            Function(character)
                Dim enemy = character.Location.Enemy(character)
                Return enemy Is Nothing OrElse enemy.CanBeBribedWith(ItemType.Beer)
            End Function)
    End Sub
    Public Overrides Sub Use(character As Character)
        Dim enemy = character.Location.Enemy(character)
        If enemy IsNot Nothing AndAlso enemy.CanBeBribedWith(ItemType.Beer) Then
            enemy.Destroy()
            character.EnqueueMessage($"You give {enemy.Name} the {ItemType.Beer.Name}, and they wander off to get drunk.")
            Return
        End If
        character.CurrentMP = character.GetStatistic(CharacterStatisticType.MP).Value
        character.Drunkenness += 10
        character.Inventory.Add(Game.Item.Create(ItemType.Bottle))
        character.EnqueueMessage("You drink the beer, and suddenly feel braver!")
    End Sub
End Class
