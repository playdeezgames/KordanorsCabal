Friend Class BeerDescriptor
    Inherits ItemTypeDescriptor

    Public Overrides ReadOnly Property SpawnLocationTypes(level As Long) As HashSet(Of LocationType)
        Get
            Return New HashSet(Of LocationType)
        End Get
    End Property

    Public Overrides ReadOnly Property Name As String
        Get
            Return "Beer"
        End Get
    End Property

    Public Overrides ReadOnly Property Encumbrance As Single
        Get
            Return 2.0!
        End Get
    End Property

    Public Overrides ReadOnly Property CanUse(character As Character) As Boolean
        Get
            Return True
        End Get
    End Property

    Public Overrides Sub Use(character As Character)
        Dim lines As New List(Of String)
        lines.Add("You drink the beer, and suddenly feel braver!")
        character.CurrentMP = character.GetStatistic(CharacterStatisticType.MP).Value
        character.Drunkenness += 10
        character.Inventory.Add(Game.Item.Create(ItemType.Bottle))
        character.EnqueueMessage(lines.ToArray)
    End Sub
End Class
