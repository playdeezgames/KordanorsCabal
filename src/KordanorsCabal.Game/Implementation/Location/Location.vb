Public Class Location
    Inherits BaseThingie
    Implements ILocation
    Public Sub New(worldData As IWorldData, locationId As Long)
        MyBase.New(worldData, locationId)
    End Sub
    Property LocationType As ILocationType Implements ILocation.LocationType
        Get
            Return New LocationType(WorldData, WorldData.Location.ReadLocationType(Id).Value)
        End Get
        Set(value As ILocationType)
            WorldData.Location.WriteLocationType(Id, value.Id)
        End Set
    End Property

    Friend Shared Function FromLocationType(worldData As IWorldData, locationType As ILocationType) As IEnumerable(Of ILocation)
        Return worldData.Location.ReadForLocationType(locationType.Id).Select(Function(x) FromId(worldData, x))
    End Function
    Shared Function Create(worldData As IWorldData, locationType As ILocationType) As ILocation
        Return FromId(worldData, worldData.Location.Create(locationType.Id))
    End Function
    Public Shared Function ByDungeonLevel(worldData As IWorldData, dungeonLevel As IDungeonLevel) As IEnumerable(Of ILocation)
        Return worldData.LocationDungeonLevel.ReadForDungeonLevel(dungeonLevel.Id).Select(Function(x) Location.FromId(worldData, x))
    End Function
    Sub DestroyRoute(direction As IDirection) Implements ILocation.DestroyRoute
        Routes(direction)?.Destroy()
    End Sub
    Function IsDungeon() As Boolean Implements ILocation.IsDungeon
        Return LocationType.IsDungeon
    End Function
    Public Function GetStatistic(statisticType As LocationStatisticType) As Long? Implements ILocation.GetStatistic
        Return WorldData.LocationStatistic.Read(Id, statisticType)
    End Function
    ReadOnly Property RequiresMP As Boolean Implements ILocation.RequiresMP
        Get
            Return LocationType.RequiresMP
        End Get
    End Property
    Public Function HasStairs() As Boolean Implements ILocation.HasStairs
        Return WorldData.Route.ReadForLocationRouteType(Id, RouteType.Stairs).Any()
    End Function
    Shared Function FromId(worldData As IWorldData, locationId As Long?) As ILocation
        Return If(locationId.HasValue, New Location(worldData, locationId.Value), Nothing)
    End Function
    ReadOnly Property Name As String Implements ILocation.Name
        Get
            Return LocationType.Name
        End Get
    End Property
    Public Function Routes(direction As IDirection) As Route Implements ILocation.Routes
        Return Route.FromId(WorldData, WorldData.Route.ReadForLocationDirection(Id, direction.Id))
    End Function
    Public Function HasRoute(direction As IDirection) As Boolean Implements ILocation.HasRoute
        Return Routes(direction) IsNot Nothing
    End Function
    Sub SetStatistic(statisticType As LocationStatisticType, statisticValue As Long?) Implements ILocation.SetStatistic
        WorldData.LocationStatistic.Write(Id, statisticType, statisticValue)
    End Sub
    ReadOnly Property HasFeature As Boolean Implements ILocation.HasFeature
        Get
            Return Feature IsNot Nothing
        End Get
    End Property
    ReadOnly Property Feature As Feature Implements ILocation.Feature
        Get
            Return Feature.FromId(WorldData, WorldData.Feature.ReadForLocation(Id))
        End Get
    End Property
    ReadOnly Property Inventory As IInventory Implements ILocation.Inventory
        Get
            Dim inventoryId As Long? = WorldData.Inventory.ReadForLocation(Id)
            If Not inventoryId.HasValue Then
                inventoryId = WorldData.Inventory.CreateForLocation(Id)
            End If
            Return Game.Inventory.FromId(WorldData, inventoryId.Value)
        End Get
    End Property
    Public ReadOnly Property RouteDirections As IEnumerable(Of IDirection) Implements ILocation.RouteDirections
        Get
            Return WorldData.Route.ReadDirectionRouteForLocation(Id).Select(Function(x) New Direction(WorldData, x.Item1))
        End Get
    End Property
    Public ReadOnly Property RouteTypes As IEnumerable(Of RouteType) Implements ILocation.RouteTypes
        Get
            Return WorldData.Route.ReadDirectionRouteTypeForLocation(Id).Select(Function(x) CType(x.Item2, RouteType))
        End Get
    End Property
    Function RouteCount() As Long Implements ILocation.RouteCount
        Return WorldData.Route.ReadCountForLocation(Id)
    End Function
    ReadOnly Property Characters As IEnumerable(Of ICharacter)
        Get
            Return WorldData.Character.ReadForLocation(Id).Select(Function(x) Character.FromId(WorldData, x))
        End Get
    End Property
    Function Enemies(character As ICharacter) As IEnumerable(Of ICharacter) Implements ILocation.Enemies
        Return Characters.Where(Function(x) x.IsEnemy(character))
    End Function
    Function Enemy(character As ICharacter) As ICharacter Implements ILocation.Enemy
        Return Enemies(character).FirstOrDefault
    End Function
    Function Friends(character As ICharacter) As IEnumerable(Of ICharacter) Implements ILocation.Friends
        Return Characters.Where(Function(x) Not x.IsEnemy(character))
    End Function
    ReadOnly Property CanMap As Boolean Implements ILocation.CanMap
        Get
            Return LocationType.CanMap
        End Get
    End Property
    Property DungeonLevel As IDungeonLevel Implements ILocation.DungeonLevel
        Get
            Return Game.DungeonLevel.FromId(WorldData, WorldData.LocationDungeonLevel.Read(Id))
        End Get
        Set(value As IDungeonLevel)
            WorldData.LocationDungeonLevel.Write(Id, value.Id)
        End Set
    End Property
End Class
