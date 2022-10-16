Public Interface ICharacterStatuses
    Inherits IBaseThingie
    ReadOnly Property IsUndead As Boolean
    Property Hunger As Long
    Property Highness As Long
    Property FoodPoisoning As Long
    Property Drunkenness As Long
    Property Chafing As Long
    Property Money As Long
    Sub DoImmobilization(delta As Long)
End Interface
