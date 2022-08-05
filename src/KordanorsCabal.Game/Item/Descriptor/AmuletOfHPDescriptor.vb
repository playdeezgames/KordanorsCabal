Friend Class AmuletOfHPDescriptor
    Inherits ItemTypeDescriptor

    Public Overrides ReadOnly Property SpawnLocationTypes(level As Long) As HashSet(Of LocationType)
        Get
            Return New HashSet(Of LocationType) From {LocationType.DungeonBoss, LocationType.DungeonDeadEnd}
        End Get
    End Property

    Public Overrides Function RollSpawnCount(level As DungeonLevel) As Long
        Return If(level = DungeonLevel.Level1, 1, 0)
    End Function

    Public Overrides ReadOnly Property Name As String
        Get
            Return "Amulet of HP"
        End Get
    End Property

    Public Overrides ReadOnly Property EquipSlot As EquipSlot?
        Get
            Return Game.EquipSlot.Neck
        End Get
    End Property
End Class
