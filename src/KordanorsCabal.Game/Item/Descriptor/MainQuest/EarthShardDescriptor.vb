Friend Class EarthShardDescriptor
    Inherits ItemTypeDescriptor
    Sub New()
        MyBase.New(
            StaticWorldData.World,
            ItemType.EarthShard,,
            ,,,, ,
            ,,,,,,,
            Function(character)
                Dim location = character.Location
                Return location.IsDungeon AndAlso location.Enemies(character).Any AndAlso character.CurrentMana > 0
            End Function,
            "UseEarthShard")
    End Sub
End Class
