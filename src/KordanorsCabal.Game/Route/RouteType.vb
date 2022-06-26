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
End Module
