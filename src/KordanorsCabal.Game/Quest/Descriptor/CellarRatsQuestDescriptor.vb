Friend Class CellarRatsQuestDescriptor
    Inherits QuestDescriptor

    Public Overrides ReadOnly Property Name As String
        Get
            Return "Cellar Rats"
        End Get
    End Property

    Public Overrides Sub Complete(character As Character)
        If character.HasQuest(Quest.CellarRats) AndAlso CanComplete(character) Then
            character.EnqueueMessage("You complete the quest!")
            Dim ratTails = character.Inventory.ItemsOfType(ItemType.RatTail).Take(10)
            For Each ratTail In ratTails
                character.Money += 1
                ratTail.Destroy()
            Next
            StaticWorldData.World.CharacterQuest.Clear(character.Id, Quest.CellarRats)
            StaticWorldData.World.CharacterQuestCompletion.Write(
                character.Id,
                Quest.CellarRats,
                If(StaticWorldData.World.CharacterQuestCompletion.Read(character.Id, Quest.CellarRats), 0) + 1)
            Return
        End If
        character.EnqueueMessage("You cannot complete this quest at this time.")
    End Sub

    Public Overrides Sub Accept(character As Character)
        If CanAccept(character) Then
            character.EnqueueMessage("You accept the quest!")
            StaticWorldData.World.CharacterQuest.Write(character.Id, Quest.CellarRats)
            Dim ratCount = If(StaticWorldData.World.CharacterQuestCompletion.Read(character.Id, Quest.CellarRats), 0) + 1
            Dim location = Game.Location.FromLocationType(LocationType.FromId(Cellar)).Single
            While ratCount > 0
                Game.Character.Create(OldCharacterType.Rat.ToNew, location)
                ratCount -= 1
            End While
            Return
        End If
        character.EnqueueMessage("You cannot accept this quest at this time.")
    End Sub

    Public Overrides Function CanAccept(character As Character) As Boolean
        Return Not character.HasQuest(Quest.CellarRats)
    End Function

    Public Overrides Function CanComplete(character As Character) As Boolean
        Return character.Inventory.ItemsOfType(ItemType.RatTail).Count >= 1
    End Function
End Class
