Friend Class RingOfHPDescriptor
    Inherits ItemTypeDescriptor

    Sub New()
        MyBase.New("Ring Of HP")
    End Sub

    Public Overrides ReadOnly Property EquipSlots As IEnumerable(Of EquipSlot)
        Get
            Return New List(Of EquipSlot) From {EquipSlot.LeftHand, EquipSlot.RightHand}
        End Get
    End Property

    Public Overrides Function EquippedBuff(statisticType As CharacterStatisticType) As Long?
        Return If(statisticType = CharacterStatisticType.HP, 1, Nothing)
    End Function
End Class
