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
    ReadOnly characterSprites As IReadOnlyDictionary(Of OldCharacterType, Sprite) =
        New Dictionary(Of OldCharacterType, Sprite) From
        {
            {
                OldCharacterType.Acolyte,
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
                OldCharacterType.Badger,
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
                OldCharacterType.Bat,
                New Sprite(
                    Hue.Black,
                    LoadPixels(
                        AddressOf PixelConvertor,
                        "...#.....#..",
                        "..#.......#.",
                        ".##.#...#.##",
                        ".##.#####.##",
                        ".##.#.#.#.##",
                        ".##..###..##",
                        "..##..#..##.",
                        ".#.#.###.#.#",
                        ".....###....",
                        "............",
                        ".....#.#....",
                        "............"))
            },
            {
                OldCharacterType.Bishop,
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
                OldCharacterType.CabalLeader,
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
                OldCharacterType.Goblin,
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
                OldCharacterType.GoblinElite,
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
                OldCharacterType.Kordanor,
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
                OldCharacterType.Malcontent,
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
                OldCharacterType.MoonPerson,
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
                OldCharacterType.Priest,
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
                OldCharacterType.Rat,
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
                OldCharacterType.Skeleton,
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
                OldCharacterType.Snake,
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
                OldCharacterType.Zombie,
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
    Function Sprite(characterType As OldCharacterType) As Sprite
        Return characterSprites(characterType)
    End Function
End Module
