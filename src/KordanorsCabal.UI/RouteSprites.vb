Imports System.Runtime.CompilerServices

Module RouteSprites
    ReadOnly routeSprites As IReadOnlyDictionary(Of OldRouteType, Sprite) =
        New Dictionary(Of OldRouteType, Sprite) From
        {
            {
                OldRouteType.Portal,
                New Sprite(
                        Hue.Purple,
                        LoadPixels(
                            AddressOf PixelConvertor,
                            "............",
                            "..####...#..",
                            ".#..###...#.",
                            ".....###..#.",
                            "...#####.##.",
                            "..#########.",
                            ".#########..",
                            ".##.#####...",
                            ".#..###.....",
                            ".#...###..#.",
                            "..#...####..",
                            "............"))
            }
        }
    <Extension>
    Function Sprite(routeType As OldRouteType) As Sprite
        Return routeSprites(routeType)
    End Function
End Module
