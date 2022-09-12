Public Interface ICharacterTypeCombat
    Inherits IBaseThingie
    Function CanBeBribedWith(itemType As OldItemType) As Boolean
    Sub DropLoot(location As ILocation)
End Interface
