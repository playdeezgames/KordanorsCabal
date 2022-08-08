Friend Class SpaceSordDescriptor
    Inherits ItemTypeDescriptor

    Public Overrides ReadOnly Property SpawnLocationTypes(level as DungeonLevel) As HashSet(Of LocationType)
        Get
            Return New HashSet(Of LocationType) From {LocationType.Moon}
        End Get
    End Property

    Sub New()
        MyBase.New("SpaceSord")
    End Sub

    Public Overrides ReadOnly Property Encumbrance As Single
        Get
            Return 5
        End Get
    End Property

    Public Overrides Function RollSpawnCount(level As DungeonLevel) As Long
        Return If(level = DungeonLevel.Moon, 1, 0)
    End Function

    Public Overrides ReadOnly Property EquipSlots As IEnumerable(Of EquipSlot)
        Get
            Return New List(Of EquipSlot) From {Game.EquipSlot.Weapon}
        End Get
    End Property

    Public Overrides ReadOnly Property AttackDice As Long
        Get
            Return 10
        End Get
    End Property

    Public Overrides ReadOnly Property MaximumDamage As Long?
        Get
            Return 5
        End Get
    End Property
    Public Overrides ReadOnly Property MaximumDurability As Long?
        Get
            Return 100
        End Get
    End Property
End Class
