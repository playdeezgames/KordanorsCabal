Public Interface ILocation
    Inherits IBaseThingie
    Property LocationType As ILocationType
    Property DungeonLevel As IDungeonLevel
    ReadOnly Property Inventory As IInventory
    ReadOnly Property Routes As IRoutes
    ReadOnly Property Statistics As ILocationStatistics
    ReadOnly Property Factions As ILocationFactions
    ReadOnly Property Feature As IFeature
End Interface
