Friend Class RingOfHPDescriptor
    Inherits ItemTypeDescriptor

    Sub New()
        MyBase.New(
            ItemType.RingOfHP,
            "Ring Of HP",,,, MakeList(EquipSlot.FromName(LeftHand), EquipSlot.FromName(RightHand)),
            New Dictionary(Of OldCharacterStatisticType, Long) From {{OldCharacterStatisticType.HP, 1}})
    End Sub
End Class
