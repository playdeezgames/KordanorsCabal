Friend Class WaterShardDescriptor
    Inherits ItemType

    Sub New()
        MyBase.New(
            StaticWorldData.World,
            OldItemType.WaterShard,
            ,,,,,,,,,,,,,
            "CanUseWaterShard",
            "UseWaterShard")
    End Sub
End Class
