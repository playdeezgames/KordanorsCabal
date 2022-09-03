Friend Class BottleDescriptor
    Inherits ItemTypeDescriptor

    Sub New()
        MyBase.New(
            StaticWorldData.World,
            ItemType.Bottle,
            MakeList(EquipSlot.FromId(StaticWorldData.World, 1L)), ,
            2,
            1,,
            3,,,,,,,,
            Function(character)
                Dim enemy = character.Location.Enemy(character)
                Return enemy IsNot Nothing AndAlso enemy.CanBeBribedWith(ItemType.Bottle)
            End Function,
            "UseBotttle")
    End Sub
End Class
