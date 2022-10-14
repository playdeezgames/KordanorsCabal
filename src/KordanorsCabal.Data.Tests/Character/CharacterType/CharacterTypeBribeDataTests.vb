Public Class CharacterTypeBribeDataTests
    Inherits WorldDataSubobjectTests(Of ICharacterTypeBribeData)

    Public Sub New()
        MyBase.New(Function(x) x.CharacterTypeBribe)
    End Sub

    <Fact>
    Sub ShouldQueryTheStoreToSeeWhenAnItemTypeCanBribeACharacterType()
        WithSubobject(Sub(store, checker, subject)
                          Dim characterTypeId = 1L
                          Dim itemType = 2L
                          store.SetupGet(Function(x) x.Column).Returns((New Mock(Of IStoreColumn)).Object)
                          subject.Read(characterTypeId, itemType).ShouldBeFalse
                          store.Verify(Function(x) x.Column.ReadColumnValue(Of Long, Long, Long)(
                                       It.IsAny(Of Action),
                                       Tables.CharacterTypeBribes,
                                       Columns.ItemTypeIdColumn,
                                       (Columns.CharacterTypeIdColumn, characterTypeId),
                                       (Columns.ItemTypeIdColumn, itemType)))
                      End Sub)
    End Sub
End Class
