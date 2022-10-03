Friend Class FireShardDescriptor
    Inherits ItemType
    Sub New()
        MyBase.New(
            StaticWorldData.World,
            OldItemType.FireShard,,
            "CanUseFireShard")
    End Sub
End Class
