Public Interface IDirection
    Inherits IBaseThingie
    ReadOnly Property Opposite As IDirection
    ReadOnly Property PreviousDirection As IDirection
    ReadOnly Property NextDirection As IDirection
    ReadOnly Property Name As String
    ReadOnly Property Abbreviation As String
End Interface
