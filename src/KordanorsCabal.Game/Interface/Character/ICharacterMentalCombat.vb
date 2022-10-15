Public Interface ICharacterMentalCombat
    Inherits IBaseThingie
    Sub AddStress(delta As Long)
    ReadOnly Property CanDoIntimidation() As Boolean
    Sub DoIntimidation()
    ReadOnly Property CanIntimidate As Boolean
    ReadOnly Property IsDemoralized As Boolean
    Property CurrentMP As Long
    Function RollWillpower() As Long
    Function RollInfluence() As Long
End Interface
