Friend Class HolyWaterDescriptor
    Inherits ItemTypeDescriptor

    Sub New()
        MyBase.New(
            StaticWorldData.World,
            ItemType.HolyWater,,,,,,,,,
            10,
            MakeList(ShoppeType.Healer),,,,
            Function(character) character.CanFight AndAlso character.Location.Enemy(character).IsUndead,
            "UseHolyWater")
    End Sub
End Class
