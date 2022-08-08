Friend Class SpaceSordDescriptor
    Inherits ItemTypeDescriptor

    Sub New()
        MyBase.New("SpaceSord",
                   5,
            MakeDictionary(
                (DungeonLevel.Moon, MakeHashSet(LocationType.Moon))))
    End Sub

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
