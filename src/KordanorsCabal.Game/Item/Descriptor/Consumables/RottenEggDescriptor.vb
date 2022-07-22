Friend Class RottenEggDescriptor
    Inherits ItemTypeDescriptor

    Public Overrides ReadOnly Property SpawnLocationTypes(level As Long) As HashSet(Of LocationType)
        Get
            Return New HashSet(Of LocationType)
        End Get
    End Property

    Public Overrides ReadOnly Property Name As String
        Get
            Return "Rotten Egg"
        End Get
    End Property

    Public Overrides ReadOnly Property CanUse(character As Character) As Boolean
        Get
            Dim enemy = character.Location.Enemy(character)
            Return (enemy IsNot Nothing AndAlso enemy.CanBeBribedWith(ItemType.RottenEgg))
        End Get
    End Property

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
