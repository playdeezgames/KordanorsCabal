Friend Class RingOfHPDescriptor
    Inherits ItemTypeDescriptor

    Sub New()
        MyBase.New(
            ItemType.RingOfHP,
            "Ring Of HP",,,, MakeList(EquipSlot.LeftHand.ToDescriptor, EquipSlot.RightHand.ToDescriptor),
            New Dictionary(Of CharacterStatisticType, Long) From {{CharacterStatisticType.HP, 1}})
    End Sub
End Class
