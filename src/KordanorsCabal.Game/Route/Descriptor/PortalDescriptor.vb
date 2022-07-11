Friend Class PortalDescriptor
    Inherits RouteTypeDescriptor
    Public Overrides ReadOnly Property IsSingleUse As Boolean
        Get
            Return True
        End Get
    End Property
End Class
