Public Class Armor
    Inherits BaseThingie
    Implements IArmor
    Public Sub New(worldData As IWorldData, id As Long)
        MyBase.New(worldData, id)
    End Sub
    Public Shared Function FromId(worldData As IWorldData, id As Long?) As IArmor
        Return If(id.HasValue, New Armor(worldData, id.Value), Nothing)
    End Function
    ReadOnly Property IsArmor() As Boolean Implements IArmor.IsArmor
        Get
            Return DefendDice > 0
        End Get
    End Property
    ReadOnly Property DefendDice As Long Implements IArmor.DefendDice
        Get
            Return If(WorldData.ItemStatistic.Read(Id, StatisticTypeDefendDice), 0)
        End Get
    End Property
End Class
