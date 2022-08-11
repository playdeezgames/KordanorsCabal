Public Class DirectionData
    Inherits NameCacheData
    Friend Const TableName = "Directions"
    Friend Const DirectionIdColumn = "DirectionId"
    Friend Const DirectionNameColumn = "DirectionName"
    Friend Const AbbreviationColumn = "Abbreviation"
    Friend Const IsCardinalColumn = "IsCardinal"
    Friend Const PreviousDirectionIdColumn = "PreviousDirectionId"
    Friend Const OppositeDirectionIdColumn = "OppositeDirectionId"
    Friend Const NextDirectionIdColumn = "NextDirectionId"
    Friend Sub Initialize()
        Store.ExecuteNonQuery(
            $"CREATE TABLE IF NOT EXISTS [{TableName}] AS
                WITH [cte](
                    [{DirectionIdColumn}],
                    [{DirectionNameColumn}],
                    [{AbbreviationColumn}],
                    [{IsCardinalColumn}],
                    [{PreviousDirectionIdColumn}],
                    [{OppositeDirectionIdColumn}],
                    [{NextDirectionIdColumn}]) AS
                (VALUES
                    (1,'North','N',1,4,3,2),
                    (2,'East' ,'E',1,1,4,3),
                    (3,'South','S',1,2,1,4),
                    (4,'West' ,'W',1,3,2,1),
                    (5,'Up','U',0,NULL,6,NULL),
                    (6,'Down','D',0,NULL,5,NULL),
                    (7,'In','In',0,NULL,8,NULL),
                    (8,'Out','Out',0,NULL,7,NULL))
                SELECT 
                    [{DirectionIdColumn}],
                    [{DirectionNameColumn}],
                    [{AbbreviationColumn}],
                    [{IsCardinalColumn}],
                    [{PreviousDirectionIdColumn}],
                    [{OppositeDirectionIdColumn}],
                    [{NextDirectionIdColumn}]
                FROM [cte];")
    End Sub

    Public Sub New(store As Store)
        MyBase.New(store)
        lookUpByName = Function(name) store.ReadColumnValue(Of String, Long)(
            AddressOf Initialize,
            TableName,
            DirectionIdColumn,
            (DirectionNameColumn, name))
    End Sub

    Public Function ReadName(directionId As Long) As String
        Return Store.ReadColumnString(
            AddressOf Initialize,
            TableName,
            DirectionNameColumn,
            (DirectionIdColumn, directionId))
    End Function
    Public Function ReadAbbreviation(directionId As Long) As String
        Return Store.ReadColumnString(
            AddressOf Initialize,
            TableName,
            AbbreviationColumn,
            (DirectionIdColumn, directionId))
    End Function

    Public Function ReadAll() As IEnumerable(Of Long)
        Return Store.ReadRecords(Of Long)(
            AddressOf Initialize,
            TableName,
            DirectionIdColumn)
    End Function

    Public Function ReadIsCardinal(directionId As Long) As Boolean
        Return If(Store.ReadColumnValue(Of Long, Long)(
            AddressOf Initialize,
            TableName,
            IsCardinalColumn,
            (DirectionIdColumn, directionId)), 0) > 0
    End Function
    Public Function ReadOpposite(directionId As Long) As Long?
        Return Store.ReadColumnValue(Of Long, Long)(
            AddressOf Initialize,
            TableName,
            OppositeDirectionIdColumn,
            (DirectionIdColumn, directionId))
    End Function
    Public Function ReadNext(directionId As Long) As Long?
        Return Store.ReadColumnValue(Of Long, Long)(
            AddressOf Initialize,
            TableName,
            NextDirectionIdColumn,
            (DirectionIdColumn, directionId))
    End Function
    Public Function ReadPrevious(directionId As Long) As Long?
        Return Store.ReadColumnValue(Of Long, Long)(
            AddressOf Initialize,
            TableName,
            PreviousDirectionIdColumn,
            (DirectionIdColumn, directionId))
    End Function
End Class
