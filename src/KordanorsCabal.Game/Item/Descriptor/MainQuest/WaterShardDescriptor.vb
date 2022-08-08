﻿Friend Class WaterShardDescriptor
    Inherits ItemTypeDescriptor

    Sub New()
        MyBase.New("Water Shard",,
            MakeDictionary(
                (DungeonLevel.Level4, MakeHashSet(LocationType.DungeonBoss))))
    End Sub

    Public Overrides Function RollSpawnCount(level As DungeonLevel) As Long
        Select Case level
            Case DungeonLevel.Level4
                Return 1
            Case Else
                Return 0
        End Select
    End Function
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

    Public Overrides ReadOnly Property IsConsumed As Boolean
        Get
            Return False
        End Get
    End Property
End Class
