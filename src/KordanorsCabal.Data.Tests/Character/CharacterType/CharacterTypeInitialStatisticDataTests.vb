Public Class CharacterTypeInitialStatisticDataTests
    Inherits WorldDataSubobjectTests(Of ICharacterTypeInitialStatisticData)
    Sub New()
        MyBase.New(Function(x) x.CharacterTypeInitialStatistic)
    End Sub
    <Fact>
    Sub ShouldQueryTheStoreForInitialStatisticsOfACharacterType()
        WithSubobject(
            Sub(store, checker, subject)
                Dim characterType = 1L
                store.SetupGet(Function(x) x.Record).Returns((New Mock(Of IStoreRecord)).Object)
                subject.ReadAllForCharacterType(characterType).ShouldBeNull
                store.Verify(Function(x) x.Record.WithValue(Of Long, Long, Long)(
                                 It.IsAny(Of Action),
                                 Tables.CharacterTypeInitialStatistics,
                                 (Columns.CharacterStatisticTypeIdColumn, Columns.InitialValueColumn),
                                 (Columns.CharacterTypeIdColumn, characterType)))
            End Sub)
    End Sub
End Class
