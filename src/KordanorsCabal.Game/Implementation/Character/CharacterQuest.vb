Public Class CharacterQuest
    Inherits BaseThingie
    Implements ICharacterQuest

    Public Sub New(worldData As IWorldData, id As Long)
        MyBase.New(worldData, id)
    End Sub
    Shared Function FromId(worldData As IWorldData, id As Long?) As ICharacterQuest
        Return If(id.HasValue, New CharacterQuest(worldData, id.Value), Nothing)
    End Function
    Public Function CanAcceptQuest(quest As IQuestType) As Boolean Implements ICharacterQuest.CanAcceptQuest
        Return Not HasQuest(quest) AndAlso quest.CanAccept(Character.FromId(WorldData, Id))
    End Function
    Public Sub CompleteQuest(quest As IQuestType) Implements ICharacterQuest.CompleteQuest
        quest.Complete(Character.FromId(WorldData, Id))
    End Sub
    Public Sub AcceptQuest(quest As IQuestType) Implements ICharacterQuest.AcceptQuest
        quest.Accept(Character.FromId(WorldData, Id))
    End Sub
    Public Function HasQuest(quest As IQuestType) As Boolean Implements ICharacterQuest.HasQuest
        Return WorldData.CharacterQuest.Read(Id, quest.Id)
    End Function
End Class
