Friend Class RingOfHPDescriptor
    Inherits ItemTypeDescriptor

    Sub New()
        MyBase.New(
            ItemType.RingOfHP,
            "Ring Of HP",,,, MakeList(EquipSlot.FromId(LeftHand), EquipSlot.FromId(RightHand)),
            New Dictionary(Of Long, Long) From {{6, 1}})
    End Sub
End Class
