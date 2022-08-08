Friend Class AmuletOfHPDescriptor
    Inherits ItemTypeDescriptor

    Sub New()
        MyBase.New(
            "Amulet of HP", ,
            MakeDictionary(
                (DungeonLevel.Level1, MakeHashSet(LocationType.DungeonDeadEnd, LocationType.DungeonBoss)),
                (DungeonLevel.Level2, MakeHashSet(LocationType.DungeonDeadEnd, LocationType.DungeonBoss)),
                (DungeonLevel.Level3, MakeHashSet(LocationType.DungeonDeadEnd, LocationType.DungeonBoss)),
                (DungeonLevel.Level4, MakeHashSet(LocationType.DungeonDeadEnd, LocationType.DungeonBoss)),
                (DungeonLevel.Level5, MakeHashSet(LocationType.DungeonDeadEnd, LocationType.DungeonBoss))),
            MakeDictionary((DungeonLevel.Level1, "1d1")))
    End Sub

    Public Overrides ReadOnly Property EquipSlots As IEnumerable(Of EquipSlot)
        Get
            Return New List(Of EquipSlot) From {Game.EquipSlot.Neck}
        End Get
    End Property

    Public Overrides Function EquippedBuff(statisticType As CharacterStatisticType) As Long?
        Return If(statisticType = CharacterStatisticType.HP, 1, Nothing)
    End Function
End Class
