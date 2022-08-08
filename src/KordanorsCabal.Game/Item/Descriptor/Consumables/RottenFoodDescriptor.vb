Friend Class RottenFoodDescriptor
    Inherits ItemTypeDescriptor

    Public Overrides ReadOnly Property SpawnLocationTypes(level As Long) As HashSet(Of LocationType)
        Get
            Return New HashSet(Of LocationType)
        End Get
    End Property

    Sub New()
        MyBase.New("Food")
    End Sub
    Public Overrides ReadOnly Property CanUse(character As Character) As Boolean
        Get
            Return True
        End Get
    End Property

    Public Overrides Sub Use(character As Character)
        If RNG.RollDice("1d2") = 1 Then
            UseRotten(character)
        Else
            UseFresh(character)
        End If
    End Sub

    Private Sub UseRotten(character As Character)
        character.Hunger \= 2
        character.FoodPoisoning = 10
        character.EnqueueMessage(
                $"Food was rotten!",
                $"You got food poisoning!")
    End Sub

    Private Shared Sub UseFresh(character As Character)
        Dim healRoll = 1
        character.ChangeStatistic(CharacterStatisticType.Wounds, -healRoll)
        character.Hunger = 0
        character.EnqueueMessage(
                $"Food heals up to {healRoll} HP!",
                $"You now have {character.CurrentHP} HP!")
    End Sub

    Public Overrides Sub Purify(item As Item)
        ItemData.WriteItemType(item.Id, ItemType.Food)
    End Sub
End Class
