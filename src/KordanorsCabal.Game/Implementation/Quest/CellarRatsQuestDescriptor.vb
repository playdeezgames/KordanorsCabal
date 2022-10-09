Friend Class CellarRatsQuestDescriptor
    Inherits QuestType

    Public Sub New(worldData As IWorldData)
        MyBase.New(worldData, OldQuestType.CellarRats)
    End Sub
    Public Overrides Sub Complete(worldData As IWorldData, character As ICharacter)
        If character.HasQuest(OldQuestType.CellarRats) AndAlso CanComplete(character) Then
            worldData.Events.Perform(worldData, "CharacterCompleteCellarRatsQuest", character.Id)
            Return
        End If
        character.EnqueueMessage("You cannot complete this quest at this time.")
    End Sub

    Public Overrides Sub Accept(worldData As IWorldData, character As ICharacter)
        If CanAccept(character) Then
            worldData.Events.Perform(worldData, "CharacterAcceptCellarRatsQuest", character.Id)
            Return
        End If
        character.EnqueueMessage("You cannot accept this quest at this time.")
    End Sub

    Public Overrides Function CanAccept(character As ICharacter) As Boolean
        Return WorldData.Events.Test(WorldData, "CharacterCanAcceptCellarRatsQuest", character.Id)
    End Function

    Public Overrides Function CanComplete(character As ICharacter) As Boolean
        Return WorldData.Events.Test(WorldData, "CharacterCanCompleteCellarRatsQuest", character.Id)
    End Function
End Class
