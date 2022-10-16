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
                store.SetupGet(Function(x) x.Record).Returns((New Mock(Of IStoreRecord)).Object)
                subject.Read(characterType).ShouldBeNull
                store.Verify(Function(x) x.Record.WithValue(Of Long, Long?, Long)(
                                 It.IsAny(Of Action),
                                 Tables.CharacterTypeLoots,
                                 (Columns.ItemTypeIdColumn, Columns.WeightColumn),
                                 (Columns.CharacterTypeIdColumn, characterType)))
            End Sub)
    End Sub
End Class
