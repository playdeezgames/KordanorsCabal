Friend Class EarthShardDescriptor
    Inherits ItemTypeDescriptor
    Sub New()
        MyBase.New(
            StaticWorldData.World,
            ItemType.EarthShard,,
            ,,,, ,
            ,,,,,,,
            "CanUseEarthShard",
            "UseEarthShard")
    End Sub
End Class
