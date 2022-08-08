Friend Class BottleDescriptor
    Inherits ItemTypeDescriptor

    Sub New()
        MyBase.New("Empty Bottle", 1,,, MakeList(EquipSlot.Weapon))
    End Sub

    Public Overrides ReadOnly Property AttackDice As Long
        Get
            Return 2
        End Get
    End Property

    Public Overrides ReadOnly Property MaximumDamage As Long?
        Get
            Return 1
        End Get
    End Property
    Public Overrides ReadOnly Property MaximumDurability As Long?
        Get
            Return 3
        End Get
    End Property

    Public Overrides ReadOnly Property CanUse(character As Character) As Boolean
        Get
            Dim enemy = character.Location.Enemy(character)
            Return enemy IsNot Nothing AndAlso enemy.CanBeBribedWith(ItemType.Bottle)
        End Get
    End Property

    Public Overrides Sub Use(character As Character)
        Dim enemy = character.Location.Enemy(character)
        character.EnqueueMessage($"You give the {ItemType.Bottle.Name} to the {enemy.Name}, and it wanders off happily.")
        enemy.Destroy()
    End Sub

End Class
