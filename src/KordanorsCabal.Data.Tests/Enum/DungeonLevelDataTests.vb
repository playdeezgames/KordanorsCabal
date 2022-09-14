Public Class DungeonLevelDataTests
    Inherits WorldDataSubobjectTests(Of IDungeonLevelData)
    Sub New()
        MyBase.New(Function(x) x.DungeonLevel)
    End Sub
    <Fact>
    Sub ShouldReadAllDungeonLevelsFromTheStore()
        WithSubobject(
            Sub(store, checker, subject)
                subject.ReadAll().ShouldBeNull
                store.Verify(
                    Function(x) x.ReadRecords(Of Long)(
                    It.IsAny(Of Action),
                    Tables.DungeonLevels,
                    Columns.DungeonLevelIdColumn))
            End Sub)
    End Sub
    <Fact>
    Sub ShouldQueryTheDataStoreForTheNameOfADungeonLevel()
        WithSubobject(
            Sub(store, checker, subject)
                Dim dungeonLevelId = 1L
                subject.ReadName(dungeonLevelId).ShouldBeNull
                'IStore.ReadColumnString<long>(Action, "DungeonLevels", "DungeonLevelName", (DungeonLevelId, 1))
                store.Verify(
                    Function(x) x.ReadColumnString(Of Long)(
                    It.IsAny(Of Action),
                    Tables.DungeonLevels,
                    Columns.DungeonLevelNameColumn,
                    (Columns.DungeonLevelIdColumn, dungeonLevelId)))
            End Sub)
    End Sub
End Class
