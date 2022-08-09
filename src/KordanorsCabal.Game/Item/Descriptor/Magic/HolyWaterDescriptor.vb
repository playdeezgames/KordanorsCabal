Friend Class HolyWaterDescriptor
    Inherits ItemTypeDescriptor

    Sub New()
        MyBase.New(
            "Holy ""Water""",,,,,,,,,,,,,,,,,,
            Function(character) character.CanFight AndAlso character.Location.Enemy(character).IsUndead)
    End Sub
    Public Overrides Sub Use(character As Character)
        Dim sfx As Sfx? = Nothing
        Dim damageRoll = RNG.RollDice("1d4")
        Dim enemy = character.Location.Enemy(character)
        Dim lines As New List(Of String) From {
            $"{ItemType.HolyWater.Name} deals {damageRoll} HP to {enemy.Name}!"
        }
        enemy.DoDamage(damageRoll)
        If enemy.IsDead Then
            Dim result = enemy.Kill(character)
            sfx = If(result.Item1, sfx)
            lines.AddRange(result.Item2)
        End If
        character.EnqueueMessage(sfx, lines.ToArray)
        character.DoCounterAttacks()
    End Sub
End Class
