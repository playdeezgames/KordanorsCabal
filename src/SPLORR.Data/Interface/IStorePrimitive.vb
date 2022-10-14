Public Interface IStorePrimitive
    Sub Execute(
                       sql As String,
                       ParamArray parameters() As (String, Object))
End Interface
