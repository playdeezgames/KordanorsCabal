Public Class EquipSlot
    ReadOnly Property Id As Long
    ReadOnly Property Name As String
        Get
            Return StaticWorldData.World.EquipSlot.ReadName(Id)
        End Get
    End Property
    Sub New(equipSlotId As Long)
        Me.Id = equipSlotId
    End Sub
    Sub New(name As String)
        Me.New(StaticWorldData.World.EquipSlot.ReadForName(name).Value)
    End Sub
    Public Shared Function FromName(name As String) As EquipSlot
        Return New EquipSlot(name)
    End Function
End Class
