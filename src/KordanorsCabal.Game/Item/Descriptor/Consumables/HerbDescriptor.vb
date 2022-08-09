Friend Class HerbDescriptor
    Inherits ItemTypeDescriptor

    Sub New()
        MyBase.New(
            ItemType.Herb,
            """Herb""",,,,,,,,,,,,,
            5,
            MakeList(ShoppeType.BlackMage),,,,
            Function(character) character.Inventory.ItemsOfType(ItemType.Bong).Any,
            Sub(character)
                Dim delta = character.MaximumMana - character.CurrentMana
                character.CurrentMana = character.MaximumMana
                character.Highness += 10
                character.EnqueueMessage($"You use yer {ItemType.Bong.Name} to smoke yer {ItemType.Herb.Name}.", $"You gain {delta} {CharacterStatisticType.Mana}.")
            End Sub)
    End Sub
End Class
