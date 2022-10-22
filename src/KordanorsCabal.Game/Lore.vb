Public Class Lore
    Inherits BaseThingie
    Implements ILore

    Public Sub New(worldData As IWorldData, id As Long)
        MyBase.New(worldData, id)
    End Sub

    Public ReadOnly Property Text As String Implements ILore.Text
        Get
            Return WorldData.Lore.ReadText(Id)
        End Get
    End Property

    Public ReadOnly Property ItemName As String Implements ILore.ItemName
        Get
            Return WorldData.Lore.ReadItemName(Id)
        End Get
    End Property

    Shared Function FromId(worldData As IWorldData, id As Long?) As ILore
        Return If(id.HasValue, New Lore(worldData, id.Value), Nothing)
    End Function
End Class
