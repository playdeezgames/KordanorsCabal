Friend Class Pr0nDescriptor
    Inherits ItemTypeDescriptor

    Sub New()
        MyBase.New(
            StaticWorldData.World,
            ItemType.Pr0n,,,,,,,,,
            10,
            MakeList(ShoppeType.BlackMarket),,,,
            "CanUsePr0n",
            "UsePr0n")
    End Sub
End Class
