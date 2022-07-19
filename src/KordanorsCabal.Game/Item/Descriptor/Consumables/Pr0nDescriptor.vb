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
            Return (enemy IsNot Nothing AndAlso enemy.CanBeBribedWith(ItemType.Pr0n)) OrElse enemy Is Nothing
        End Get
    End Property

    Public Overrides Sub Use(character As Character)
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
        character.ChangeStatistic(CharacterStatisticType.Stress, -healRoll)
        Dim lines As New List(Of String)
        lines.Add($"You make use of {ItemType.Pr0n.Name}, which cheers you up by {healRoll} {CharacterStatisticType.MP.Name}.")
        lines.Add($"You now have {character.CurrentMP} {CharacterStatisticType.MP.Name}.")
        lines.Add($"You also receive 10 chafing. Try lotion next time.")
        character.EnqueueMessage(lines.ToArray)
    End Sub
End Class
