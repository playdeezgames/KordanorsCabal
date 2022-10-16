﻿Public Class QuestTypeData
    Inherits BaseData
    Implements IQuestTypeData
    Friend Const TableName = "QuestTypes"
    Friend Const QuestTypeIdColumn = "QuestTypeId"
    Friend Const CanAcceptEventNameColumn = "CanAcceptEventName"
    Friend Const AcceptEventNameColumn = "AcceptEventName"
    Friend Const CanCompleteEventNameColumn = "CanCompleteEventName"
    Friend Const CompleteEventNameColumn = "CompleteEventName"
    Public Sub New(store As IStore, world As IWorldData)
        MyBase.New(store, world)
    End Sub

    Public Function ReadCanAcceptEventName(questTypeId As Long) As String Implements IQuestTypeData.ReadCanAcceptEventName
        Return Store.Column.ReadString(AddressOf NoInitializer, TableName, CanAcceptEventNameColumn, (QuestTypeIdColumn, questTypeId))
    End Function

    Public Function ReadCanCompleteEventName(questTypeId As Long) As String Implements IQuestTypeData.ReadCanCompleteEventName
        Return Store.Column.ReadString(AddressOf NoInitializer, TableName, CanCompleteEventNameColumn, (QuestTypeIdColumn, questTypeId))
    End Function

    Public Function ReadAcceptEventName(questTypeId As Long) As String Implements IQuestTypeData.ReadAcceptEventName
        Return Store.Column.ReadString(AddressOf NoInitializer, TableName, AcceptEventNameColumn, (QuestTypeIdColumn, questTypeId))
    End Function

    Public Function ReadCompleteEventName(questTypeId As Long) As String Implements IQuestTypeData.ReadCompleteEventName
        Return Store.Column.ReadString(AddressOf NoInitializer, TableName, CompleteEventNameColumn, (QuestTypeIdColumn, questTypeId))
    End Function

    Public Function ReadAll() As IEnumerable(Of Long) Implements IQuestTypeData.ReadAll
        Return Store.Record.All(Of Long)(AddressOf NoInitializer, TableName, QuestTypeIdColumn)
    End Function
End Class
