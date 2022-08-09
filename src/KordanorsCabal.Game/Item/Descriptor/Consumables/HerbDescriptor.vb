Friend Class HerbDescriptor
    Inherits ItemTypeDescriptor

    Sub New()
        MyBase.New(
            """Herb""",,,,,,,,,,,,,,,,,,
            Function(character) character.Inventory.ItemsOfType(ItemType.Bong).Any)
    End Sub
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
