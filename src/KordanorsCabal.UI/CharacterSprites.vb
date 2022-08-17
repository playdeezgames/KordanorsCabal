Imports System.Runtime.CompilerServices

Module CharacterSprites
    Function PixelConvertor(character As Char) As (Pattern, Boolean)?
        Select Case character
            Case " "c
                Return (Pattern.Space, False)
            Case "."c
                Return Nothing
            Case "#"c
                Return (Pattern.Space, True)
            Case Else
                Throw New NotImplementedException
        End Select
    End Function
    Function LoadPixels(convertor As Func(Of Char, (Pattern, Boolean)?),
                       ParamArray lines() As String) As IReadOnlyDictionary(Of (Integer, Integer), (Pattern, Boolean))
        Dim result As New Dictionary(Of (Integer, Integer), (Pattern, Boolean))
        For row = 0 To lines.Length - 1
            Dim line = lines(row)
            For column = 0 To line.Length - 1
                Dim character = line(column)
                Dim pixel = convertor(character)
                If pixel IsNot Nothing Then
                    result((column, row)) = pixel.Value
                End If
            Next
        Next
        Return result
    End Function
    ReadOnly characterSprites As IReadOnlyDictionary(Of Long, Sprite) =
        New Dictionary(Of Long, Sprite) From
        {
            {
                1,
                New Sprite(
                    Hue.Cyan,
                    LoadPixels(
                        AddressOf PixelConvertor,
                        "............",
                        ".....##.....",
                        "....####....",
                        "....   #....",
                        "....# # #...",
                        "....    #...",
                        "....#  ##...",
                        "...#     #..",
                        "....## ##...",
                        ".....   ....",
                        ".....###....",
                        "....#####..."))
            },
            {
                2,
                New Sprite(
                    Hue.Black,
                    LoadPixels(
                        AddressOf PixelConvertor,
                        "............",
                        "...#.#......",
                        "..####......",
                        ".. # ##.....",
                        ".######.....",
                        "# # #####...",
                        "##### ####..",
                        ".##  ######.",
                        "...#######.#",
                        "...#######..",
                        "....######..",
                        ".....#...#.."))
            },
            {
                3,
                New Sprite(
                    Hue.Black,
                    LoadPixels(
                        AddressOf PixelConvertor,
                        "...#.....#..",
                        "..#.......#.",
                        ".##.#...#.##",
                        ".##.#####.##",
                        ".##.# # #.##",
                        ".##..###..##",
                        "..##..#..##.",
                        ".#.#.###.#.#",
                        ".....###....",
                        "............",
                        ".....#.#....",
                        "............"))
            },
            {
                4,
                New Sprite(
                    Hue.Red,
                    LoadPixels(
                        AddressOf PixelConvertor,
                        "............",
                        ".....##.....",
                        "....####....",
                        "....   #....",
                        "....# # #...",
                        "....    #...",
                        "..#.#  ##...",
                        ".....###.#..",
                        "..##.###.#..",
                        ".....###....",
                        ".....###....",
                        "....#####..."))
            },
            {
                5,
                New Sprite(
                    Hue.Orange,
                    LoadPixels(
                        AddressOf PixelConvertor,
                        "............",
                        "..#...##....",
                        ".###.####...",
                        "..#..   #...",
                        ".....# # #..",
                        "..#..    #..",
                        "..#..#  ##..",
                        "..#...###.#.",
                        "..###.###.#.",
                        "..#...###...",
                        "..#...###...",
                        "..#..#####.."))
            },
            {
                6,
                New Sprite(
                    Hue.Green,
                    LoadPixels(
                        AddressOf PixelConvertor,
                        "............",
                        "............",
                        "............",
                        "............",
                        "....###.....",
                        ".... # #....",
                        "....###.....",
                        "........#...",
                        "....#.##.#..",
                        "...#..##.#..",
                        ".....#.#....",
                        "....##..#..."))
            },
            {
                7,
                New Sprite(
                    Hue.Green,
                    LoadPixels(
                        AddressOf PixelConvertor,
                        "............",
                        "............",
                        "............",
                        "............",
                        ".#..###.....",
                        ".#.. # #....",
                        ".#..###.....",
                        "..#.....#...",
                        "..#.#.##.#..",
                        "...#..##.#..",
                        ".....#.#....",
                        "....##..#..."))
            },
            {
                8,
                New Sprite(
                    Hue.Red,
                    LoadPixels(
                        AddressOf PixelConvertor,
                        "....#..#....",
                        "...#....#...",
                        "...#....#...",
                        "....####....",
                        ".... # #....",
                        "....###.....",
                        ".......##...",
                        ".....###.#..",
                        "...#.###.#..",
                        ".....###....",
                        "......#.#...",
                        ".....#..#..."))
            },
            {
                9,
                New Sprite(
                    Hue.Orange,
                    LoadPixels(
                        AddressOf PixelConvertor,
                        "............",
                        ".....#......",
                        "..#.###.....",
                        ".#.######...",
                        ".#.. # #....",
                        ".#..###.....",
                        "..#....##...",
                        ".....###.#..",
                        "...#.###.#..",
                        ".....#......",
                        ".....#.#....",
                        "....##.#...."))
            },
            {
                10,
                New Sprite(
                    Hue.LightGreen,
                    LoadPixels(
                        AddressOf PixelConvertor,
                        "............",
                        "....#####...",
                        "...# ### #..",
                        "...#  #  #..",
                        "....#####...",
                        "......#.....",
                        ".....####...",
                        ".....###.#..",
                        "...#.###.#..",
                        ".....#.#....",
                        ".....#.#....",
                        "....##.#...."))
            },
            {
                12,
                New Sprite(
                    Hue.Purple,
                    LoadPixels(
                        AddressOf PixelConvertor,
                        "............",
                        ".....##.....",
                        "....####....",
                        "....   #....",
                        "....# # #...",
                        "....    #...",
                        "....#  ##...",
                        ".....###.#..",
                        "...#.###.#..",
                        ".....###....",
                        ".....###....",
                        "....#####..."))
            },
            {
                13,
                New Sprite(
                    Hue.Black,
                    LoadPixels(
                        AddressOf PixelConvertor,
                        "............",
                        "............",
                        "...##.......",
                        "..#  ##...#.",
                        "..#  # ###..",
                        "...######...",
                        ".....###....",
                        "....###.....",
                        ".#.#####....",
                        "#..####.#...",
                        "#..# ##.....",
                        ".##.#####..."))
            },
            {
                14,
                New Sprite(
                    Hue.Blue,
                    LoadPixels(
                        AddressOf PixelConvertor,
                        "............",
                        ".....###....",
                        "....#####...",
                        ".... # ##...",
                        "....####....",
                        "....#.#.....",
                        "........#...",
                        ".....###.#..",
                        "...#..#..#..",
                        ".....###....",
                        ".....#.#....",
                        "....##.#...."))
            },
            {
                15,
                New Sprite(
                    Hue.Green,
                    LoadPixels(
                        AddressOf PixelConvertor,
                        "............",
                        "............",
                        "....####....",
                        "...# ####...",
                        "...##..##...",
                        "..#...##....",
                        "......##....",
                        ".....##.....",
                        ".#...##.##..",
                        "..##.##..##.",
                        ".....######.",
                        "......####.."))
            },
            {
                16,
                New Sprite(
                    Hue.Purple,
                    LoadPixels(
                        AddressOf PixelConvertor,
                        "............",
                        "............",
                        "....####....",
                        ".... #  #...",
                        "....#####...",
                        "....#.#.....",
                        "........#...",
                        ".....###.#..",
                        "...#.###.#..",
                        ".....###....",
                        ".....#.#....",
                        "....##.#...."))
            }
        }
    <Extension>
    Function Sprite(characterType As CharacterType) As Sprite
        Return characterSprites(characterType.Id)
    End Function
End Module
