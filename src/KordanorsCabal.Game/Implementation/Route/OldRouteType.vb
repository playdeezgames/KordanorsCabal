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
    Function Abbreviation(routeType As OldRouteType, worldData As IWorldData) As String
        Return RouteTypeDescriptors(worldData)(routeType).Abbreviation
    End Function
    <Extension>
    Function UnlockItem(routeType As OldRouteType, worldData As IWorldData) As Long?
        Return RouteTypeDescriptors(worldData)(routeType).UnlockItem
    End Function
    <Extension>
    Function UnlockedRouteType(routeType As OldRouteType, worldData As IWorldData) As IRouteType
        Return RouteTypeDescriptors(worldData)(routeType).UnlockedRouteType
    End Function
    <Extension>
    Function IsSingleUse(routeType As OldRouteType, worldData As IWorldData) As Boolean
        Return RouteTypeDescriptors(worldData)(routeType).IsSingleUse
    End Function
End Module
