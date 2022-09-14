Public Class CharacterTypeLootDataTests
    Inherits WorldDataSubobjectTests(Of ICharacterTypeLootData)
    Sub New()
        MyBase.New(Function(x) x.CharacterTypeLoot)
    End Sub
    <Fact>
    Sub ShouldQueryTheStoreForLootDataAssociatedWithACharacterType()
        WithSubobject(
            Sub(store, checker, subject)
                Dim characterType = 1L
                subject.Read(characterType).ShouldBeNull
                'IStore.ReadRecordsWithColumnValue<long, long, long>(Action, "CharacterTypeLoots", (ItemTypeId, Weight), (CharacterTypeId, 1))
                store.Verify(Function(x) x.ReadRecordsWithColumnValue(Of Long, Long, Long)(
                                 It.IsAny(Of Action),
                                 Tables.CharacterTypeLoots,
                                 (Columns.ItemTypeIdColumn, Columns.WeightColumn),
                                 (Columns.CharacterTypeIdColumn, characterType)))
            End Sub)
    End Sub
End Class
