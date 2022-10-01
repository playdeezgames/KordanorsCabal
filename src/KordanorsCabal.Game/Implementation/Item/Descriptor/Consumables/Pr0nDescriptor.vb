Friend Class Pr0nDescriptor
    Inherits ItemType

    Sub New()
        MyBase.New(
            StaticWorldData.World,
            OldItemType.Pr0n,,,,
            10,
            MakeList(ShoppeType.BlackMarket),,,,
            "CanUsePr0n",
            "UsePr0n")
    End Sub
End Class
