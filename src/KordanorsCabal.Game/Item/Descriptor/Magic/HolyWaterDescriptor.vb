Friend Class HolyWaterDescriptor
    Inherits ItemTypeDescriptor

    Public Overrides ReadOnly Property SpawnLocationTypes(level As Long) As HashSet(Of LocationType)
        Get
            Return New HashSet(Of LocationType)
        End Get
    End Property

    Public Overrides ReadOnly Property Name As String
        Get
            Return "Holy ""Water"""
        End Get
    End Property

    Public Overrides ReadOnly Property CanUse(character As Character) As Boolean
        Get
            Return character.CanFight AndAlso character.Location.Enemy(character).IsUndead
        End Get
    End Property

    Public Overrides Sub Use(character As Character)
        Dim sfx As Sfx? = Nothing
        Dim damageRoll = RNG.RollDice("1d4")
        Dim enemy = character.Location.Enemy(character)
        Dim lines As New List(Of String)
        lines.Add($"{ItemType.HolyWater.Name} deals {damageRoll} HP to {enemy.Name}!")
        enemy.DoDamage(damageRoll)
        If enemy.IsDead Then
            sfx = Game.Character.KillEnemy(character, lines, enemy, sfx)
        End If
        character.EnqueueMessage(sfx, lines.ToArray)
        'TODO: do counter attacks!
    End Sub
End Class
