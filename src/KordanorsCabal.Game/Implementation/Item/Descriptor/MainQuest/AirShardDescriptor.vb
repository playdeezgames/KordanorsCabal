Friend Class AirShardDescriptor
    Inherits ItemType

    Sub New()
        MyBase.New(
            StaticWorldData.World,
            OldItemType.AirShard,
            ,,,,,,,,,,,,
            "CanUseAirShard",
            "UseAirShard")
    End Sub
End Class
