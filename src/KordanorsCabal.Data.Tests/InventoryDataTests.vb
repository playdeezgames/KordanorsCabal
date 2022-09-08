Public Class InventoryDataTests
    Inherits WorldDataSubobjectTests(Of IInventoryData)
    Sub New()
        MyBase.New(Function(x) x.Inventory)
    End Sub
    <Fact>
    Sub ShouldClearDataForAGivenCharacter()
        WithSubobject(
            Sub(store, subject)
                Dim characterId = 1L
                subject.ClearForCharacter(characterId)
                store.Verify(
                Sub(x) x.ClearForColumnValue(
                    It.IsAny(Of Action),
                    Tables.Inventories,
                    (Columns.CharacterIdColumn, characterId)))
            End Sub)
    End Sub
End Class
