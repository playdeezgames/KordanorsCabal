Public Class CharacterQuest
    Inherits BaseThingie
    Implements ICharacterQuest

    Public Sub New(worldData As IWorldData, id As Long)
        MyBase.New(worldData, id)
    End Sub
    Shared Function FromId(worldData As IWorldData, id As Long?) As ICharacterQuest
        Return If(id.HasValue, New CharacterQuest(worldData, id.Value), Nothing)
    End Function
    Public Function CanAccept(quest As IQuestType) As Boolean Implements ICharacterQuest.CanAccept
        Return Not Has(quest) AndAlso quest.CanAccept(Character.FromId(WorldData, Id))
    End Function
    Public Sub Complete(quest As IQuestType) Implements ICharacterQuest.Complete
        quest.Complete(Character.FromId(WorldData, Id))
    End Sub
    Public Sub Accept(quest As IQuestType) Implements ICharacterQuest.Accept
        quest.Accept(Character.FromId(WorldData, Id))
    End Sub
    Public Function Has(quest As IQuestType) As Boolean Implements ICharacterQuest.Has
        Return WorldData.CharacterQuest.Read(Id, quest.Id)
    End Function
End Class
