Friend Class FoodDescriptor
    Inherits ItemTypeDescriptor

    Public Overrides ReadOnly Property SpawnLocationTypes(level As Long) As HashSet(Of LocationType)
        Get
            Return New HashSet(Of LocationType)
        End Get
    End Property

    Public Overrides ReadOnly Property Name As String
        Get
            Return "Food"
        End Get
    End Property
    Public Overrides ReadOnly Property CanUse As Boolean
        Get
            Return True
        End Get
    End Property

    Public Overrides Sub Use(character As Character)
        Dim healRoll = 1
        character.ChangeStatistic(CharacterStatisticType.Wounds, -healRoll)
        If character.Id = World.PlayerCharacter.Id Then
            World.PlayerCharacter.EnqueueMessage(
                    $"Food heals up to {healRoll} HP!",
                    $"You now have {character.CurrentHP} HP!")
        End If
    End Sub
End Class
