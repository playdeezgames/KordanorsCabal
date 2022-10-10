Imports System.Runtime.CompilerServices

Module RouteTypeExtensions
    Private ReadOnly table As IReadOnlyDictionary(Of Long, Hue) =
        New Dictionary(Of Long, Hue) From
        {
            {OldRouteType.CopperLock, Hue.Green},
            {OldRouteType.GoldLock, Hue.Yellow},
            {OldRouteType.IronLock, Hue.Black},
            {OldRouteType.PlatinumLock, Hue.Cyan},
            {OldRouteType.SilverLock, Hue.Purple}
        }
    <Extension>
    Function TextHue(routeType As IRouteType) As Hue
        Dim result As Hue = Hue.Blue
        table.TryGetValue(routeType.Id, result)
        Return result
    End Function
End Module
