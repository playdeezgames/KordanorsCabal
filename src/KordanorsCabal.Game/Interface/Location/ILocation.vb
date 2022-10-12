Public Interface ILocation
    Inherits IBaseThingie
    ReadOnly Property Name As String
    Property LocationType As ILocationType
    Property DungeonLevel As IDungeonLevel
    ReadOnly Property RequiresMP As Boolean
    Function IsDungeon() As Boolean
    ReadOnly Property CanMap As Boolean

    Function HasRoute(direction As IDirection) As Boolean
    Function Routes(direction As IDirection) As IRoute
    Function RouteCount() As Long
    ReadOnly Property RouteDirections As IEnumerable(Of IDirection)
    Sub DestroyRoute(direction As IDirection)
    Function HasStairs() As Boolean
    ReadOnly Property RouteTypes As IEnumerable(Of IRouteType)

    ReadOnly Property Inventory As IInventory

    Sub SetStatistic(statisticType As LocationStatisticType, statisticValue As Long?)
    Function GetStatistic(statisticType As LocationStatisticType) As Long?

    Function Friends(character As ICharacter) As IEnumerable(Of ICharacter)
    Function Enemy(character As ICharacter) As ICharacter
    Function Enemies(character As ICharacter) As IEnumerable(Of ICharacter)

    ReadOnly Property HasFeature As Boolean
    ReadOnly Property Feature As IFeature
End Interface
