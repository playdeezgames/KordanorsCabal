Friend Class HolyWaterDescriptor
    Inherits ItemType

    Sub New()
        MyBase.New(
            StaticWorldData.World,
            OldItemType.HolyWater,,
            10,
            MakeList(ShoppeType.Healer),,,,
            "IsFightingUndead",
            "UseHolyWater")
    End Sub
End Class
