Friend Class Pr0nDescriptor
    Inherits ItemTypeDescriptor

    Sub New()
        MyBase.New(
            StaticWorldData.World,
            ItemType.Pr0n,,,,,,,,,
            10,
            MakeList(ShoppeType.BlackMarket),,,,
            Function(character)
                Dim enemy = character.Location.Enemy(character)
                Return (enemy IsNot Nothing AndAlso enemy.CanBeBribedWith(ItemType.Pr0n)) OrElse enemy Is Nothing
            End Function,
            "UsePr0n")
    End Sub
End Class
