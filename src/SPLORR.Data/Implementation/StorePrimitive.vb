Public Class StorePrimitive
    Inherits StoreBase
    Implements IStorePrimitive

    Public Sub New(backer As IBacker)
        MyBase.New(backer)
    End Sub

    Public Sub Execute(sql As String, ParamArray parameters() As (String, Object)) Implements IStorePrimitive.Execute
        backer.Execute(sql, parameters)
    End Sub
End Class
