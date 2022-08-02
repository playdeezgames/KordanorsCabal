Friend Class FireShardDescriptor
    Inherits ItemTypeDescriptor
    Public Overrides ReadOnly Property SpawnLocationTypes(level As Long) As HashSet(Of LocationType)
        Get
            Return New HashSet(Of LocationType) From {LocationType.DungeonBoss}
        End Get
    End Property

    Public Overrides ReadOnly Property Name As String
        Get
            Return "Fire Shard"
        End Get
    End Property

    Public Overrides Function RollSpawnCount(level As DungeonLevel) As Long
        Select Case level
            Case DungeonLevel.Level3
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
            character.EnqueueMessage($"You cannot use {ItemType.FireShard.Name} right now!")
            Return
        End If
        character.DoFatigue(1)
        Dim enemy = character.Location.Enemy(character)
        Dim lines As New List(Of String)
        Dim sfx As Sfx? = Nothing
        lines.Add($"You use {ItemType.FireShard.Name} on {enemy.Name}!")
        Dim damage As Long = character.RollSpellDice(SpellType.HolyBolt)
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
