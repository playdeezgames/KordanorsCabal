Public Class Item
    ReadOnly Property Id As Long
    Sub New(itemId As Long)
        Id = itemId
    End Sub
    Shared Function FromId(itemId As Long) As Item
        Return New Item(itemId)
    End Function
    Shared Function Create(itemType As ItemType) As Item
        Return FromId(ItemData.Create(itemType))
    End Function

    Public ReadOnly Property ItemType As ItemType
        Get
            Return CType(ItemData.ReadItemType(Id).Value, ItemType)
        End Get
    End Property

    Public ReadOnly Property Name As String
        Get
            Return ItemType.Name
        End Get
    End Property

    ReadOnly Property CanUse As Boolean
        Get
            Return ItemType.CanUse
        End Get
    End Property

    ReadOnly Property CanEquip As Boolean
        Get
            Return ItemType.EquipSlot.HasValue
        End Get
    End Property

    Friend Sub Destroy()
        ItemData.Clear(Id)
    End Sub

    Friend Function Encumbrance() As Single
        Return ItemType.Encumbrance
    End Function

    Friend Sub Use(character As Character)
        ItemType.Use(character)
    End Sub

    Friend ReadOnly Property EquipSlot() As EquipSlot?
        Get
            Return ItemType.EquipSlot
        End Get
    End Property

    Friend ReadOnly Property MaximumDamage As Long?
        Get
            Return ItemType.MaximumDamage
        End Get
    End Property

    Friend ReadOnly Property AttackDice As Long
        Get
            Return ItemType.AttackDice
        End Get
    End Property

    Friend ReadOnly Property MaximumDurability As Long?
        Get
            Return ItemType.MaximumDurability
        End Get
    End Property

    Friend Sub ReduceDurability(amount As Long)
        If MaximumDurability.HasValue Then
            ChangeStatistic(ItemStatisticType.Wear, amount)
        End If
    End Sub

    Private Sub ChangeStatistic(statisticType As ItemStatisticType, delta As Long)
        SetStatistic(statisticType, GetStatistic(statisticType) + delta)
    End Sub

    Private Function GetStatistic(statisticType As ItemStatisticType) As Long
        Return If(ItemStatisticData.Read(Id, statisticType), statisticType.DefaultValue)
    End Function

    Private Sub SetStatistic(statisticType As ItemStatisticType, value As Long)
        ItemStatisticData.Write(Id, statisticType, value)
    End Sub
End Class
