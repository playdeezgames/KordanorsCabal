Friend Class CopperLockDescriptor
    Inherits RouteTypeDescriptor

    Public Overrides ReadOnly Property Abbreviation As String
        Get
            Return "CU"
        End Get
    End Property
    Public Overrides ReadOnly Property UnlockItem As ItemType?
        Get
            Return ItemType.CopperKey
        End Get
    End Property
End Class
