Friend Class SilverLockDescriptor
    Inherits RouteTypeDescriptor

    Public Overrides ReadOnly Property Abbreviation As String
        Get
            Return "AG"
        End Get
    End Property
    Public Overrides ReadOnly Property UnlockItem As ItemType?
        Get
            Return ItemType.SilverKey
        End Get
    End Property
    Public Overrides ReadOnly Property UnlockedRouteType As RouteType?
        Get
            Return RouteType.Passageway
        End Get
    End Property
End Class
