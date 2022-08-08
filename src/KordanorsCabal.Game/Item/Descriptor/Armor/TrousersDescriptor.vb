Friend Class TrousersDescriptor
    Inherits ItemTypeDescriptor

    Public Overrides ReadOnly Property SpawnLocationTypes(level as DungeonLevel) As HashSet(Of LocationType)
        Get
            Return New HashSet(Of LocationType)
        End Get
    End Property

    Sub New()
        MyBase.New("Trousers")
    End Sub

    Public Overrides ReadOnly Property EquipSlots As IEnumerable(Of EquipSlot)
        Get
            Return New List(Of EquipSlot) From {Game.EquipSlot.Legs}
        End Get
    End Property
End Class
