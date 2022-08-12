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
    Function InitialStatistics(characterType As OldCharacterType) As IReadOnlyDictionary(Of Long, Long)
        Return CharacterTypeDescriptors(characterType).InitialStatistics
    End Function
    <Extension>
    Function MaximumEncumbrance(characterType As OldCharacterType, character As Character) As Long
        Return CharacterTypeDescriptors(characterType).MaximumEncumbrance(character)
    End Function
    <Extension>
    Function IsEnemy(characterType As OldCharacterType, character As Character) As Boolean
        Return CharacterTypeDescriptors(characterType).IsEnemy(character)
    End Function
    <Extension>
    Function SpawnCount(characterType As OldCharacterType, level As DungeonLevel) As Long
        Return CharacterTypeDescriptors(characterType).SpawnCount(level)
    End Function
    <Extension>
    Function CanSpawn(characterType As OldCharacterType, location As Location, level As DungeonLevel) As Boolean
        Return CharacterTypeDescriptors(characterType).CanSpawn(location, level)
    End Function
    <Extension>
    Function Name(characterType As OldCharacterType) As String
        Return CharacterTypeDescriptors(characterType).Name
    End Function
    <Extension>
    Function RollMoneyDrop(characterType As OldCharacterType) As Long
        Return CharacterTypeDescriptors(characterType).RollMoneyDrop
    End Function
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
    Function GenerateAttackType(characterType As OldCharacterType, character As Character) As AttackType
        Return CharacterTypeDescriptors(characterType).GenerateAttackType(character)
    End Function
End Module
