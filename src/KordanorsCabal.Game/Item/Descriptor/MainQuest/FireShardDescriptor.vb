Friend Class FireShardDescriptor
    Inherits ItemTypeDescriptor
    Sub New()
        MyBase.New(
            StaticWorldData.World,
            ItemType.FireShard,
            "Fire Shard",,
            MakeDictionary(
                (3L, MakeHashSet(LocationType.FromId(StaticWorldData.World, 6L)))),
            MakeDictionary(
                (3L, "1d1")),,,,,, ,
            False,,,,,,,,
            Function(character)
                Dim location = character.Location
                Return location.IsDungeon AndAlso location.Enemies(character).Any AndAlso character.CurrentMana > 0
            End Function,
            Sub(character)
                character.DoFatigue(1)
                Dim enemy = character.Location.Enemy(character)
                Dim lines As New List(Of String)
                Dim sfx As Sfx? = Nothing
                lines.Add($"You use {ItemType.FireShard.Name} on {enemy.Name}!")
                Dim damage As Long = RNG.RollDice("3d4")
                lines.Add($"You do {damage} damage!")
                enemy.DoDamage(damage)
                If enemy.IsDead Then
                    Dim result = enemy.Kill(character)
                    sfx = If(result.Item1, sfx)
                    lines.AddRange(result.Item2)
                End If
                character.EnqueueMessage(sfx, lines.ToArray)
                character.DoCounterAttacks()
            End Sub)
    End Sub
End Class
