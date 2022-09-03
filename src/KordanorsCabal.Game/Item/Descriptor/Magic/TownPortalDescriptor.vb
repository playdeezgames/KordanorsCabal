Friend Class TownPortalDescriptor
    Inherits ItemTypeDescriptor

    Sub New()
        MyBase.New(
            StaticWorldData.World,
            ItemType.TownPortal,,,,,,,,,
            50,
            MakeList(ShoppeType.BlackMage),,,,
            Function(character) character.Location.IsDungeon,
            "UseTownPortal")
    End Sub
End Class
