Imports System.Runtime.CompilerServices

Public Module DungeonLevelUtility
    Friend ReadOnly AllDungeonLevels As IReadOnlyList(Of OldDungeonLevel) =
        New List(Of OldDungeonLevel) From
        {
            OldDungeonLevel.Level1,
            OldDungeonLevel.Level2,
            OldDungeonLevel.Level3,
            OldDungeonLevel.Level4,
            OldDungeonLevel.Level5,
            OldDungeonLevel.Moon
        }
    Public Const Level1 = "Level I"
    Public Const Level2 = "Level II"
    Public Const Level3 = "Level III"
    Public Const Level4 = "Level IV"
    Public Const Level5 = "Level V"
    Public Const Moon = "The Moon"
    <Extension>
    Public Function ToOld(dungeonLevel As DungeonLevel) As OldDungeonLevel
        Return CType(dungeonLevel.Id, OldDungeonLevel)
    End Function
    <Extension>
    Public Function ToNew(oldDungeonLevel As OldDungeonLevel) As DungeonLevel
        Return New DungeonLevel(oldDungeonLevel)
    End Function
End Module
