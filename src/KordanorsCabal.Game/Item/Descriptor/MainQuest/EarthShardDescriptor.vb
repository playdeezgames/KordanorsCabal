Friend Class EarthShardDescriptor
    Inherits ItemTypeDescriptor
    Sub New()
        MyBase.New("Earth Shard",,
            MakeDictionary(
                (DungeonLevel.Level2, MakeHashSet(LocationType.DungeonBoss))))
    End Sub

    Public Overrides Function RollSpawnCount(level As DungeonLevel) As Long
        Select Case level
            Case DungeonLevel.Level2
                Return 1
            Case Else
                Return 0
        End Select
    End Function
    Public Overrides ReadOnly Property CanUse(character As Character) As Boolean
        Get
            Dim location = character.Location
            Return location.IsDungeon AndAlso location.Enemies(character).Any AndAlso character.CurrentMana > 0
        End Get
    End Property

    Public Overrides Sub Use(character As Character)
        If Not CanUse(character) Then
            character.EnqueueMessage($"You cannot use {ItemType.EarthShard.Name} right now!")
            Return
        End If
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
    End Sub

    Public Overrides ReadOnly Property IsConsumed As Boolean
        Get
            Return False
        End Get
    End Property
End Class
