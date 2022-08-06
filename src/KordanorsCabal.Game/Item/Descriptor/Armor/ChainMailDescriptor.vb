Friend Class ChainMailDescriptor
    Inherits ItemTypeDescriptor

    Public Overrides ReadOnly Property SpawnLocationTypes(level As Long) As HashSet(Of LocationType)
        Get
            Select Case level
                Case 1
                    Return New HashSet(Of LocationType) From {LocationType.DungeonDeadEnd}
            End Select
            Return New HashSet(Of LocationType) From {LocationType.Dungeon, LocationType.DungeonDeadEnd}
        End Get
    End Property

    Public Overrides ReadOnly Property Name As String
        Get
            Return "Chainmail"
        End Get
    End Property

    Public Overrides Function RollSpawnCount(level As DungeonLevel) As Long
        Select Case level
            Case DungeonLevel.Level1
                Return RNG.RollDice("1d6")
            Case Else
                Return 0
        End Select
    End Function

    Public Overrides ReadOnly Property EquipSlots As IEnumerable(Of EquipSlot)
        Get
            Return New List(Of EquipSlot) From {Game.EquipSlot.Torso}
        End Get
    End Property

    Public Overrides ReadOnly Property Encumbrance As Single
        Get
            Return 20.0!
        End Get
    End Property

    Public Overrides ReadOnly Property MaximumDurability As Long?
        Get
            Return 25
        End Get
    End Property

    Public Overrides ReadOnly Property DefendDice As Long
        Get
            Return 2
        End Get
    End Property
End Class
