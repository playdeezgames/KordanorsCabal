Public Class InventoryDataShould
    Inherits WorldDataSubobjectTests(Of IInventoryData)
    Sub New()
        MyBase.New(Function(x) x.Inventory)
    End Sub
    <Fact>
    Sub ClearInventoryDataForAGivenCharacter()
        WithSubobject(
            Sub(store, checker, subject)
                Dim characterId = 1L
                store.Setup(Sub(x) x.Clear.ForValue(It.IsAny(Of Action), It.IsAny(Of String), It.IsAny(Of (String, Long))))
                subject.ClearForCharacter(characterId)
                store.Verify(
                Sub(x) x.Clear.ForValue(
                    It.IsAny(Of Action),
                    Tables.Inventories,
                    (Columns.CharacterIdColumn, characterId)))
            End Sub)
    End Sub
    <Fact>
    Sub CreateInventoryForAGivenCharacter()
        WithSubobject(
            Sub(store, checker, subject)
                Dim characterId = 1L
                store.SetupGet(Function(x) x.Create).Returns((New Mock(Of IStoreCreate)).Object)
                subject.CreateForCharacter(characterId).ShouldBe(0)
                store.Verify(
                Function(x) x.Create.Entry(
                    It.IsAny(Of Action),
                    Tables.Inventories,
                    (Columns.CharacterIdColumn, characterId)))
            End Sub)
    End Sub
    <Fact>
    Sub CreateInventoryForAGivenLocation()
        WithSubobject(
            Sub(store, checker, subject)
                Dim locationId = 1L
                store.SetupGet(Function(x) x.Create).Returns((New Mock(Of IStoreCreate)).Object)
                subject.CreateForLocation(locationId).ShouldBe(0)
                store.Verify(
                Function(x) x.Create.Entry(
                    It.IsAny(Of Action),
                    Tables.Inventories,
                    (Columns.LocationIdColumn, locationId)))
            End Sub)
    End Sub
    <Fact>
    Sub QueryTheStoreForInventoryDataForAGivenCharacter()
        WithSubobject(
            Sub(store, checker, subject)
                Dim characterId = 1L
                store.SetupGet(Function(x) x.Column).Returns((New Mock(Of IStoreColumn)).Object)
                subject.ReadForCharacter(characterId).ShouldBeNull
                store.Verify(
                Sub(x) x.Column.ReadValue(Of Long, Long)(
                    It.IsAny(Of Action),
                    Tables.Inventories,
                    Columns.InventoryIdColumn,
                    (Columns.CharacterIdColumn, characterId)))
            End Sub)
    End Sub
    <Fact>
    Sub QueryTheStoreForInventoryDataForAGivenLocation()
        WithSubobject(
            Sub(store, checker, subject)
                Dim characterId = 1L
                store.SetupGet(Function(x) x.Column).Returns((New Mock(Of IStoreColumn)).Object)
                subject.ReadForLocation(characterId).ShouldBeNull
                store.Verify(
                    Function(x) x.Column.ReadValue(Of Long, Long)(
                        It.IsAny(Of Action),
                        Tables.Inventories,
                        Columns.InventoryIdColumn,
                        (Columns.LocationIdColumn, characterId)))
            End Sub)
    End Sub
    <Fact>
    Sub fetch_location_associated_with_inventory()
        WithSubobject(
            Sub(store, checker, subject)
                Const inventoryId = 1L
                store.SetupGet(Function(x) x.Column).Returns((New Mock(Of IStoreColumn)).Object)
                subject.ReadLocation(inventoryId).ShouldBeNull
                store.Verify(
                    Function(x) x.Column.ReadValue(Of Long, Long)(
                        It.IsAny(Of Action),
                        Tables.Inventories,
                        Columns.LocationIdColumn,
                        (Columns.InventoryIdColumn, inventoryId)))
            End Sub)
    End Sub
End Class
