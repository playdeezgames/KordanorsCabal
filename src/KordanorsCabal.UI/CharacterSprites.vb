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
    ReadOnly sprites As IReadOnlyDictionary(Of CharacterType, Sprite) =
        New Dictionary(Of CharacterType, Sprite) From
        {
            {
                CharacterType.Acolyte,
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
                CharacterType.Bishop,
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
                CharacterType.CabalLeader,
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
                CharacterType.Goblin,
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
                CharacterType.GoblinElite,
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
                CharacterType.Kordanor,
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
                CharacterType.Priest,
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
                CharacterType.Skeleton,
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
            }
        }
    <Extension>
    Function Sprite(characterType As CharacterType) As Sprite
        Return sprites(characterType)
    End Function
End Module
