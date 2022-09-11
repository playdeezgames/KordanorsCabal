Public Interface ILocation
    Inherits IBaseThingie
    Function Routes(direction As Direction) As Route
    ReadOnly Property Inventory As Inventory
    Sub SetStatistic(statisticType As LocationStatisticType, statisticValue As Long?)
    ReadOnly Property RequiresMP As Boolean
    Function RouteCount() As Long
    Function Enemy(character As ICharacter) As ICharacter
    ReadOnly Property RouteDirections As IEnumerable(Of Direction)
    Property LocationType As ILocationType
    Function IsDungeon() As Boolean
    ReadOnly Property HasFeature As Boolean
    Function HasRoute(direction As Direction) As Boolean
    Function Friends(character As ICharacter) As IEnumerable(Of ICharacter)
    ReadOnly Property Feature As Feature
    Function Enemies(character As ICharacter) As IEnumerable(Of ICharacter)
    Property DungeonLevel As IDungeonLevel
    Sub DestroyRoute(direction As Direction)
    ReadOnly Property CanMap As Boolean
    ReadOnly Property RouteTypes As IEnumerable(Of RouteType)
    ReadOnly Property Name As String
    Function HasStairs() As Boolean
    Function GetStatistic(statisticType As LocationStatisticType) As Long?
End Interface
