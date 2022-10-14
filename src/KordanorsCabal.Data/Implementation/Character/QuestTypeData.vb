Public Class QuestTypeData
    Inherits BaseData
    Implements IQuestTypeData
    Friend Const TableName = "QuestTypes"
    Friend Const QuestTypeIdColumn = "QuestTypeId"
    Friend Const CanAcceptEventNameColumn = "CanAcceptEventName"
    Friend Const AcceptEventNameColumn = "AcceptEventName"
    Friend Const CanCompleteEventNameColumn = "CanCompleteEventName"
    Friend Const CompleteEventNameColumn = "CompleteEventName"

    Friend Sub Initialize()
        Store.Primitive.ExecuteNonQuery($"CREATE TABLE IF NOT EXISTS [{TableName}] AS
                WITH [cte](
                    [{QuestTypeIdColumn}],
                    [{CanAcceptEventNameColumn}],
                    [{AcceptEventNameColumn}],
                    [{CanCompleteEventNameColumn}],
                    [{CompleteEventNameColumn}]) AS
                (VALUES
                    (1,'CharacterCanAcceptCellarRatsQuest','CharacterAcceptCellarRatsQuest','CharacterCanCompleteCellarRatsQuest','CharacterCompleteCellarRatsQuest'))
                SELECT 
                    [{QuestTypeIdColumn}],
                    [{CanAcceptEventNameColumn}],
                    [{AcceptEventNameColumn}],
                    [{CanCompleteEventNameColumn}],
                    [{CompleteEventNameColumn}]
                FROM [cte];")
    End Sub

    Public Sub New(store As IStore, world As IWorldData)
        MyBase.New(store, world)
    End Sub

    Public Function ReadCanAcceptEventName(questTypeId As Long) As String Implements IQuestTypeData.ReadCanAcceptEventName
        Return Store.Column.ReadColumnString(AddressOf Initialize, TableName, CanAcceptEventNameColumn, (QuestTypeIdColumn, questTypeId))
    End Function

    Public Function ReadCanCompleteEventName(questTypeId As Long) As String Implements IQuestTypeData.ReadCanCompleteEventName
        Return Store.Column.ReadColumnString(AddressOf Initialize, TableName, CanCompleteEventNameColumn, (QuestTypeIdColumn, questTypeId))
    End Function

    Public Function ReadAcceptEventName(questTypeId As Long) As String Implements IQuestTypeData.ReadAcceptEventName
        Return Store.Column.ReadColumnString(AddressOf Initialize, TableName, AcceptEventNameColumn, (QuestTypeIdColumn, questTypeId))
    End Function

    Public Function ReadCompleteEventName(questTypeId As Long) As String Implements IQuestTypeData.ReadCompleteEventName
        Return Store.Column.ReadColumnString(AddressOf Initialize, TableName, CompleteEventNameColumn, (QuestTypeIdColumn, questTypeId))
    End Function

    Public Function ReadAll() As IEnumerable(Of Long) Implements IQuestTypeData.ReadAll
        Return Store.Record.ReadRecords(Of Long)(AddressOf Initialize, TableName, QuestTypeIdColumn)
    End Function
End Class
