Friend Class AirShardDescriptor
    Inherits ItemType

    Sub New()
        MyBase.New(
            StaticWorldData.World,
            OldItemType.AirShard,,
            "CanUseAirShard")
    End Sub
End Class
