Imports System.Runtime.CompilerServices

Module RouteSprites
    ReadOnly routeSprites As IReadOnlyDictionary(Of RouteType, Sprite) =
        New Dictionary(Of RouteType, Sprite) From
        {
            {
                RouteType.Portal,
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
    Function Sprite(routeType As RouteType) As Sprite
        Return routeSprites(routeType)
    End Function
End Module
