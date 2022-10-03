Public Class InventoryDataTests
    Inherits WorldDataSubobjectTests(Of IInventoryData)
    Sub New()
        MyBase.New(Function(x) x.Inventory)
    End Sub
    <Fact>
    Sub ShouldClearInventoryDataForAGivenCharacter()
        WithSubobject(
            Sub(store, checker, subject)
                Dim characterId = 1L
                subject.ClearForCharacter(characterId)
                store.Verify(
                Sub(x) x.ClearForColumnValue(
                    It.IsAny(Of Action),
                    Tables.Inventories,
                    (Columns.CharacterIdColumn, characterId)))
            End Sub)
    End Sub
    <Fact>
    Sub ShouldCreateInventoryForAGivenCharacter()
        WithSubobject(
            Sub(store, checker, subject)
                Dim characterId = 1L
                subject.CreateForCharacter(characterId).ShouldBe(0)
                store.Verify(
                Sub(x) x.CreateRecord(
                    It.IsAny(Of Action),
                    Tables.Inventories,
                    (Columns.CharacterIdColumn, characterId)))
            End Sub)
    End Sub
    <Fact>
    Sub ShouldCreateInventoryForAGivenLocation()
        WithSubobject(
            Sub(store, checker, subject)
                Dim locationId = 1L
                subject.CreateForLocation(locationId).ShouldBe(0)
                store.Verify(
                Sub(x) x.CreateRecord(
                    It.IsAny(Of Action),
                    Tables.Inventories,
                    (Columns.LocationIdColumn, locationId)))
            End Sub)
    End Sub
    <Fact>
    Sub ShouldQueryTheStoreForInventoryDataForAGivenCharacter()
        WithSubobject(
            Sub(store, checker, subject)
                Dim characterId = 1L
                subject.ReadForCharacter(characterId).ShouldBeNull
                store.Verify(
                Sub(x) x.ReadColumnValue(Of Long, Long)(
                    It.IsAny(Of Action),
                    Tables.Inventories,
                    Columns.InventoryIdColumn,
                    (Columns.CharacterIdColumn, characterId)))
            End Sub)
    End Sub
    <Fact>
    Sub ShouldQueryTheStoreForInventoryDataForAGivenLocation()
        WithSubobject(
            Sub(store, checker, subject)
                Dim characterId = 1L
                subject.ReadForLocation(characterId).ShouldBeNull
                store.Verify(
                Sub(x) x.ReadColumnValue(Of Long, Long)(
                    It.IsAny(Of Action),
                    Tables.Inventories,
                    Columns.InventoryIdColumn,
                    (Columns.LocationIdColumn, characterId)))
            End Sub)
    End Sub
End Class
