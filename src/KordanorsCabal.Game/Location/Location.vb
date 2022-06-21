Public Class Location
    ReadOnly Property Id As Long
    Sub New(locationId As Long)
        Id = locationId
    End Sub

    Friend Shared Function FromLocationType(locationType As LocationType) As IEnumerable(Of Location)
        Return LocationData.ReadForLocationType(locationType).Select(AddressOf FromId)
    End Function

    Shared Function Create(locationType As LocationType) As Location
        Return FromId(LocationData.Create(locationType))
    End Function
    ReadOnly Property LocationType As LocationType
        Get
            Return CType(LocationData.ReadLocationType(Id).Value, LocationType)
        End Get
    End Property
    Shared Function FromId(locationId As Long) As Location
        Return New Location(locationId)
    End Function

    ReadOnly Property Name As String
        Get
            Return LocationType.Name
        End Get
    End Property
    ReadOnly Property Routes As IReadOnlyDictionary(Of Direction, Route)
        Get
            Return RouteData.ReadForLocation(Id).
                ToDictionary(Function(x) CType(x.Item1, Direction), Function(x) Route.FromId(x.Item2))
        End Get
    End Property
End Class
