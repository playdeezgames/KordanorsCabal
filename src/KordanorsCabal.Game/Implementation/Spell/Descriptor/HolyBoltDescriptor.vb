Friend Class HolyBoltDescriptor
    Inherits SpellType

    Public Sub New(worldData As IWorldData, id As Long)
        MyBase.New(worldData, id)
    End Sub

    Public Overrides Sub Cast(character As ICharacter)
        If Not CanCast(character) Then
            character.EnqueueMessage($"You cannot cast {OldSpellType.HolyBolt.Name(WorldData)} now!")
            Return
        End If
        Dim enemy = character.Location.Enemy(character)
        Dim lines As New List(Of String)
        Dim sfx As Sfx? = Nothing
        lines.Add($"You cast {OldSpellType.HolyBolt.Name(WorldData)} on {enemy.Name}!")
        character.DoFatigue(1)
        Dim damage As Long = character.RollSpellDice(OldSpellType.HolyBolt)
        lines.Add($"You do {damage} damage!")
        enemy.DoDamage(damage)
        If enemy.IsDead Then
            Dim result = enemy.Kill(character)
            sfx = If(result.Item1, sfx)
            lines.AddRange(result.Item2)
        End If
        character.EnqueueMessage(sfx, lines.ToArray)
        character.DoCounterAttacks()
    End Sub
End Class
