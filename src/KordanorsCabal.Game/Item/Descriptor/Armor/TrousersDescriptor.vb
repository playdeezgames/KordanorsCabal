Friend Class TrousersDescriptor
    Inherits ItemTypeDescriptor

    Sub New()
        MyBase.New("Trousers",,,, MakeList(EquipSlot.Legs))
    End Sub

End Class
