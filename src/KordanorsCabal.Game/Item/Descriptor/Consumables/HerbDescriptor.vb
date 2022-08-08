Friend Class HerbDescriptor
    Inherits ItemTypeDescriptor

    Sub New()
        MyBase.New("""Herb""")
    End Sub

    Public Overrides ReadOnly Property CanUse(character As Character) As Boolean
        Get
            Return character.Inventory.ItemsOfType(ItemType.Bong).Any
        End Get
    End Property

    Public Overrides Sub Use(character As Character)
        If Not CanUse(character) Then
            character.EnqueueMessage($"You need a {ItemType.Bong.Name} for that!")
            Return
        End If
        Dim delta = character.MaximumMana - character.CurrentMana
        character.CurrentMana = character.MaximumMana
        character.Highness += 10
        character.EnqueueMessage($"You use yer {ItemType.Bong.Name} to smoke yer {ItemType.Herb.Name}.", $"You gain {delta} {CharacterStatisticType.Mana}.")
    End Sub
End Class
