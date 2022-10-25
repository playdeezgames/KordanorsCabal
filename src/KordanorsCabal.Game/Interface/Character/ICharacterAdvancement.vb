Public Interface ICharacterAdvancement
    Inherits IBaseThingie
    Function AddXP(xp As Long) As Boolean
    Sub AssignPoint(statisticType As IStatisticType)
    ReadOnly Property IsFullyAssigned As Boolean
End Interface
