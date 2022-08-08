Public Enum DungeonLevel
    None
    Level1
    Level2
    Level3
    Level4
    Level5
    Moon
End Enum
Friend Module DungeonLevelUtility
    Friend ReadOnly AllDungeonLevels As IReadOnlyList(Of DungeonLevel) =
        New List(Of DungeonLevel) From
        {
            DungeonLevel.Level1,
            DungeonLevel.Level2,
            DungeonLevel.Level3,
            DungeonLevel.Level4,
            DungeonLevel.Level5,
            DungeonLevel.Moon
        }
End Module

