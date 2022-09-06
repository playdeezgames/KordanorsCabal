Public Class CharacterTypeInitialStatisticDataTests
    Inherits WorldDataSubobjectTests(Of ICharacterTypeInitialStatisticData)
    Sub New()
        MyBase.New(Function(x) x.CharacterTypeInitialStatistic)
    End Sub
    <Fact>
    Sub ShouldQueryTheStoreForInitialStatisticsOfACharacterType()
        WithSubobject(
            Sub(store, subject)
                Dim characterType = 1L
                subject.ReadAllForCharacterType(characterType).ShouldBeNull
                store.Verify(Function(x) x.ReadRecordsWithColumnValue(Of Long, Long, Long)(
                                 It.IsAny(Of Action),
                                 Tables.CharacterTypeInitialStatistics,
                                 (Columns.CharacterStatisticTypeIdColumn, Columns.InitialValueColumn),
                                 (Columns.CharacterTypeIdColumn, characterType)))
            End Sub)
    End Sub
End Class
