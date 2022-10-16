Public Class DungeonLevelData
    Inherits BaseData
    Implements IDungeonLevelData
    Friend Const TableName = "DungeonLevels"
    Friend Const DungeonLevelIdColumn = "DungeonLevelId"
    Friend Const DungeonLevelNameColumn = "DungeonLevelName"
    Public Function ReadAll() As IEnumerable(Of Long) Implements IDungeonLevelData.ReadAll
        Return Store.Record.All(Of Long)(
            AddressOf NoInitializer,
            TableName,
            DungeonLevelIdColumn)
    End Function
    Public Sub New(store As IStore, world As IWorldData)
        MyBase.New(store, world)
    End Sub

    Public Function ReadName(dungeonLevelId As Long) As String Implements IDungeonLevelData.ReadName
        Return Store.Column.ReadString(
            AddressOf NoInitializer,
            TableName,
            DungeonLevelNameColumn,
            (DungeonLevelIdColumn, dungeonLevelId))
    End Function
End Class
