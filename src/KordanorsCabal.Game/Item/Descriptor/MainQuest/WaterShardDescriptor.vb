Friend Class WaterShardDescriptor
    Inherits ItemTypeDescriptor

    Sub New()
        MyBase.New(
            StaticWorldData.World,
            ItemType.WaterShard,
            ,,,,,,,,,,,,,
            "CanUseWaterShard",
            "UseWaterShard")
    End Sub
End Class
