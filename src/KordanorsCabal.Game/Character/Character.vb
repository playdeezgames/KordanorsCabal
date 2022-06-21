﻿Public Class Character
    ReadOnly Property Id As Long
    Sub New(characterId As Long)
        Id = characterId
    End Sub
    ReadOnly Property CharacterType As CharacterType
        Get
            Return CType(CharacterData.ReadCharacterType(Id), CharacterType)
        End Get
    End Property

    Friend Shared Function Create(characterType As CharacterType, location As Location) As Character
        Dim character = FromId(CharacterData.Create(characterType, location.Id))
        For Each entry In characterType.InitialStatistics
            character.SetStatistic(entry.Key, entry.Value)
        Next
        Return character
    End Function

    Friend Sub SetStatistic(statisticType As StatisticType, statisticValue As Long)
        CharacterStatisticData.Write(Id, statisticType, statisticValue)
    End Sub

    Friend Sub ChangeStatistic(statisticType As StatisticType, delta As Long)
        SetStatistic(statisticType, GetStatistic(statisticType) + delta)
    End Sub

    ReadOnly Property Location As Location
        Get
            Return Location.FromId(CharacterData.ReadLocation(Id).Value)
        End Get
    End Property
    Shared Function FromId(characterId As Long) As Character
        Return New Character(characterId)
    End Function

    Public Function GetStatistic(statisticType As StatisticType) As Long
        Return If(CharacterStatisticData.Read(Id, statisticType), statisticType.DefaultValue)
    End Function
End Class