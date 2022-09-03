Friend Class AirShardDescriptor
    Inherits ItemTypeDescriptor

    Sub New()
        MyBase.New(
            StaticWorldData.World,
            ItemType.AirShard,
            ,,,,,,,,,,,,,
            "CanUseAirShard",
            "UseAirShard")
    End Sub
End Class
