Friend Class Pr0nDescriptor
    Inherits ItemTypeDescriptor

    Sub New()
        MyBase.New(
            ItemType.Pr0n,
            "Pr0n Scroll",,,,,,,,,,,,,
            10,
            MakeList(ShoppeType.BlackMarket),,,,
            Function(character)
                Dim enemy = character.Location.Enemy(character)
                Return (enemy IsNot Nothing AndAlso enemy.CanBeBribedWith(ItemType.Pr0n)) OrElse enemy Is Nothing
            End Function,
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
                character.ChangeStatistic(OldCharacterStatisticType.Stress.ToNew, -healRoll)
                Dim lines As New List(Of String)
                lines.Add($"You make use of {ItemType.Pr0n.Name}, which cheers you up by {healRoll} {OldCharacterStatisticType.MP.ToNew.Name}.")
                lines.Add($"You now have {character.CurrentMP} {OldCharacterStatisticType.MP.ToNew.Name}.")
                Dim lotionItem = character.Inventory.ItemsOfType(ItemType.Lotion).FirstOrDefault
                If lotionItem Is Nothing Then
                    lines.Add($"You also receive 10 {OldCharacterStatisticType.Chafing.ToNew.Name}. Try {ItemType.Lotion.Name} next time.")
                    character.Chafing = 10
                Else
                    lines.Add($"You use a bit of {ItemType.Lotion.Name} to prevent {OldCharacterStatisticType.Chafing.ToNew.Name}.")
                    lotionItem.ReduceDurability(1)
                    If lotionItem.Durability = 0 Then
                        lines.Add($"You ran out that bottle of {ItemType.Lotion.Name}.")
                        lotionItem.Destroy()
                        character.Inventory.Add(Item.Create(ItemType.Bottle))
                    End If
                End If

                character.EnqueueMessage(lines.ToArray)
            End Sub)
    End Sub
End Class
