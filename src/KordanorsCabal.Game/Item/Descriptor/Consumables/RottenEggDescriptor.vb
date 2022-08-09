Friend Class RottenEggDescriptor
    Inherits ItemTypeDescriptor

    Sub New()
        MyBase.New(
            "Rotten Egg",,,,,,,,,,,,,,,,,,
            Function(character)
                Dim enemy = character.Location.Enemy(character)
                Return (enemy IsNot Nothing AndAlso enemy.CanBeBribedWith(ItemType.RottenEgg))
            End Function)
    End Sub
    Public Overrides Sub Use(character As Character)
        Dim enemy = character.Location.Enemy(character)
        If enemy IsNot Nothing AndAlso enemy.CanBeBribedWith(ItemType.RottenEgg) Then
            character.EnqueueMessage($"You give {enemy.Name} the {ItemType.RottenEgg.Name}, and they quickly wander off with a seeming great purpose.")
            enemy.Destroy()
            Return
        End If
        character.EnqueueMessage($"You cannot use that now!")
    End Sub
End Class
