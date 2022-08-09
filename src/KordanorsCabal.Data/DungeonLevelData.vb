Public Class DungeonLevelData
    Inherits BaseData
    Friend Const TableName = "DungeonLevels"
    Friend Const DungeonLevelIdColumn = "column1"
    Friend Const DungeonLevelNameColumn = "column2"
    Friend Sub Initialize()
        Store.ExecuteNonQuery(
            $"CREATE TABLE IF NOT EXISTS [{TableName}] AS
                WITH [cte]([{DungeonLevelIdColumn}] INT NOT NULL,[{DungeonLevelNameColumn}] TEXT NOT NULL) AS
                (
                    VALUES 
                        (1,'Level I'),
                        (2,'Level II'),
                        (3,'Level III'),
                        (4,'Level IV'),
                        (5,'Level V'),
                        (6,'The Moon')
                )
                SELECT [{DungeonLevelIdColumn}],[{DungeonLevelNameColumn}] FROM [cte]")
    End Sub

    Public Function ReadName(dungeonLevelId As Long) As String
        Return Store.ReadColumnString(
            AddressOf Initialize,
            TableName,
            DungeonLevelNameColumn,
            (DungeonLevelIdColumn, dungeonLevelId))
    End Function

    Public Sub New(store As Store)
        MyBase.New(store)
    End Sub

    Public ReadOnly Property All As IEnumerable(Of Long)
        Get
            Return Store.ReadColumnValues(Of Long)(AddressOf Initialize, TableName, DungeonLevelIdColumn)
        End Get
    End Property
End Class
