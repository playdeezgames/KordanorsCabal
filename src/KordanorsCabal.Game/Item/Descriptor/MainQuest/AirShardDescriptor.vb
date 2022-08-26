Friend Class AirShardDescriptor
    Inherits ItemTypeDescriptor

    Sub New()
        MyBase.New(
            StaticWorldData.World,
            ItemType.AirShard,
            "Air Shard",,
            MakeDictionary(
                (1L, MakeHashSet(LocationType.FromId(StaticWorldData.World, 6L)))),
            MakeDictionary(
                (1L, "1d1")),,,,,, ,
            False,,,,,,,,
            Function(character) character.Location.IsDungeon AndAlso character.CurrentMana > 0,
            Sub(character)
                character.CurrentMana -= 1
                Dim level = character.Location.DungeonLevel
                Dim locations = Location.FromLocationType(StaticWorldData.World, LocationType.FromId(StaticWorldData.World, 4L)).Where(Function(x) x.DungeonLevel.Id = level.Id)
                character.Location = RNG.FromEnumerable(locations)
                character.EnqueueMessage($"You use the {ItemType.AirShard.Name} and suddenly find yerself somewhere else!")
            End Sub)
    End Sub
End Class
