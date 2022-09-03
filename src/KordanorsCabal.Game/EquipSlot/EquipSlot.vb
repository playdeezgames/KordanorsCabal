Public Class EquipSlot
    Inherits BaseThingie
    ReadOnly Property Name As String
        Get
            Return WorldData.EquipSlot.ReadName(Id)
        End Get
    End Property
    Sub New(worldData As IWorldData, equipSlotId As Long)
        MyBase.New(worldData, equipSlotId)
    End Sub
    Sub New(worldData As IWorldData, name As String)
        Me.New(worldData, worldData.EquipSlot.ReadForName(name).Value)
    End Sub
    Public Shared Function FromId(worldData As IWorldData, equipSlotId As Long) As EquipSlot
        Return New EquipSlot(worldData, equipSlotId)
    End Function
End Class
