Friend Class BeerDescriptor
    Inherits ItemTypeDescriptor

    Sub New()
        MyBase.New(
            StaticWorldData.World,
            ItemType.Beer,
            2,,,,,,,,,,,
            5,
            MakeList(ShoppeType.InnKeeper),,,,
            Function(character)
                Dim enemy = character.Location.Enemy(character)
                Return enemy Is Nothing OrElse enemy.CanBeBribedWith(ItemType.Beer)
            End Function,
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
            End Sub)
    End Sub
End Class
