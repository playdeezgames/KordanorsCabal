Friend Class RottenEggDescriptor
    Inherits ItemTypeDescriptor

    Sub New()
        MyBase.New(
            StaticWorldData.World,
            ItemType.RottenEgg,,,,,,,,,,,,,,,
            Function(character)
                Dim enemy = character.Location.Enemy(character)
                Return (enemy IsNot Nothing AndAlso enemy.CanBeBribedWith(ItemType.RottenEgg))
            End Function,
            Sub(character)
                Dim enemy = character.Location.Enemy(character)
                If enemy IsNot Nothing AndAlso enemy.CanBeBribedWith(ItemType.RottenEgg) Then
                    character.EnqueueMessage($"You give {enemy.Name} the {ItemType.RottenEgg.Name}, and they quickly wander off with a seeming great purpose.")
                    enemy.Destroy()
                    Return
                End If
                character.EnqueueMessage($"You cannot use that now!")
            End Sub)
    End Sub
End Class
