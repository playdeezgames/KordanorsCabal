Friend Class WaterShardDescriptor
    Inherits ItemTypeDescriptor

    Sub New()
        MyBase.New("Water Shard",,
            MakeDictionary(
                (DungeonLevel.Level4, MakeHashSet(LocationType.DungeonBoss))),
            MakeDictionary(
                (DungeonLevel.Level4, "1d1")),,,,,, , False)
    End Sub

    Public Overrides ReadOnly Property CanUse(character As Character) As Boolean
        Get
            Return character.Location.IsDungeon AndAlso character.CurrentMana > 0
        End Get
    End Property

    Public Overrides Sub Use(character As Character)
        If Not CanUse(character) Then
            character.EnqueueMessage($"You cannot use {ItemType.WaterShard.Name} right now!")
            Return
        End If
        character.CurrentMana -= 1
        character.Heal()
        character.EnqueueMessage($"You use {ItemType.WaterShard.Name} to heal yer wounds!")
    End Sub
End Class
