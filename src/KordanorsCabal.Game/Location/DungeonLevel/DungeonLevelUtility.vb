Imports System.Runtime.CompilerServices

Public Module DungeonLevelUtility
    Friend ReadOnly Property AllDungeonLevels As IReadOnlyList(Of DungeonLevel)
        Get
            Return StaticWorldData.World.DungeonLevel.ReadAll().Select(Function(x) New DungeonLevel(x)).ToList
        End Get
    End Property

    Public Const Level1 = "Level I"
    Public Const Level2 = "Level II"
    Public Const Level3 = "Level III"
    Public Const Level4 = "Level IV"
    Public Const Level5 = "Level V"
    Public Const TheMoon = "The Moon"
End Module
