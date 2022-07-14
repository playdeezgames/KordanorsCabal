Friend Class HolyBoltDescriptor
    Inherits SpellDescriptor

    Public Overrides ReadOnly Property Name As String
        Get
            Return "Holy Bolt"
        End Get
    End Property
    Public Overrides ReadOnly Property MaximumLevel As Long
        Get
            Return 1
        End Get
    End Property

    Public Overrides ReadOnly Property RequiredPower(level As Long) As Long
        Get
            Select Case level
                Case 0
                    Return 0
                Case 1
                    Return 1
                Case Else
                    Return Long.MaxValue
            End Select
        End Get
    End Property

    Public Overrides ReadOnly Property CanCast(character As Character) As Boolean
        Get
            Dim enemy = character.Location.Enemy(character)
            If enemy Is Nothing OrElse Not enemy.IsUndead Then
                Return False
            End If
            Return If(character.GetStatistic(CharacterStatisticType.Mana), 0) > 0
        End Get
    End Property

    Public Overrides Sub Cast(character As Character)
        If Not CanCast(character) Then
            character.EnqueueMessage($"You cannot cast {SpellType.HolyBolt.Name} now!")
            Return
        End If
        Dim enemy = character.Location.Enemy(character)
        Dim lines As New List(Of String)
        Dim sfx As Sfx? = Nothing
        lines.Add($"You cast {SpellType.HolyBolt.Name} on {enemy.Name}!")
        character.ChangeStatistic(CharacterStatisticType.Mana, -1)
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
End Class
