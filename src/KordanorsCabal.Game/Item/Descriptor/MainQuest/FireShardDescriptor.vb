Friend Class FireShardDescriptor
    Inherits ItemTypeDescriptor
    Sub New()
        MyBase.New(
            StaticWorldData.World,
            ItemType.FireShard,,,,,,,,,,,,,,
            "CanUseFireShard",
            "UseFireShard")
    End Sub
End Class
