Friend Class FireShardDescriptor
    Inherits ItemTypeDescriptor
    Sub New()
        MyBase.New(
            StaticWorldData.World,
            ItemType.FireShard,,,,,,,,,,,,,,
            Function(character)
                Dim location = character.Location
                Return location.IsDungeon AndAlso location.Enemies(character).Any AndAlso character.CurrentMana > 0
            End Function,
            "UseFireShard")
    End Sub
End Class
