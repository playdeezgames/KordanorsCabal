Friend Class CellarRatsQuestDescriptor
    Inherits QuestType

    Public Sub New(worldData As IWorldData)
        MyBase.New(worldData, OldQuestType.CellarRats)
    End Sub

    Public Overrides ReadOnly Property Name As String
        Get
            Return "Cellar Rats"
        End Get
    End Property

    Public Overrides Sub Complete(worldData As IWorldData, character As ICharacter)
        If character.HasQuest(OldQuestType.CellarRats) AndAlso CanComplete(character) Then
            character.EnqueueMessage("You complete the quest!")
            Dim ratTails = character.Inventory.ItemsOfType(ItemType.FromId(StaticWorldData.World, 21L)).Take(10)
            For Each ratTail In ratTails
                character.Money += 1
                ratTail.Destroy()
            Next
            StaticWorldData.World.CharacterQuest.Clear(character.Id, OldQuestType.CellarRats)
            StaticWorldData.World.CharacterQuestCompletion.Write(
                character.Id,
                OldQuestType.CellarRats,
                If(StaticWorldData.World.CharacterQuestCompletion.Read(character.Id, OldQuestType.CellarRats), 0) + 1)
            Return
        End If
        character.EnqueueMessage("You cannot complete this quest at this time.")
    End Sub

    Public Overrides Sub Accept(worldData As IWorldData, character As ICharacter)
        If CanAccept(character) Then
            character.EnqueueMessage("You accept the quest!")
            worldData.CharacterQuest.Write(character.Id, OldQuestType.CellarRats)
            Dim ratCount = If(worldData.CharacterQuestCompletion.Read(character.Id, OldQuestType.CellarRats), 0) + 1
            Dim location = Game.Location.FromLocationType(worldData, LocationType.FromId(worldData, 7L)).Single
            Dim initialStatistics = CharacterType.FromId(worldData, 13).Spawning.InitialStatistics()
            While ratCount > 0
                Game.Character.Create(worldData, CharacterType.FromId(worldData, 13), location, initialStatistics)
                ratCount -= 1
            End While
            Return
        End If
        character.EnqueueMessage("You cannot accept this quest at this time.")
    End Sub

    Public Overrides Function CanAccept(character As ICharacter) As Boolean
        Return Not character.HasQuest(OldQuestType.CellarRats)
    End Function

    Public Overrides Function CanComplete(character As ICharacter) As Boolean
        Return character.Inventory.ItemsOfType(ItemType.FromId(StaticWorldData.World, 21L)).Count >= 1
    End Function
End Class
