﻿Friend Class WaterShardDescriptor
    Inherits ItemTypeDescriptor

    Sub New()
        MyBase.New(
            ItemType.WaterShard,
            "Water Shard",,
            MakeDictionary(
                (4L, MakeHashSet(LocationType.FromId(DungeonBoss)))),
            MakeDictionary(
                (4L, "1d1")),,,,,, ,
            False,,,,,,,,
            Function(character) character.Location.IsDungeon AndAlso character.CurrentMana > 0,
            Sub(character)
                character.CurrentMana -= 1
                character.Heal()
                character.EnqueueMessage($"You use {ItemType.WaterShard.Name} to heal yer wounds!")
            End Sub)
    End Sub
End Class
