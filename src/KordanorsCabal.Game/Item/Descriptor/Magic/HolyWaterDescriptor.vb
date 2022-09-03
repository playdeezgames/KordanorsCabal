Friend Class HolyWaterDescriptor
    Inherits ItemTypeDescriptor

    Sub New()
        MyBase.New(
            StaticWorldData.World,
            ItemType.HolyWater,,,,,,,,,
            10,
            MakeList(ShoppeType.Healer),,,,
            "IsFightingUndead",
            "UseHolyWater")
    End Sub
End Class
