Public Interface ILocation
    Inherits IBaseThingie
    Property LocationType As ILocationType
    Property DungeonLevel As IDungeonLevel
    ReadOnly Property Inventory As IInventory
    ReadOnly Property Routes As IRoutes

    ReadOnly Property RouteCount As Long
    ReadOnly Property RouteDirections As IEnumerable(Of IDirection)
    Sub DestroyRoute(direction As IDirection)
    ReadOnly Property HasStairs As Boolean
    ReadOnly Property RouteTypes As IEnumerable(Of IRouteType)


    Sub SetStatistic(statisticType As LocationStatisticType, statisticValue As Long?)
    Function GetStatistic(statisticType As LocationStatisticType) As Long?

    Function Friends(character As ICharacter) As IEnumerable(Of ICharacter)
    Function Enemy(character As ICharacter) As ICharacter
    Function Enemies(character As ICharacter) As IEnumerable(Of ICharacter)

    ReadOnly Property HasFeature As Boolean
    ReadOnly Property Feature As IFeature
End Interface
