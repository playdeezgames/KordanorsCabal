Friend Class ShortswordDescriptor
    Inherits ItemTypeDescriptor

    Public Overrides ReadOnly Property SpawnLocationTypes(level As Long) As HashSet(Of LocationType)
        Get
            Return New HashSet(Of LocationType) From {LocationType.Dungeon}
        End Get
    End Property

    Public Overrides ReadOnly Property Name As String
        Get
            Return "Shortsword"
        End Get
    End Property

    Public Overrides ReadOnly Property Encumbrance As Single
        Get
            Return 5
        End Get
    End Property

    Public Overrides Function RollSpawnCount(level As Long) As Long
        Select Case level
            Case 1
                Return RNG.RollDice("2d6")
            Case 2
                Return RNG.RollDice("1d6")
            Case Else
                Return 0
        End Select
    End Function

    Public Overrides ReadOnly Property EquipSlot As EquipSlot?
        Get
            Return Game.EquipSlot.Weapon
        End Get
    End Property

    Public Overrides ReadOnly Property AttackDice As Long
        Get
            Return 4
        End Get
    End Property

    Public Overrides ReadOnly Property MaximumDamage As Long?
        Get
            Return 2
        End Get
    End Property
    Public Overrides ReadOnly Property MaximumDurability As Long?
        Get
            Return 20
        End Get
    End Property
End Class
