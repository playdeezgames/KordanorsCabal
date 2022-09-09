Public Class ItemTypeSpawnCountDataTests
    Inherits WorldDataSubobjectTests(Of IItemTypeSpawnCountData)
    Sub New()
        MyBase.New(Function(x) x.ItemTypeSpawnCount)
    End Sub
    <Fact>
    Sub ShouldQueryTheStoreForTheSpawnCountOfAGivenCombinationOfItemTypeAndDungeonLevel()
        WithSubobject(
            Sub(store, subject)
                Dim itemTypeId = 1L
                Dim dungeonLevelId = 2L
                subject.Read(itemTypeId, dungeonLevelId).ShouldBeNull
                'IStore.ReadColumnString<long, long>(Action, "ItemTypeSpawnCounts", "SpawnDice", (ItemTypeId, 1), (DungeonLevelId, 2))
                store.Verify(
                    Function(x) x.ReadColumnString(Of Long, Long)(
                    It.IsAny(Of Action),
                    Tables.ItemTypeSpawnCounts,
                    Columns.SpawnDiceColumn,
                    (Columns.ItemTypeIdColumn, itemTypeId),
                    (Columns.DungeonLevelIdColumn, dungeonLevelId)))
            End Sub)
    End Sub
End Class
