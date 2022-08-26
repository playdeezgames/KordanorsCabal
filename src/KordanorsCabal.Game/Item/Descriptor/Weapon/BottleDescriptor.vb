Friend Class BottleDescriptor
    Inherits ItemTypeDescriptor

    Sub New()
        MyBase.New(
            StaticWorldData.World,
            ItemType.Bottle,
            1,,,
            MakeList(EquipSlot.FromId(StaticWorldData.World, 1L)), ,
            2,
            1,,
            3,,,,,,,,,
            Function(character)
                Dim enemy = character.Location.Enemy(character)
                Return enemy IsNot Nothing AndAlso enemy.CanBeBribedWith(ItemType.Bottle)
            End Function,
            Sub(character)
                Dim enemy = character.Location.Enemy(character)
                character.EnqueueMessage($"You give the {ItemType.Bottle.Name} to the {enemy.Name}, and it wanders off happily.")
                enemy.Destroy()
            End Sub)
    End Sub
End Class
