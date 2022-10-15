Public Interface ICharacterAdvancement
    Inherits IBaseThingie
    Function AddXP(xp As Long) As Boolean
    Sub AssignPoint(statisticType As ICharacterStatisticType)
    ReadOnly Property IsFullyAssigned As Boolean
End Interface
