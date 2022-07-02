﻿Imports System.Runtime.CompilerServices

Public Enum CharacterType
    None
    N00b
    Skeleton
    Goblin
End Enum
Module CharacterTypeExtensions
    <Extension>
    Function InitialStatistics(characterType As CharacterType) As IReadOnlyDictionary(Of CharacterStatisticType, Long)
        Return CharacterTypeDescriptors(characterType).InitialStatistics
    End Function
    <Extension>
    Function MaximumEncumbrance(characterType As CharacterType, character As Character) As Single
        Return CharacterTypeDescriptors(characterType).MaximumEncumbrance(character)
    End Function
    <Extension>
    Function IsEnemy(characterType As CharacterType, character As Character) As Boolean
        Return CharacterTypeDescriptors(characterType).IsEnemy(character)
    End Function
    <Extension>
    Function SpawnCount(characterType As CharacterType, level As Long) As Long
        Return CharacterTypeDescriptors(characterType).SpawnCount(level)
    End Function
    <Extension>
    Function CanSpawn(characterType As CharacterType, location As Location, level As Long) As Boolean
        Return CharacterTypeDescriptors(characterType).CanSpawn(location, level)
    End Function
    <Extension>
    Function Name(characterType As CharacterType) As String
        Return CharacterTypeDescriptors(characterType).Name
    End Function
    <Extension>
    Function RollMoneyDrop(characterType As CharacterType) As Long
        Return CharacterTypeDescriptors(characterType).RollMoneyDrop
    End Function
    <Extension>
    Function PartingShot(characterType As CharacterType) As String
        Return CharacterTypeDescriptors(characterType).PartingShot
    End Function
    <Extension>
    Function XPValue(characterType As CharacterType) As Long
        Return CharacterTypeDescriptors(characterType).XPValue
    End Function
    <Extension>
    Sub DropLoot(characterType As CharacterType, location As Location)
        CharacterTypeDescriptors(characterType).DropLoot(location)
    End Sub
End Module
