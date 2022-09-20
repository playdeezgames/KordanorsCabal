Friend Class BeerDescriptor
    Inherits ItemType

    Sub New()
        MyBase.New(
            StaticWorldData.World,
            OldItemType.Beer,
            ,,,,,,,
            5,
            MakeList(ShoppeType.InnKeeper),,,,
            "CanUseBeer",
            "UseBeer")
    End Sub
End Class
