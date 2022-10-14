Public Class CharacterTypeAttackTypeDataTests
    Inherits WorldDataSubobjectTests(Of ICharacterTypeAttackTypeData)

    Public Sub New()
        MyBase.New(Function(x) x.CharacterTypeAttackType)
    End Sub
    <Fact>
    Sub ShouldQueryTheStoreForAnAttackTypeGeneratorForACharacterType()
        WithSubobject(
            Sub(store, checker, subject)
                Dim characterTypeId = 1L
                store.SetupGet(Function(x) x.Record).Returns((New Mock(Of IStoreRecord)).Object)
                subject.Read(characterTypeId).ShouldBeNull
                store.Verify(Function(x) x.Record.ReadRecordsWithColumnValue(Of Long, Long, Long)(
                                 It.IsAny(Of Action),
                                 Tables.CharacterTypeAttackTypes,
                                 (AttackTypeColumn, WeightColumn),
                                 (CharacterTypeIdColumn, characterTypeId)))
            End Sub)
    End Sub
End Class
