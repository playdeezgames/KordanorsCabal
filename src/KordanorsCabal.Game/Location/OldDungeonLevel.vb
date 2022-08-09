Public Enum OldDungeonLevel
    None
    Level1
    Level2
    Level3
    Level4
    Level5
    Moon
End Enum
Friend Module OldDungeonLevelUtility
    Friend ReadOnly OldAllDungeonLevels As IReadOnlyList(Of OldDungeonLevel) =
        New List(Of OldDungeonLevel) From
        {
            OldDungeonLevel.Level1,
            OldDungeonLevel.Level2,
            OldDungeonLevel.Level3,
            OldDungeonLevel.Level4,
            OldDungeonLevel.Level5,
            OldDungeonLevel.Moon
        }
End Module

