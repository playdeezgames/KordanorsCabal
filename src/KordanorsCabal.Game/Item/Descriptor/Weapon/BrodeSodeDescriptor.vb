Friend Class BrodeSodeDescriptor
    Inherits ItemTypeDescriptor

    Public Overrides ReadOnly Property SpawnLocationTypes(level As Long) As HashSet(Of LocationType)
        Get
            Return New HashSet(Of LocationType) From {LocationType.Dungeon}
        End Get
    End Property

    Public Overrides ReadOnly Property Name As String
        Get
            Return "Dagger"
        End Get
    End Property

    Public Overrides ReadOnly Property Encumbrance As Single
        Get
            Return 10
        End Get
    End Property

    Public Overrides Function RollSpawnCount(level As Long) As Long
        Select Case level
            Case 2
                Return RNG.RollDice("2d6")
            Case 3
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
            Return 6
        End Get
    End Property

    Public Overrides ReadOnly Property MaximumDamage As Long?
        Get
            Return 3
        End Get
    End Property
    Public Overrides ReadOnly Property MaximumDurability As Long?
        Get
            Return 40
        End Get
    End Property
End Class
