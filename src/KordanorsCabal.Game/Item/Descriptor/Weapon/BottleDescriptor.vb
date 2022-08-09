Friend Class BottleDescriptor
    Inherits ItemTypeDescriptor

    Sub New()
        MyBase.New(
            "Empty Bottle",
            1,,,
            MakeList(EquipSlot.Weapon), ,
            2,
            1,,
            3,,,,,,,,,
            Function(character)
                Dim enemy = character.Location.Enemy(character)
                Return enemy IsNot Nothing AndAlso enemy.CanBeBribedWith(ItemType.Bottle)
            End Function)
    End Sub
    Public Overrides Sub Use(character As Character)
        Dim enemy = character.Location.Enemy(character)
        character.EnqueueMessage($"You give the {ItemType.Bottle.Name} to the {enemy.Name}, and it wanders off happily.")
        enemy.Destroy()
    End Sub

End Class
