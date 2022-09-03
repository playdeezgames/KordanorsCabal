Friend Class HerbDescriptor
    Inherits ItemTypeDescriptor

    Sub New()
        MyBase.New(
            StaticWorldData.World,
            ItemType.Herb,,,,,,,,,
            5,
            MakeList(ShoppeType.BlackMage),,,,
            Function(character) character.Inventory.ItemsOfType(ItemType.Bong).Any,
            "UseHerb")
    End Sub
End Class
