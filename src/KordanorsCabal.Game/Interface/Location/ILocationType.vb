Public Interface ILocationType
    Inherits IBaseThingie
    ReadOnly Property RequiresMP() As Boolean
    ReadOnly Property Name As String
    ReadOnly Property IsDungeon As Boolean
    ReadOnly Property CanMap As Boolean
End Interface
