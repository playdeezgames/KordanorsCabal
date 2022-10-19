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
    Shared Function FromId(worldData As IWorldData, locationId As Long?) As ILocation
        Return If(locationId.HasValue, New Location(worldData, locationId.Value), Nothing)
    End Function

    Public Sub DecayItems() Implements ILocation.DecayItems
        WorldData.Events.Perform(WorldData, "LocationDecayItems", Id)
    End Sub

    ReadOnly Property Feature As IFeature Implements ILocation.Feature
        Get
            Return Game.Feature.FromId(WorldData, WorldData.Feature.ReadForLocation(Id))
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
    Property DungeonLevel As IDungeonLevel Implements ILocation.DungeonLevel
        Get
            Return Game.DungeonLevel.FromId(WorldData, WorldData.LocationDungeonLevel.Read(Id))
        End Get
        Set(value As IDungeonLevel)
            WorldData.LocationDungeonLevel.Write(Id, value.Id)
        End Set
    End Property

    Public ReadOnly Property Routes As IRoutes Implements ILocation.Routes
        Get
            Return Game.Routes.FromId(WorldData, Id)
        End Get
    End Property

    Public ReadOnly Property Statistics As ILocationStatistics Implements ILocation.Statistics
        Get
            Return LocationStatistics.FromId(WorldData, Id)
        End Get
    End Property

    Public ReadOnly Property Factions As ILocationFactions Implements ILocation.Factions
        Get
            Return LocationFactions.FromId(WorldData, Id)
        End Get
    End Property
End Class
