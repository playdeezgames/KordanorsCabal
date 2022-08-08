Friend Class TrousersDescriptor
    Inherits ItemTypeDescriptor

    Sub New()
        MyBase.New("Trousers")
    End Sub

    Public Overrides ReadOnly Property EquipSlots As IEnumerable(Of EquipSlot)
        Get
            Return New List(Of EquipSlot) From {Game.EquipSlot.Legs}
        End Get
    End Property
End Class
