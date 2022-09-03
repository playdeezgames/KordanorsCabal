Friend Class BeerDescriptor
    Inherits ItemTypeDescriptor

    Sub New()
        MyBase.New(
            StaticWorldData.World,
            ItemType.Beer,
            ,,,,,,,,
            5,
            MakeList(ShoppeType.InnKeeper),,,,
            "CanUseBeer",
            "UseBeer")
    End Sub
End Class
