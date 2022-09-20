Friend Class EarthShardDescriptor
    Inherits ItemType
    Sub New()
        MyBase.New(
            StaticWorldData.World,
            OldItemType.EarthShard,,
            ,,, ,
            ,,,,,,,
            "CanUseEarthShard",
            "UseEarthShard")
    End Sub
End Class
