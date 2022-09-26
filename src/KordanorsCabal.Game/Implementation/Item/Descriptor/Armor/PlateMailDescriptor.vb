Friend Class PlateMailDescriptor
    Inherits ItemType
    Sub New()
        MyBase.New(
            StaticWorldData.World,
            OldItemType.PlateMail,,,
            4,
            50,
            50,
            MakeList(ShoppeType.Blacksmith),
            250,
            MakeList(ShoppeType.Blacksmith),
            100,
            MakeList(ShoppeType.Blacksmith))
    End Sub
End Class
