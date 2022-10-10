Imports System.Runtime.CompilerServices

Module RouteTypeExtensions
    Private ReadOnly table As IReadOnlyDictionary(Of OldRouteType, Hue) =
        New Dictionary(Of OldRouteType, Hue) From
        {
            {OldRouteType.CopperLock, Hue.Green},
            {OldRouteType.GoldLock, Hue.Yellow},
            {OldRouteType.IronLock, Hue.Black},
            {OldRouteType.PlatinumLock, Hue.Cyan},
            {OldRouteType.SilverLock, Hue.Purple}
        }
    <Extension>
    Function TextHue(routeType As OldRouteType) As Hue
        Dim result As Hue = Hue.Blue
        table.TryGetValue(routeType, result)
        Return result
    End Function
End Module
