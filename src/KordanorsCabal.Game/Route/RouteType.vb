Imports System.Runtime.CompilerServices

Public Enum RouteType
    None
    Road
    Passageway
    Stairs
    IronLock
    CopperLock
    SilverLock
    GoldLock
    PlatinumLock
End Enum
Public Module RouteTypeExtensions
    <Extension>
    Function Abbreviation(routeType As RouteType) As String
        Return RouteTypeDescriptors(routeType).Abbreviation
    End Function
    <Extension>
    Function UnlockItem(routeType As RouteType) As ItemType?
        Return RouteTypeDescriptors(routeType).UnlockItem
    End Function
    <Extension>
    Function UnlockedRouteType(routeType As RouteType) As RouteType?
        Return RouteTypeDescriptors(routeType).UnlockedRouteType
    End Function
End Module
