Friend Class WaterShardDescriptor
    Inherits ItemType

    Sub New()
        MyBase.New(
            StaticWorldData.World,
            OldItemType.WaterShard,,
            "CanUseWaterShard")
    End Sub
End Class
