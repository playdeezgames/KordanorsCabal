Public Class DungeonLevelDataTests
    Inherits WorldDataSubobjectTests(Of IDungeonLevelData)
    Sub New()
        MyBase.New(Function(x) x.DungeonLevel)
    End Sub
    <Fact>
    Sub ShouldReadAllDungeonLevelsFromTheStore()
        WithSubobject(
            Sub(store, subject)
                subject.ReadAll().ShouldBeNull
                store.Verify(
                    Function(x) x.ReadRecords(Of Long)(
                    It.IsAny(Of Action),
                    Tables.DungeonLevels,
                    Columns.DungeonLevelIdColumn))
            End Sub)
    End Sub
End Class
