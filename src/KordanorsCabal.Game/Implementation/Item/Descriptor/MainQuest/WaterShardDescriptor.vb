Friend Class WaterShardDescriptor
    Inherits ItemType

    Sub New()
        MyBase.New(
            StaticWorldData.World,
            OldItemType.WaterShard)
    End Sub
End Class
