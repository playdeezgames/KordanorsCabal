Public Interface IQuestTypeData
    Function ReadCanAcceptEventName(questTypeId As Long) As String
    Function ReadCanCompleteEventName(questTypeId As Long) As String
    Function ReadAcceptEventName(questTypeId As Long) As String
    Function ReadCompleteEventName(questTypeId As Long) As String
    Function ReadAll() As IEnumerable(Of Long)
End Interface
