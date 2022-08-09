Friend Class EarthShardDescriptor
    Inherits ItemTypeDescriptor
    Sub New()
        MyBase.New(
            ItemType.EarthShard,
            "Earth Shard",,
            MakeDictionary(
                (DungeonLevel.Level2, MakeHashSet(LocationType.DungeonBoss))),
            MakeDictionary(
                (DungeonLevel.Level2, "1d1")),,,,,, ,
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
                lines.Add($"You use {ItemType.EarthShard.Name} on {enemy.Name}!")
                Dim immobilization As Long = character.RollSpellDice(SpellType.HolyBolt)
                lines.Add($"You immobilize {enemy.Name} for {immobilization} turns!")
                enemy.DoImmobilization(immobilization)
                character.EnqueueMessage(sfx, lines.ToArray)
                character.DoCounterAttacks()
            End Sub)
    End Sub
End Class
