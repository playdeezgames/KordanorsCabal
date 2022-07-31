Imports System.Runtime.CompilerServices

Module RouteTypeExtensions
    Private ReadOnly table As IReadOnlyDictionary(Of RouteType, Hue) =
        New Dictionary(Of RouteType, Hue) From
        {
            {RouteType.CopperLock, Hue.Green},
            {RouteType.GoldLock, Hue.Yellow},
            {RouteType.IronLock, Hue.Black},
            {RouteType.PlatinumLock, Hue.Cyan},
            {RouteType.SilverLock, Hue.Purple}
        }
    <Extension>
    Function TextHue(routeType As RouteType) As Hue
        Dim result As Hue = Hue.Blue
        table.TryGetValue(routeType, result)
        Return result
    End Function
End Module
