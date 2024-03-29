﻿Public Class CharacterQuest
    Inherits SubcharacterBase
    Implements ICharacterQuest

    Public Sub New(worldData As IWorldData, character As ICharacter)
        MyBase.New(worldData, character)
    End Sub
    Shared Function FromCharacter(worldData As IWorldData, character As ICharacter) As ICharacterQuest
        Return If(character IsNot Nothing, New CharacterQuest(worldData, character), Nothing)
    End Function
    Public Function CanAccept(quest As IQuestType) As Boolean Implements ICharacterQuest.CanAccept
        Return Not Has(quest) AndAlso quest.CanAccept(character)
    End Function
    Public Sub Complete(quest As IQuestType) Implements ICharacterQuest.Complete
        quest.Complete(character)
    End Sub
    Public Sub Accept(quest As IQuestType) Implements ICharacterQuest.Accept
        quest.Accept(character)
    End Sub
    Public Function Has(quest As IQuestType) As Boolean Implements ICharacterQuest.Has
        Return WorldData.CharacterQuest.Read(Id, quest.Id)
    End Function
End Class
