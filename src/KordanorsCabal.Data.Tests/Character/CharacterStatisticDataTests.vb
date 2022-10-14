Public Class CharacterStatisticDataTests
    Inherits WorldDataSubobjectTests(Of ICharacterStatisticData)
    Public Sub New()
        MyBase.New(Function(x) x.CharacterStatistic)
    End Sub
    <Fact>
    Sub ShouldRemoveAGivenCharactersStatisticsFromTheStore()
        WithSubobject(
            Sub(store, checker, subject)
                Dim characterId = 1L
                store.Setup(Sub(x) x.Clear.ClearForColumnValue(It.IsAny(Of Action), It.IsAny(Of String), It.IsAny(Of (String, Long))))
                subject.ClearForCharacter(characterId)
                store.Verify(Sub(x) x.Clear.ClearForColumnValue(
                             It.IsAny(Of Action),
                             Tables.CharacterStatistics,
                             (CharacterIdColumn, characterId)))
            End Sub)
    End Sub
    <Fact>
    Sub ShouldQueryTheStoreForAGivenCharactersStatisticValue()
        WithSubobject(
            Sub(store, checker, subject)
                Dim characterId = 1L
                Dim statisticType = 2L
                store.SetupGet(Function(x) x.Column).Returns((New Mock(Of IStoreColumn)).Object)
                subject.Read(characterId, statisticType).ShouldBeNull
                store.Verify(Sub(x) x.Column.ReadColumnValue(Of Long, Long, Long)(
                             It.IsAny(Of Action),
                             Tables.CharacterStatistics,
                             Columns.StatisticValueColumn,
                             (CharacterIdColumn, characterId),
                             (CharacterStatisticTypeIdColumn, statisticType)))
            End Sub)
    End Sub
    <Fact>
    Sub ShouldUpdateTheStoreWithAStatisticValueForACharacter()
        WithSubobject(
            Sub(store, checker, subject)
                Dim characterId = 1L
                Dim statisticType = 2L
                Dim statisticValue = 3L
                store.SetupGet(Function(x) x.Replace).Returns((New Mock(Of IStoreReplace)).Object)
                subject.Write(characterId, statisticType, statisticValue)
                store.Verify(Sub(x) x.Replace.ReplaceRecord(
                             It.IsAny(Of Action),
                             Tables.CharacterStatistics,
                             (CharacterIdColumn, characterId),
                             (CharacterStatisticTypeIdColumn, statisticType),
                             (StatisticValueColumn, statisticValue)))
            End Sub)
    End Sub
End Class
