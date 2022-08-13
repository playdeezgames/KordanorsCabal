Imports System.Runtime.CompilerServices

Public Module DungeonLevelUtility
    Friend ReadOnly Property AllDungeonLevels As IReadOnlyList(Of DungeonLevel)
        Get
            Return StaticWorldData.World.DungeonLevel.ReadAll().Select(Function(x) New DungeonLevel(x)).ToList
        End Get
    End Property

    Public Const Level1 = 1L
    Public Const Level2 = 2L
    Public Const Level3 = 3L
    Public Const Level4 = 4L
    Public Const Level5 = 5L
    Public Const TheMoon = 6L
End Module
