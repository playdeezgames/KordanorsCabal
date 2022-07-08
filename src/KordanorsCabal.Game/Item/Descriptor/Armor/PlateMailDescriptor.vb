Friend Class PlateMailDescriptor
    Inherits ItemTypeDescriptor
    Public Overrides ReadOnly Property SpawnLocationTypes(level As Long) As HashSet(Of LocationType)
        Get
            Select Case level
                Case 2
                    Return New HashSet(Of LocationType) From {LocationType.DungeonDeadEnd}
            End Select
            Return New HashSet(Of LocationType) From {LocationType.Dungeon, LocationType.DungeonDeadEnd}
        End Get
    End Property

    Public Overrides ReadOnly Property Name As String
        Get
            Return "Platemail"
        End Get
    End Property

    Public Overrides Function RollSpawnCount(level As Long) As Long
        Select Case level
            Case 2
                Return RNG.RollDice("1d6")
            Case Else
                Return 0
        End Select
    End Function

    Public Overrides ReadOnly Property EquipSlot As EquipSlot?
        Get
            Return Game.EquipSlot.Torso
        End Get
    End Property

    Public Overrides ReadOnly Property Encumbrance As Single
        Get
            Return 40.0!
        End Get
    End Property

    Public Overrides ReadOnly Property MaximumDurability As Long?
        Get
            Return 50
        End Get
    End Property

    Public Overrides ReadOnly Property DefendDice As Long
        Get
            Return 4
        End Get
    End Property
End Class
