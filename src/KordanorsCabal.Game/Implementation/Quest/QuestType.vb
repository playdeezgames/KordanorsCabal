Public Class QuestType
    Inherits BaseThingie
    Implements IQuestType

    Public Sub New(worldData As IWorldData, id As Long)
        MyBase.New(worldData, id)
    End Sub
    Public Shared Function FromId(worldData As IWorldData, id As Long?) As IQuestType
        Return If(id.HasValue, New QuestType(worldData, id.Value), Nothing)
    End Function

    Overridable Function CanAccept(character As ICharacter) As Boolean
        Return WorldData.Events.Test(WorldData, WorldData.QuestType.ReadCanAcceptEventName(Id), character.Id)
    End Function
    Overridable Function CanComplete(character As ICharacter) As Boolean
        Return WorldData.Events.Test(WorldData, WorldData.QuestType.ReadCanCompleteEventName(Id), character.Id)
    End Function
    Overridable Sub Complete(worldData As IWorldData, character As ICharacter)
        If character.HasQuest(CType(Id, OldQuestType)) AndAlso CanComplete(character) Then
            worldData.Events.Perform(worldData, worldData.QuestType.ReadCompleteEventName(Id), character.Id)
            Return
        End If
        character.EnqueueMessage("You cannot complete this quest at this time.")
    End Sub
    Overridable Sub Accept(worldData As IWorldData, character As ICharacter)
        If CanAccept(character) Then
            worldData.Events.Perform(worldData, worldData.QuestType.ReadAcceptEventName(Id), character.Id)
            Return
        End If
        character.EnqueueMessage("You cannot accept this quest at this time.")
    End Sub
End Class
