Imports System.Runtime.CompilerServices

Module RouteTypeExtensions
    <Extension>
    Function TextHue(routeType As RouteType) As Hue
        Select Case routeType
            Case RouteType.IronLock
                Return Hue.Black
            Case RouteType.CopperLock
                Return Hue.Green
            Case RouteType.SilverLock
                Return Hue.Purple
            Case RouteType.GoldLock
                Return Hue.Yellow
            Case RouteType.PlatinumLock
                Return Hue.Cyan
            Case Else
                Return Hue.Blue
        End Select
    End Function
End Module
