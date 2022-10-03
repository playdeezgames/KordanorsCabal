Friend Class HolyWaterDescriptor
    Inherits ItemType

    Sub New()
        MyBase.New(
            StaticWorldData.World,
            OldItemType.HolyWater,
            MakeList(ShoppeType.Healer),,,
            "IsFightingUndead",
            "UseHolyWater")
    End Sub
End Class
