Imports System.Runtime.CompilerServices

Public Enum OldCharacterType
    None
    Acolyte
    Badger
    Bat
    Bishop
    CabalLeader
    Goblin
    GoblinElite
    Kordanor
    Malcontent
    MoonPerson
    N00b
    Priest
    Rat
    Skeleton
    Snake
    Zombie
End Enum
Public Module CharacterTypeUtility
    Function AllCharacterTypes() As IEnumerable(Of OldCharacterType)
        Return StaticWorldData.World.CharacterType.ReadAll().Select(Function(x) CType(x, OldCharacterType))
    End Function
    <Extension>
    Function ToNew(oldCharacterType As OldCharacterType) As CharacterType
        Return New CharacterType(oldCharacterType)
    End Function
    <Extension>
    Function ToOld(characterType As CharacterType) As OldCharacterType
        Return CType(characterType.Id, OldCharacterType)
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