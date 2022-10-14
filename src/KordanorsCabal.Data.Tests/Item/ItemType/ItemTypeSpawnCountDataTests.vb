Public Class ItemTypeSpawnCountDataTests
    Inherits WorldDataSubobjectTests(Of IItemTypeSpawnCountData)
    Sub New()
        MyBase.New(Function(x) x.ItemTypeSpawnCount)
    End Sub
    <Fact>
    Sub ShouldQueryTheStoreForTheSpawnCountOfAGivenCombinationOfItemTypeAndDungeonLevel()
        WithSubobject(
            Sub(store, checker, subject)
                Dim itemTypeId = 1L
                Dim dungeonLevelId = 2L
                store.SetupGet(Function(x) x.Column).Returns((New Mock(Of IStoreColumn)).Object)
                subject.Read(itemTypeId, dungeonLevelId).ShouldBeNull
                store.Verify(
                    Function(x) x.Column.ReadColumnString(Of Long, Long)(
                    It.IsAny(Of Action),
                    Tables.ItemTypeSpawnCounts,
                    Columns.SpawnDiceColumn,
                    (Columns.ItemTypeIdColumn, itemTypeId),
                    (Columns.DungeonLevelIdColumn, dungeonLevelId)))
            End Sub)
    End Sub
End Class
