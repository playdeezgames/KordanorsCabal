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
    FinalLock
    Portal
    MoonPath
End Enum
Public Module RouteTypeExtensions
    <Extension>
    Function Abbreviation(routeType As RouteType) As String
        Return RouteTypeDescriptors(routeType).Abbreviation
    End Function
    <Extension>
    Function UnlockItem(routeType As RouteType) As OldItemType?
        Return RouteTypeDescriptors(routeType).UnlockItem
    End Function
    <Extension>
    Function UnlockedRouteType(routeType As RouteType) As RouteType?
        Return RouteTypeDescriptors(routeType).UnlockedRouteType
    End Function
    <Extension>
    Function IsSingleUse(routeType As RouteType) As Boolean
        Return RouteTypeDescriptors(routeType).IsSingleUse
    End Function
End Module
