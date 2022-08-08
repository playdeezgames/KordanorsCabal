Friend Class HelmetDescriptor
    Inherits ItemTypeDescriptor

    Public Overrides ReadOnly Property SpawnLocationTypes(level as DungeonLevel) As HashSet(Of LocationType)
        Get
            Return New HashSet(Of LocationType) From {LocationType.Dungeon, LocationType.DungeonDeadEnd}
        End Get
    End Property

    Sub New()
        MyBase.New("Helmet")
    End Sub

    Public Overrides Function RollSpawnCount(level As DungeonLevel) As Long
        Select Case level
            Case DungeonLevel.Level1
                Return RNG.RollDice("3d6")
            Case Else
                Return 0
        End Select
    End Function

    Public Overrides ReadOnly Property EquipSlots As IEnumerable(Of EquipSlot)
        Get
            Return New List(Of EquipSlot) From {Game.EquipSlot.Head}
        End Get
    End Property

    Public Overrides ReadOnly Property Encumbrance As Single
        Get
            Return 2.0!
        End Get
    End Property

    Public Overrides ReadOnly Property MaximumDurability As Long?
        Get
            Return 10
        End Get
    End Property

    Public Overrides ReadOnly Property DefendDice As Long
        Get
            Return 2
        End Get
    End Property
End Class
