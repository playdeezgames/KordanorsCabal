Friend Class BeerDescriptor
    Inherits ItemTypeDescriptor

    Sub New()
        MyBase.New(
            StaticWorldData.World,
            ItemType.Beer,
            ,,,,,,,,
            5,
            MakeList(ShoppeType.InnKeeper),,,,
            Function(character)
                Dim enemy = character.Location.Enemy(character)
                Return enemy Is Nothing OrElse enemy.CanBeBribedWith(ItemType.Beer)
            End Function,
            "UseBeer")
    End Sub
End Class
