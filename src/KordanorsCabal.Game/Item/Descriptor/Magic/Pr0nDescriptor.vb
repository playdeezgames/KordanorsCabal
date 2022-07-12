Friend Class Pr0nDescriptor
    Inherits ItemTypeDescriptor

    Public Overrides ReadOnly Property SpawnLocationTypes(level As Long) As HashSet(Of LocationType)
        Get
            Return New HashSet(Of LocationType)
        End Get
    End Property

    Public Overrides ReadOnly Property Name As String
        Get
            Return "Pr0n Scroll"
        End Get
    End Property

    Public Overrides ReadOnly Property CanUse(character As Character) As Boolean
        Get
            Dim enemy = character.Location.Enemy(character)
            Return enemy IsNot Nothing AndAlso enemy.CanBeBribedWith(ItemType.Pr0n)
        End Get
    End Property

    Public Overrides Sub Use(character As Character)
        Dim enemy = character.Location.Enemy(character)
        If enemy Is Nothing OrElse Not enemy.CanBeBribedWith(ItemType.Pr0n) Then
            character.EnqueueMessage("You cannot use that now.")
            Return
        End If
        character.EnqueueMessage($"You give {enemy.Name} the {ItemType.Pr0n.Name}, and they quickly wander off with a seeming great purpose.")
        enemy.Destroy()
    End Sub
End Class
