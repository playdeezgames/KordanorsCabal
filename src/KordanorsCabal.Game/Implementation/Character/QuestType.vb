﻿Public Class QuestType
    Inherits BaseThingie
    Implements IQuestType

    Public Sub New(worldData As IWorldData, id As Long)
        MyBase.New(worldData, id)
    End Sub
    Public Shared Function FromId(worldData As IWorldData, id As Long?) As IQuestType
        Return If(id.HasValue, New QuestType(worldData, id.Value), Nothing)
    End Function

    Function CanAccept(character As ICharacter) As Boolean Implements IQuestType.CanAccept
        Return WorldData.Events.Test(WorldData, WorldData.QuestType.ReadCanAcceptEventName(Id), character.Id)
    End Function
    Function CanComplete(character As ICharacter) As Boolean Implements IQuestType.CanComplete
        Return WorldData.Events.Test(WorldData, WorldData.QuestType.ReadCanCompleteEventName(Id), character.Id)
    End Function
    Sub Complete(character As ICharacter) Implements IQuestType.Complete
        If character.Quest.Has(Me) AndAlso CanComplete(character) Then
            WorldData.Events.Perform(WorldData, WorldData.QuestType.ReadCompleteEventName(Id), character.Id)
            Return
        End If
        character.EnqueueMessage(Nothing, "You cannot complete this quest at this time.")
    End Sub
    Sub Accept(character As ICharacter) Implements IQuestType.Accept
        If CanAccept(character) Then
            WorldData.Events.Perform(WorldData, WorldData.QuestType.ReadAcceptEventName(Id), character.Id)
            Return
        End If
        character.EnqueueMessage(Nothing, "You cannot accept this quest at this time.")
    End Sub
End Class
