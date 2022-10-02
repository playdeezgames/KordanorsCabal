Friend Class BeerDescriptor
    Inherits ItemType

    Sub New()
        MyBase.New(
            StaticWorldData.World,
            OldItemType.Beer,,
            MakeList(ShoppeType.InnKeeper),,,
            "CanUseBeer",
            "UseBeer")
    End Sub
End Class
