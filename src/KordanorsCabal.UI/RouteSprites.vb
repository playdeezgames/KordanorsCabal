Imports System.Runtime.CompilerServices

Module RouteSprites
    ReadOnly routeSprites As IReadOnlyDictionary(Of Long, Sprite) =
        New Dictionary(Of Long, Sprite) From
        {
            {
                10L,
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
    Function Sprite(routeType As IRouteType) As Sprite
        Return routeSprites(routeType.Id)
    End Function
End Module
