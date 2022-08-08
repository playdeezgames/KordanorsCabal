Friend Class RingOfHPDescriptor
    Inherits ItemTypeDescriptor

    Sub New()
        MyBase.New("Ring Of HP",,,, MakeList(EquipSlot.LeftHand, EquipSlot.RightHand))
    End Sub

    Public Overrides Function EquippedBuff(statisticType As CharacterStatisticType) As Long?
        Return If(statisticType = CharacterStatisticType.HP, 1, Nothing)
    End Function
End Class
