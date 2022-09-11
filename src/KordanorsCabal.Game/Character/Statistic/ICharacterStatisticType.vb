Public Interface ICharacterStatisticType
    Inherits IBaseThingie
    ReadOnly Property Name As String
    ReadOnly Property DefaultValue As Long?
    ReadOnly Property MinimumValue() As Long
    ReadOnly Property MaximumValue() As Long
    ReadOnly Property Abbreviation As String
End Interface
