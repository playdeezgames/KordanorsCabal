Public Class Repair
    Inherits BaseThingie
    Implements IRepair

    Public Sub New(worldData As IWorldData, id As Long)
        MyBase.New(worldData, id)
    End Sub

    Public Shared Function FromId(worldData As IWorldData, id As Long?) As IRepair
        Return If(id.HasValue, New Repair(worldData, id.Value), Nothing)
    End Function
    Public ReadOnly Property NeedsRepair As Boolean Implements IRepair.NeedsRepair
        Get
            Dim durability = Game.Durability.FromId(WorldData, Id)
            Return durability.Maximum.HasValue AndAlso durability.Current.Value < durability.Maximum.Value
        End Get
    End Property
End Class
