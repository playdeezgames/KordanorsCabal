Friend Class WaterShardDescriptor
    Inherits ItemTypeDescriptor

    Sub New()
        MyBase.New(
            StaticWorldData.World,
            ItemType.WaterShard,
            ,,,,,,,,,,,,,
            Function(character) character.Location.IsDungeon AndAlso character.CurrentMana > 0,
            "UseWaterShard")
    End Sub
End Class
