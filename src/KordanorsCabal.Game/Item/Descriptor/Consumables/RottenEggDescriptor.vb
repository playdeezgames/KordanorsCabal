Friend Class RottenEggDescriptor
    Inherits ItemTypeDescriptor

    Sub New()
        MyBase.New(
            StaticWorldData.World,
            ItemType.RottenEgg,,,,,,,,,,,,,,
            Function(character)
                Dim enemy = character.Location.Enemy(character)
                Return (enemy IsNot Nothing AndAlso enemy.CanBeBribedWith(ItemType.RottenEgg))
            End Function,
            "RottenEgg")
    End Sub
End Class
