Public Interface IStoreMeta

    Sub Reset()
    Function Renew() As IBacker
    Sub Restore(
               oldBacker As IBacker)
    Sub ShutDown()
    Sub Save(
            filename As String)
    Sub Load(
            filename As String)
End Interface
