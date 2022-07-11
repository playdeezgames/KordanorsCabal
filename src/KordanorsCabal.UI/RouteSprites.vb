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
End Module
