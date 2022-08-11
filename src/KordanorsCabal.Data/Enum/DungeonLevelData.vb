Public Class DungeonLevelData
    Inherits BaseData
    Friend Const TableName = "DungeonLevels"
    Friend Const DungeonLevelIdColumn = "DungeonLevelId"
    Friend Const DungeonLevelNameColumn = "DungeonLevelName"
    Friend Sub Initialize()
        Store.ExecuteNonQuery($"CREATE TABLE IF NOT EXISTS [{TableName}] AS
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

    Public Function ReadAll() As IEnumerable(Of Long)
        Return Store.ReadRecords(Of Long)(
            AddressOf Initialize,
            TableName,
            DungeonLevelIdColumn)
    End Function
    Private ReadOnly nameLookUp As New Dictionary(Of String, Long)
    Public Function ReadForName(name As String) As Long?
        Dim candidate As Long = 0
        If nameLookUp.TryGetValue(name, candidate) Then
            Return candidate
        End If
        Dim result = Store.ReadColumnValue(Of String, Long)(
            AddressOf Initialize,
            TableName,
            DungeonLevelIdColumn,
            (DungeonLevelNameColumn, name))
        If result.HasValue Then
            nameLookUp(name) = result.Value
        End If
        Return result
    End Function

    Public Sub New(store As Store)
        MyBase.New(store)
    End Sub

    Public Function ReadName(dungeonLevelId As Long) As String
        Return Store.ReadColumnString(
            AddressOf Initialize,
            TableName,
            DungeonLevelNameColumn,
            (DungeonLevelIdColumn, dungeonLevelId))
    End Function
End Class
