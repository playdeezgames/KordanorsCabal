Public Class Location
    Inherits BaseThingie
    Public Sub New(worldData As WorldData, locationId As Long)
        MyBase.New(worldData, locationId)
    End Sub
    Property LocationType As LocationType
        Get
            Return New LocationType(WorldData, WorldData.Location.ReadLocationType(Id).Value)
        End Get
        Set(value As LocationType)
            WorldData.Location.WriteLocationType(Id, value.Id)
        End Set
    End Property

    Friend Shared Function FromLocationType(worldData As WorldData, locationType As LocationType) As IEnumerable(Of Location)
        Return worldData.Location.ReadForLocationType(locationType.Id).Select(Function(x) FromId(worldData, x))
    End Function
    Shared Function Create(worldData As WorldData, locationType As LocationType) As Location
        Return FromId(worldData, worldData.Location.Create(locationType.Id))
    End Function
    Public Shared Function ByDungeonLevel(worldData As WorldData, dungeonLevel As DungeonLevel) As IEnumerable(Of Location)
        Return worldData.LocationDungeonLevel.ReadForDungeonLevel(dungeonLevel.Id).Select(Function(x) Location.FromId(worldData, x))
    End Function
    Friend Sub DestroyRoute(direction As Direction)
        Routes(direction)?.Destroy()
    End Sub
    Friend Function IsDungeon() As Boolean
        Return LocationType.IsDungeon
    End Function
    Public Function GetStatistic(statisticType As LocationStatisticType) As Long?
        Return WorldData.LocationStatistic.Read(Id, statisticType)
    End Function
    ReadOnly Property RequiresMP As Boolean
        Get
            Return LocationType.RequiresMP
        End Get
    End Property
    Public Function HasStairs() As Boolean
        Return WorldData.Route.ReadForLocationRouteType(Id, RouteType.Stairs).Any()
    End Function
    Shared Function FromId(worldData As WorldData, locationId As Long?) As Location
        Return If(locationId.HasValue, New Location(worldData, locationId.Value), Nothing)
    End Function
    ReadOnly Property Name As String
        Get
            Return LocationType.Name
        End Get
    End Property
    Public Function Routes(direction As Direction) As Route
        Return Route.FromId(WorldData.Route.ReadForLocationDirection(Id, direction.Id))
    End Function
    Public Function HasRoute(direction As Direction) As Boolean
        Return Routes(direction) IsNot Nothing
    End Function
    Friend Sub SetStatistic(statisticType As LocationStatisticType, statisticValue As Long?)
        WorldData.LocationStatistic.Write(Id, statisticType, statisticValue)
    End Sub
    Friend ReadOnly Property HasFeature As Boolean
        Get
            Return Feature IsNot Nothing
        End Get
    End Property
    ReadOnly Property Feature As Feature
        Get
            Return Feature.FromId(WorldData, WorldData.Feature.ReadForLocation(Id))
        End Get
    End Property
    ReadOnly Property Inventory As Inventory
        Get
            Dim inventoryId As Long? = WorldData.Inventory.ReadForLocation(Id)
            If Not inventoryId.HasValue Then
                inventoryId = WorldData.Inventory.CreateForLocation(Id)
            End If
            Return New Inventory(inventoryId.Value)
        End Get
    End Property
    Public ReadOnly Property RouteDirections As IEnumerable(Of Direction)
        Get
            Return WorldData.Route.ReadDirectionRouteForLocation(Id).Select(Function(x) New Direction(WorldData, x.Item1))
        End Get
    End Property
    Public ReadOnly Property RouteTypes As IEnumerable(Of RouteType)
        Get
            Return WorldData.Route.ReadDirectionRouteTypeForLocation(Id).Select(Function(x) CType(x.Item2, RouteType))
        End Get
    End Property
    Friend Function RouteCount() As Long
        Return WorldData.Route.ReadCountForLocation(Id)
    End Function
    Public Shared Operator =(first As Location, second As Location) As Boolean
        Return first.Id = second.Id
    End Operator
    Public Shared Operator <>(first As Location, second As Location) As Boolean
        Return first.Id <> second.Id
    End Operator
    ReadOnly Property Characters As IEnumerable(Of Character)
        Get
            Return WorldData.Character.ReadForLocation(Id).Select(Function(x) Character.FromId(WorldData, x))
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
    Property DungeonLevel As DungeonLevel
        Get
            Return Game.DungeonLevel.FromId(WorldData.LocationDungeonLevel.Read(Id))
        End Get
        Set(value As DungeonLevel)
            WorldData.LocationDungeonLevel.Write(Id, value.Id)
        End Set
    End Property
End Class
