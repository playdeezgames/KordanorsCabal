Friend Class AirShardDescriptor
    Inherits ItemTypeDescriptor

    Public Overrides ReadOnly Property SpawnLocationTypes(level As Long) As HashSet(Of LocationType)
        Get
            Return New HashSet(Of LocationType) From {LocationType.DungeonBoss}
        End Get
    End Property

    Sub New()
        MyBase.New("Air Shard")
    End Sub

    Public Overrides Function RollSpawnCount(level As DungeonLevel) As Long
        Select Case level
            Case DungeonLevel.Level1
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
            character.EnqueueMessage($"You cannot use {ItemType.AirShard.Name} right now!")
            Return
        End If
        character.CurrentMana -= 1
        Dim level = character.Location.DungeonLevel
        Dim locations = Location.FromLocationType(LocationType.Dungeon).Where(Function(x) x.DungeonLevel = level)
        character.Location = RNG.FromEnumerable(locations)
        character.EnqueueMessage($"You use the {ItemType.AirShard.Name} and suddenly find yerself somewhere else!")
    End Sub

    Public Overrides ReadOnly Property IsConsumed As Boolean
        Get
            Return False
        End Get
    End Property
End Class
