Public Class DungeonLevelData
    Inherits BaseData
    Implements IDungeonLevelData
    Friend Const TableName = "DungeonLevels"
    Friend Const DungeonLevelIdColumn = "DungeonLevelId"
    Friend Const DungeonLevelNameColumn = "DungeonLevelName"
    Friend Sub Initialize()
        Store.Primitive.Execute($"CREATE TABLE IF NOT EXISTS [{TableName}] AS
                WITH [cte](
                    [{DungeonLevelIdColumn}],
                    [{DungeonLevelNameColumn}]) AS
                (VALUES
                    (1,'Level I'),
                    (2,'Level II' ),
                    (3,'Level III'),
                    (4,'Level IV' ),
                    (5,'Level V'),
                    (6,'The Moon'))
                SELECT 
                    [{DungeonLevelIdColumn}],
                    [{DungeonLevelNameColumn}]
                FROM [cte];")
    End Sub

    Public Function ReadAll() As IEnumerable(Of Long) Implements IDungeonLevelData.ReadAll
        Return Store.Record.ReadRecords(Of Long)(
            AddressOf Initialize,
            TableName,
            DungeonLevelIdColumn)
    End Function
    Private ReadOnly nameLookUp As New Dictionary(Of String, Long)

    Public Sub New(store As IStore, world As IWorldData)
        MyBase.New(store, world)
    End Sub

    Public Function ReadName(dungeonLevelId As Long) As String Implements IDungeonLevelData.ReadName
        Return Store.Column.ReadString(
            AddressOf Initialize,
            TableName,
            DungeonLevelNameColumn,
            (DungeonLevelIdColumn, dungeonLevelId))
    End Function
End Class
