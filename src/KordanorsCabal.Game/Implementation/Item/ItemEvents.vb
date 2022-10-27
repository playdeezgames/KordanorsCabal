Public Class ItemEvents
    Inherits BaseThingie
    Implements IItemEvents
    Public Sub New(worldData As IWorldData, id As Long)
        MyBase.New(worldData, id)
    End Sub
    Public Shared Function FromId(worldData As IWorldData, id As Long?) As IItemEvents
        Return If(id.HasValue, New ItemEvents(worldData, id.Value), Nothing)
    End Function
    Public Const PurifyEventId = 1L
    Public Const CanUseEventId = 2L
    Public Const UseEventId = 3L
    Public Const DecayEventId = 4L
    Public Const AddToInventoryEventId = 5L
    ReadOnly Property CanUse(character As ICharacter) As Boolean Implements IItemEvents.CanUse
        Get
            Dim eventName = WorldData.ItemEvent.Read(Id, CanUseEventId)
            If eventName IsNot Nothing Then
                Return WorldData.Events.Test(WorldData, eventName, character.Id)
            End If
            Return False
        End Get
    End Property
    ReadOnly Property IsConsumed As Boolean Implements IItemEvents.IsConsumed
        Get
            Return If(WorldData.ItemStatistic.Read(Id, StatisticTypeSingleUse), 0) > 0
        End Get
    End Property
    Friend Sub Use(character As ICharacter) Implements IItemEvents.Use
        Dim eventName = WorldData.ItemEvent.Read(Id, UseEventId)
        If eventName IsNot Nothing Then
            WorldData.Events.Perform(WorldData, eventName, character.Id, Id)
        End If
    End Sub
    Public Sub Decay() Implements IItemEvents.Decay
        Dim eventName = WorldData.ItemEvent.Read(Id, Game.ItemEvents.DecayEventId)
        If eventName IsNot Nothing Then
            WorldData.Events.Perform(WorldData, eventName, Id)
        End If
    End Sub
    Public Sub Purify() Implements IItemEvents.Purify
        Dim eventName = WorldData.ItemEvent.Read(Id, Game.ItemEvents.PurifyEventId)
        If eventName IsNot Nothing Then
            WorldData.Events.Perform(WorldData, eventName, Id)
        End If
    End Sub
End Class
