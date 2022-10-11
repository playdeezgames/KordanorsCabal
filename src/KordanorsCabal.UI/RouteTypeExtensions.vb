Imports System.Runtime.CompilerServices

Module RouteTypeExtensions
    Private ReadOnly table As IReadOnlyDictionary(Of Long, Hue) =
        New Dictionary(Of Long, Hue) From
        {
            {5L, Hue.Green},
            {7L, Hue.Yellow},
            {4L, Hue.Black},
            {8L, Hue.Cyan},
            {6L, Hue.Purple}
        }
    <Extension>
    Function TextHue(routeType As IRouteType) As Hue
        Dim result As Hue = Hue.Blue
        table.TryGetValue(routeType.Id, result)
        Return result
    End Function
End Module
