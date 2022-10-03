Friend Class HolyWaterDescriptor
    Inherits ItemType

    Sub New()
        MyBase.New(
            StaticWorldData.World,
            OldItemType.HolyWater,,
            "IsFightingUndead")
    End Sub
End Class
