Public Class DungeonLevelDataTests
    Inherits WorldDataSubobjectTests(Of IDungeonLevelData)
    Sub New()
        MyBase.New(Function(x) x.DungeonLevel)
    End Sub
    <Fact>
    Sub ShouldReadAllDungeonLevelsFromTheStore()
        WithSubobject(
            Sub(store, checker, subject)
                store.SetupGet(Function(x) x.Record).Returns((New Mock(Of IStoreRecord)).Object)
                subject.ReadAll().ShouldBeNull
                store.Verify(
                    Function(x) x.Record.ReadRecords(Of Long)(
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
                store.SetupGet(Function(x) x.Column).Returns((New Mock(Of IStoreColumn)).Object)
                subject.ReadName(dungeonLevelId).ShouldBeNull
                store.Verify(
                    Function(x) x.Column.ReadString(Of Long)(
                    It.IsAny(Of Action),
                    Tables.DungeonLevels,
                    Columns.DungeonLevelNameColumn,
                    (Columns.DungeonLevelIdColumn, dungeonLevelId)))
            End Sub)
    End Sub
End Class
