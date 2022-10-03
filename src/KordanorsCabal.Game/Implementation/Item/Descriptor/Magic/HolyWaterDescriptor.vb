Friend Class HolyWaterDescriptor
    Inherits ItemType

    Sub New()
        MyBase.New(
            StaticWorldData.World,
            OldItemType.HolyWater,,
            "IsFightingUndead",
            "UseHolyWater")
    End Sub
End Class
