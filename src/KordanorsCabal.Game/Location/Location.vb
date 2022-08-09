Public Class Location
    Public ReadOnly Property Id As Long
    Public Sub New(locationId As Long)
        Id = locationId
    End Sub
    Property LocationType As LocationType
        Get
            Return CType(StaticWorldData.World.Location.ReadLocationType(Id).Value, LocationType)
        End Get
        Set(value As LocationType)
            StaticWorldData.World.Location.WriteLocationType(Id, value)
        End Set
    End Property

    Friend Shared Function FromLocationType(locationType As LocationType) As IEnumerable(Of Location)
        Return StaticWorldData.World.Location.ReadForLocationType(locationType).Select(AddressOf FromId)
    End Function
    Shared Function Create(locationType As LocationType) As Location
        Return FromId(StaticWorldData.World.Location.Create(locationType))
    End Function
    Public Shared Function ByStatisticValue(statisticType As LocationStatisticType, statisticValue As Long) As IEnumerable(Of Location)
        Return StaticWorldData.World.LocationStatistic.ReadForStatisticValue(statisticType, statisticValue).Select(AddressOf Location.FromId)
    End Function
    Public Shared Function ByDungeonLevel(dungeonLevel As OldDungeonLevel) As IEnumerable(Of Location)
        Return StaticWorldData.World.LocationDungeonLevel.ReadForDungeonLevel(dungeonLevel).Select(AddressOf Location.FromId)
    End Function
    Friend Sub DestroyRoute(direction As Direction)
        If Routes.ContainsKey(direction) Then
            Routes(direction).Destroy()
        End If
    End Sub
    Friend Function IsDungeon() As Boolean
        Return LocationType.IsDungeon
    End Function
    Public Function GetStatistic(statisticType As LocationStatisticType) As Long?
        Return StaticWorldData.World.LocationStatistic.Read(Id, statisticType)
    End Function
    ReadOnly Property RequiresMP As Boolean
        Get
            Return LocationType.RequiresMP
        End Get
    End Property
    Public Function HasStairs() As Boolean
        Return Routes.Any(Function(x) x.Value.RouteType = RouteType.Stairs)
    End Function
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
            Return StaticWorldData.World.Route.ReadForLocation(Id).
                ToDictionary(Function(x) CType(x.Item1, Direction), Function(x) Route.FromId(x.Item2))
        End Get
    End Property
    Public Function HasRoute(direction As Direction) As Boolean
        Return Routes.ContainsKey(direction)
    End Function
    Friend Sub SetStatistic(statisticType As LocationStatisticType, statisticValue As Long?)
        StaticWorldData.World.LocationStatistic.Write(Id, statisticType, statisticValue)
    End Sub
    Friend ReadOnly Property HasFeature As Boolean
        Get
            Return Feature IsNot Nothing
        End Get
    End Property
    ReadOnly Property Feature As Feature
        Get
            Return Feature.FromId(StaticWorldData.World.Feature.ReadForLocation(Id))
        End Get
    End Property
    ReadOnly Property Inventory As Inventory
        Get
            Dim inventoryId As Long? = StaticWorldData.World.Inventory.ReadForLocation(Id)
            If Not inventoryId.HasValue Then
                inventoryId = StaticWorldData.World.Inventory.CreateForLocation(Id)
            End If
            Return New Inventory(inventoryId.Value)
        End Get
    End Property
    Friend Function RouteCount() As Integer
        Return Routes.Count
    End Function
    Public Shared Operator =(first As Location, second As Location) As Boolean
        Return first.Id = second.Id
    End Operator
    Public Shared Operator <>(first As Location, second As Location) As Boolean
        Return first.Id <> second.Id
    End Operator
    ReadOnly Property Characters As IEnumerable(Of Character)
        Get
            Return StaticWorldData.World.Character.ReadForLocation(Id).Select(AddressOf Character.FromId)
        End Get
    End Property
    Function Enemies(character As Character) As IEnumerable(Of Character)
        Return Characters.Where(Function(x) x.IsEnemy(character))
    End Function
    Function Enemy(character As Character) As Character
        Return Enemies(character).FirstOrDefault
    End Function
    Function Friends(character As Character) As IEnumerable(Of Character)
        Return Characters.Where(Function(x) Not x.IsEnemy(character))
    End Function
    Friend ReadOnly Property CanMap As Boolean
        Get
            Return LocationType.CanMap
        End Get
    End Property
    Property DungeonLevel As OldDungeonLevel
        Get
            Return CType(If(StaticWorldData.World.LocationDungeonLevel.Read(Id), 0), OldDungeonLevel)
        End Get
        Set(value As OldDungeonLevel)
            StaticWorldData.World.LocationDungeonLevel.Write(Id, value)
        End Set
    End Property
End Class
