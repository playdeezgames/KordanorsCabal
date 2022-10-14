Public Interface IStorePrimitive
    Sub ExecuteNonQuery(
                       sql As String,
                       ParamArray parameters() As (String, Object))
End Interface
