Friend Class AirShardDescriptor
    Inherits ItemTypeDescriptor

    Sub New()
        MyBase.New(
            StaticWorldData.World,
            ItemType.AirShard,
            ,,,,,,,,,,,,,
            Function(character) character.Location.IsDungeon AndAlso character.CurrentMana > 0,
            "UseAirShard")
    End Sub
End Class
