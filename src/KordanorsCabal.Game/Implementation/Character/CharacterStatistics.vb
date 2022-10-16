Public Class CharacterStatistics
    Inherits SubcharacterBase
    Implements ICharacterStatistics

    Public Sub New(worldData As IWorldData, character As ICharacter)
        MyBase.New(worldData, character)
    End Sub

    Public Shared Function FromCharacter(worldData As IWorldData, character As ICharacter) As ICharacterStatistics
        Return If(character IsNot Nothing, New CharacterStatistics(worldData, character), Nothing)
    End Function
    Public Sub SetStatistic(statisticType As ICharacterStatisticType, statisticValue As Long) Implements ICharacterStatistics.SetStatistic
        WorldData.CharacterStatistic.Write(Id, statisticType.Id, Math.Min(Math.Max(statisticValue, statisticType.MinimumValue), statisticType.MaximumValue))
    End Sub
    Sub ChangeStatistic(statisticType As ICharacterStatisticType, delta As Long) Implements ICharacterStatistics.ChangeStatistic
        Dim current = GetStatistic(statisticType)
        If current IsNot Nothing Then
            SetStatistic(statisticType, current.Value + delta)
        End If
    End Sub
    Public Function GetStatistic(statisticType As ICharacterStatisticType) As Long? Implements ICharacterStatistics.GetStatistic
        Dim result = If(WorldData.CharacterStatistic.Read(Id,
                                                          statisticType.Id), statisticType.DefaultValue)
        If result.HasValue Then
            For Each item In EquippedItems
                Dim buff As Long = If(item.Equipment.EquippedBuff(statisticType), 0)
                result = result.Value + buff
            Next
        End If
        Return result
    End Function
End Class
