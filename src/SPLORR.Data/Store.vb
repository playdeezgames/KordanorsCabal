Public Class Store
    Inherits StoreBase
    Implements IStore
    Private templateFilename As String
    Public Sub New(filename As String)
        MyBase.New(New Backer)
        Me.templateFilename = filename
    End Sub
    Private ReadOnly Property LastInsertRowId() As Long
        Get
            Return backer.LastInsertRowId
        End Get
    End Property

    Public ReadOnly Property Count As IStoreCount Implements IStore.Count
        Get
            Return New StoreCount(backer)
        End Get
    End Property

    Public ReadOnly Property Primitive As IStorePrimitive Implements IStore.Primitive
        Get
            Return New StorePrimitive(backer)
        End Get
    End Property

    Public ReadOnly Property Clear As IStoreClear Implements IStore.Clear
        Get
            Return New StoreClear(backer)
        End Get
    End Property

    Public ReadOnly Property Meta As IStoreMeta Implements IStore.Meta
        Get
            Return New StoreMeta(backer, templateFilename)
        End Get
    End Property

    Public ReadOnly Property Column As IStoreColumn Implements IStore.Column
        Get
            Return New StoreColumn(backer)
        End Get
    End Property

    Public ReadOnly Property Create As IStoreCreate Implements IStore.Create
        Get
            Return New StoreCreate(backer)
        End Get
    End Property

    Public ReadOnly Property Replace As IStoreReplace Implements IStore.Replace
        Get
            Return New StoreReplace(backer)
        End Get
    End Property

    Public Function ReadRecords(Of TOutputColumn)(initializer As Action, tableName As String, outputColumnName As String) As List(Of TOutputColumn) Implements IStore.ReadRecords
        initializer()
        Return ExecuteReader(
            Function(reader) CType(reader(outputColumnName), TOutputColumn),
            $"SELECT [{outputColumnName}] FROM [{tableName}];")
    End Function
    Public Function ReadRecordsWithColumnValue(Of TInputColumn, TOutputColumn)(initializer As Action, tableName As String, outputColumnName As String, forColumnValue As (String, TInputColumn)) As List(Of TOutputColumn) Implements IStore.ReadRecordsWithColumnValue
        initializer()
        Return ExecuteReader(
            Function(reader) CType(reader(outputColumnName), TOutputColumn),
            $"SELECT [{outputColumnName}] FROM [{tableName}] WHERE [{forColumnValue.Item1}]=@{forColumnValue.Item1};",
            ($"@{forColumnValue.Item1}", forColumnValue.Item2))
    End Function
    Public Function ReadRecordsWithColumnValues(Of TFirstInputColumn, TSecondInputColumn, TOutputColumn)(initializer As Action, tableName As String, outputColumnName As String, firstColumnValue As (String, TFirstInputColumn), secondColumnValue As (String, TSecondInputColumn)) As List(Of TOutputColumn) Implements IStore.ReadRecordsWithColumnValues
        initializer()
        Return ExecuteReader(
            Function(reader) CType(reader(outputColumnName), TOutputColumn),
            $"SELECT 
                [{outputColumnName}] 
            FROM [{tableName}] 
            WHERE 
                [{firstColumnValue.Item1}]=@{firstColumnValue.Item1} 
                AND [{secondColumnValue.Item1}]=@{secondColumnValue.Item1};",
            ($"@{firstColumnValue.Item1}", firstColumnValue.Item2),
            ($"@{secondColumnValue.Item1}", secondColumnValue.Item2))
    End Function
    Public Function ReadRecordsWithColumnValue(
            Of TInputColumn,
                TFirstOutputColumn,
                TSecondOutputColumn)(
                    initializer As Action,
                    tableName As String,
                    outputColumnNames As (String, String),
                    forColumnValue As (String, TInputColumn)) As List(Of Tuple(Of TFirstOutputColumn, TSecondOutputColumn)) Implements IStore.ReadRecordsWithColumnValue
        initializer()
        Return ExecuteReader(
            Function(reader) New Tuple(Of TFirstOutputColumn, TSecondOutputColumn)(CType(reader(outputColumnNames.Item1), TFirstOutputColumn), CType(reader(outputColumnNames.Item2), TSecondOutputColumn)),
            $"SELECT [{outputColumnNames.Item1}],[{outputColumnNames.Item2}] FROM [{tableName}] WHERE [{forColumnValue.Item1}]=@{forColumnValue.Item1};",
            ($"@{forColumnValue.Item1}", forColumnValue.Item2))
    End Function
End Class
