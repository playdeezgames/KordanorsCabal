Imports System.Runtime.CompilerServices

Public Enum OldRouteType
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
    Function Abbreviation(routeType As OldRouteType) As String
        Return RouteTypeDescriptors(routeType).Abbreviation
    End Function
    <Extension>
    Function UnlockItem(routeType As OldRouteType) As Long?
        Return RouteTypeDescriptors(routeType).UnlockItem
    End Function
    <Extension>
    Function UnlockedRouteType(routeType As OldRouteType) As OldRouteType?
        Return RouteTypeDescriptors(routeType).UnlockedRouteType
    End Function
    <Extension>
    Function IsSingleUse(routeType As OldRouteType) As Boolean
        Return RouteTypeDescriptors(routeType).IsSingleUse
    End Function
End Module
