Friend Class DaggerDescriptor
    Inherits ItemTypeDescriptor

    Sub New()
        MyBase.New("Dagger",
                   1,
            MakeDictionary(
                (DungeonLevel.Level1, MakeHashSet(LocationType.Dungeon)),
                (DungeonLevel.Level2, MakeHashSet(LocationType.Dungeon)),
                (DungeonLevel.Level3, MakeHashSet(LocationType.Dungeon)),
                (DungeonLevel.Level4, MakeHashSet(LocationType.Dungeon)),
                (DungeonLevel.Level5, MakeHashSet(LocationType.Dungeon))),
                1,
                MakeList(ShoppeType.Blacksmith))
    End Sub

    Public Overrides Function RollSpawnCount(level As DungeonLevel) As Long
        Select Case level
            Case DungeonLevel.Level1
                Return RNG.RollDice("4d6")
            Case DungeonLevel.Level2
                Return RNG.RollDice("2d6")
            Case Else
                Return 0
        End Select
    End Function

    Public Overrides ReadOnly Property EquipSlots As IEnumerable(Of EquipSlot)
        Get
            Return New List(Of EquipSlot) From {Game.EquipSlot.Weapon}
        End Get
    End Property

    Public Overrides ReadOnly Property AttackDice As Long
        Get
            Return 2
        End Get
    End Property

    Public Overrides ReadOnly Property MaximumDamage As Long?
        Get
            Return 1
        End Get
    End Property
    Public Overrides ReadOnly Property MaximumDurability As Long?
        Get
            Return 10
        End Get
    End Property
End Class
