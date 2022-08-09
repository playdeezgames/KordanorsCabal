Friend Class AirShardDescriptor
    Inherits ItemTypeDescriptor

    Sub New()
        MyBase.New("Air Shard",,
            MakeDictionary(
                (DungeonLevel.Level1, MakeHashSet(LocationType.DungeonBoss))),
            MakeDictionary(
                (DungeonLevel.Level1, "1d1")),,,,,, ,
            False,,,,,,,,
            Function(character) character.Location.IsDungeon AndAlso character.CurrentMana > 0)
    End Sub

    Public Overrides Sub Use(character As Character)
        If Not CanUse(character) Then
            character.EnqueueMessage($"You cannot use {ItemType.AirShard.Name} right now!")
            Return
        End If
        character.CurrentMana -= 1
        Dim level = character.Location.DungeonLevel
        Dim locations = Location.FromLocationType(LocationType.Dungeon).Where(Function(x) x.DungeonLevel = level)
        character.Location = RNG.FromEnumerable(locations)
        character.EnqueueMessage($"You use the {ItemType.AirShard.Name} and suddenly find yerself somewhere else!")
    End Sub
End Class
