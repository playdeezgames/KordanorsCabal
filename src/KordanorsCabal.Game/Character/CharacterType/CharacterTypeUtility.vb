Public Module CharacterTypeUtility
    Function AllCharacterTypes(worldData As WorldData) As IEnumerable(Of CharacterType)
        Return StaticWorldData.World.CharacterType.ReadAll().Select(Function(x) CharacterType.FromId(worldData, x))
    End Function
    Public Const Acolyte = 1L
    Public Const Badger = 2L
    Public Const Bat = 3L
    Public Const Bishop = 4L
    Public Const CabalLeader = 5L
    Public Const Goblin = 6L
    Public Const GoblinElite = 7L
    Public Const Kordanor = 8L
    Public Const Malcontent = 9L
    Public Const MoonPerson = 10L
    Public Const N00b = 11L
    Public Const Priest = 12L
    Public Const Rat = 13L
    Public Const Skeleton = 14L
    Public Const Snake = 15L
    Public Const Zombie = 16L
End Module