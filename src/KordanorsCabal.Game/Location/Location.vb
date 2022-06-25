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
    Property LocationType As LocationType
        Get
            Return CType(LocationData.ReadLocationType(Id).Value, LocationType)
        End Get
        Set(value As LocationType)
            LocationData.WriteLocationType(Id, value)
        End Set
    End Property
    Shared Function FromId(locationId As Long?) As Location
        Return If(locationId.HasValue, New Location(locationId.Value), Nothing)
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

    Public Function HasRoute(direction As Direction) As Boolean
        Return Routes.ContainsKey(direction)
    End Function

    Friend Sub SetStatistic(statisticType As LocationStatisticType, statisticValue As Long?)
        LocationStatisticData.Write(Id, statisticType, statisticValue)
    End Sub

    Friend ReadOnly Property HasFeature As Boolean
        Get
            Return Feature IsNot Nothing
        End Get
    End Property

    ReadOnly Property Feature As Feature
        Get
            Return Feature.FromId(FeatureData.ReadForLocation(Id))
        End Get
    End Property

    ReadOnly Property Inventory As Inventory
        Get
            Dim inventoryId As Long? = LocationInventoryData.Read(Id)
            If Not inventoryId.hasValue Then
                inventoryId = InventoryData.Create()
                LocationInventoryData.Write(Id, inventoryId.Value)
            End If
            Return New Inventory(inventoryId.Value)
        End Get
    End Property

    Friend Function RouteCount() As Integer
        Return Routes.Count
    End Function
End Class
