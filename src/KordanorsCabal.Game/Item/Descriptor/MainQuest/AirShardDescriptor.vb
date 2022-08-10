Friend Class AirShardDescriptor
    Inherits ItemTypeDescriptor

    Sub New()
        MyBase.New(
            ItemType.AirShard,
            "Air Shard",,
            MakeDictionary(
                (1L, MakeHashSet(LocationType.DungeonBoss))),
            MakeDictionary(
                (1L, "1d1")),,,,,, ,
            False,,,,,,,,
            Function(character) character.Location.IsDungeon AndAlso character.CurrentMana > 0,
            Sub(character)
                character.CurrentMana -= 1
                Dim level = character.Location.DungeonLevel
                Dim locations = Location.FromLocationType(LocationType.Dungeon).Where(Function(x) x.DungeonLevel = level)
                character.Location = RNG.FromEnumerable(locations)
                character.EnqueueMessage($"You use the {ItemType.AirShard.Name} and suddenly find yerself somewhere else!")
            End Sub)
    End Sub
End Class
