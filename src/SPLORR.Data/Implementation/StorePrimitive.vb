Public Class StorePrimitive
    Inherits StoreBase
    Implements IStorePrimitive

    Public Sub New(backer As IBacker)
        MyBase.New(backer)
    End Sub

    Public Sub ExecuteNonQuery(sql As String, ParamArray parameters() As (String, Object)) Implements IStorePrimitive.ExecuteNonQuery
        backer.ExecuteNonQuery(sql, parameters)
    End Sub
End Class
