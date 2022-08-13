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
Module CharacterTypeExtensions
    <Extension>
    Function PartingShot(characterType As OldCharacterType) As String
        Return CharacterTypeDescriptors(characterType).PartingShot
    End Function
    <Extension>
    Function XPValue(characterType As OldCharacterType) As Long
        Return CharacterTypeDescriptors(characterType).XPValue
    End Function
    <Extension>
    Sub DropLoot(characterType As OldCharacterType, location As Location)
        CharacterTypeDescriptors(characterType).DropLoot(location)
    End Sub
    <Extension>
    Function IsUndead(characterType As OldCharacterType) As Boolean
        Return CharacterTypeDescriptors(characterType).IsUndead
    End Function
    <Extension>
    Function CanBeBribedWith(characterType As OldCharacterType, itemType As ItemType) As Boolean
        Return CharacterTypeDescriptors(characterType).CanBeBribedWith(itemType)
    End Function
    <Extension>
    Function GenerateAttackType(characterType As OldCharacterType) As AttackType
        Return CharacterTypeDescriptors(characterType).GenerateAttackType
    End Function
End Module
Friend Module CharacterTypeUtility
    Friend ReadOnly CharacterTypeDescriptors As IReadOnlyDictionary(Of OldCharacterType, CharacterType) =
        New Dictionary(Of OldCharacterType, CharacterType) From
        {
            {OldCharacterType.Acolyte, New AcolyteDescriptor(OldCharacterType.Acolyte)},
            {OldCharacterType.Badger, New BadgerDescriptor(OldCharacterType.Badger)},
            {OldCharacterType.Bat, New BatDescriptor(OldCharacterType.Bat)},
            {OldCharacterType.Bishop, New BishopDescriptor(OldCharacterType.Bishop)},
            {OldCharacterType.CabalLeader, New CabalLeaderDescriptor(OldCharacterType.CabalLeader)},
            {OldCharacterType.Goblin, New GoblinDescriptor(OldCharacterType.Goblin)},
            {OldCharacterType.GoblinElite, New GoblinEliteDescriptor(OldCharacterType.GoblinElite)},
            {OldCharacterType.Kordanor, New KordanorDescriptor(OldCharacterType.Kordanor)},
            {OldCharacterType.Malcontent, New MalcontentDescriptor(OldCharacterType.Malcontent)},
            {OldCharacterType.MoonPerson, New MoonPersonDescriptor(OldCharacterType.MoonPerson)},
            {OldCharacterType.N00b, New N00bDescriptor(OldCharacterType.N00b)},
            {OldCharacterType.Priest, New PriestDescriptor(OldCharacterType.Priest)},
            {OldCharacterType.Rat, New RatDescriptor(OldCharacterType.Rat)},
            {OldCharacterType.Skeleton, New SkeletonDescriptor(OldCharacterType.Skeleton)},
            {OldCharacterType.Snake, New SnakeDescriptor(OldCharacterType.Snake)},
            {OldCharacterType.Zombie, New ZombieDescriptor(OldCharacterType.Zombie)}
        }
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