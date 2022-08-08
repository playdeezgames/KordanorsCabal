Friend Class AmuletOfHPDescriptor
    Inherits ItemTypeDescriptor

    Public Overrides ReadOnly Property SpawnLocationTypes(level As DungeonLevel) As HashSet(Of LocationType)
        Get
            Return New HashSet(Of LocationType) From {LocationType.DungeonBoss, LocationType.DungeonDeadEnd}
        End Get
    End Property

    Public Overrides Function RollSpawnCount(level As DungeonLevel) As Long
        Return If(level = DungeonLevel.Level1, 1, 0)
    End Function

    Sub New()
        MyBase.New("Amulet of HP")
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
