Friend Class RingOfHPDescriptor
    Inherits ItemTypeDescriptor

    Sub New()
        MyBase.New(
            ItemType.RingOfHP,
            "Ring Of HP",,,, MakeList(EquipSlot.FromName(LeftHand), EquipSlot.FromName(RightHand)),
            New Dictionary(Of Long, Long) From {{6, 1}})
    End Sub
End Class
