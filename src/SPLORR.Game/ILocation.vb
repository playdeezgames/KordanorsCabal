Public Interface ILocation(Of TLocationType)
    ReadOnly Property Id As Long
    Property LocationType As TLocationType
    ReadOnly Property Name As String
End Interface
