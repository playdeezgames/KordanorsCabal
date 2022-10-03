Friend Class EarthShardDescriptor
    Inherits ItemType
    Sub New()
        MyBase.New(
            StaticWorldData.World,
            OldItemType.EarthShard,,
            "CanUseEarthShard")
    End Sub
End Class
