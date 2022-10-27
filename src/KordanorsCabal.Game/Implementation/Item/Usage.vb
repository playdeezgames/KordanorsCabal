Public Class Usage
    Inherits BaseThingie
    Implements IUsage
    Public Sub New(worldData As IWorldData, id As Long)
        MyBase.New(worldData, id)
    End Sub
    Public Shared Function FromId(worldData As IWorldData, id As Long?) As IUsage
        Return If(id.HasValue, New Usage(worldData, id.Value), Nothing)
    End Function
    Public Const PurifyEventId = 1L
    Public Const CanUseEventId = 2L
    Public Const UseEventId = 3L
    Public Const DecayEventId = 4L
    Public Const AddToInventoryEventId = 5L
    ReadOnly Property CanUse(character As ICharacter) As Boolean Implements IUsage.CanUse
        Get
            Dim eventName = WorldData.ItemEvent.Read(Id, CanUseEventId)
            If eventName IsNot Nothing Then
                Return WorldData.Events.Test(WorldData, eventName, character.Id)
            End If
            Return False
        End Get
    End Property
    ReadOnly Property IsConsumed As Boolean Implements IUsage.IsConsumed
        Get
            Return If(WorldData.ItemStatistic.Read(Id, StatisticTypeSingleUse), 0) > 0
        End Get
    End Property
    Friend Sub Use(character As ICharacter) Implements IUsage.Use
        Dim eventName = WorldData.ItemEvent.Read(Id, UseEventId)
        If eventName IsNot Nothing Then
            WorldData.Events.Perform(WorldData, eventName, character.Id, Id)
        End If
    End Sub
    Public Sub Decay() Implements IUsage.Decay
        Dim eventName = WorldData.ItemEvent.Read(Id, Game.Usage.DecayEventId)
        If eventName IsNot Nothing Then
            WorldData.Events.Perform(WorldData, eventName, Id)
        End If
    End Sub
    Public Sub Purify() Implements IUsage.Purify
        Dim eventName = WorldData.ItemEvent.Read(Id, Game.Usage.PurifyEventId)
        If eventName IsNot Nothing Then
            WorldData.Events.Perform(WorldData, eventName, Id)
        End If
    End Sub
End Class
