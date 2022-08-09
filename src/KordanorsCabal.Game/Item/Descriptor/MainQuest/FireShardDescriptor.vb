Friend Class FireShardDescriptor
    Inherits ItemTypeDescriptor
    Sub New()
        MyBase.New("Fire Shard",,
            MakeDictionary(
                (OldDungeonLevel.Level3, MakeHashSet(LocationType.DungeonBoss))),
            MakeDictionary(
                (OldDungeonLevel.Level3, "1d1")))
    End Sub
    Public Overrides ReadOnly Property CanUse(character As Character) As Boolean
        Get
            Dim location = character.Location
            Return location.IsDungeon AndAlso location.Enemies(character).Any AndAlso character.CurrentMana > 0
        End Get
    End Property

    Public Overrides Sub Use(character As Character)
        If Not CanUse(character) Then
            character.EnqueueMessage($"You cannot use {ItemType.FireShard.Name} right now!")
            Return
        End If
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
    End Sub

    Public Overrides ReadOnly Property IsConsumed As Boolean
        Get
            Return False
        End Get
    End Property
End Class
