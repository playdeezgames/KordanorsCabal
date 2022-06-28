Friend Class GoldLockDescriptor
    Inherits RouteTypeDescriptor

    Public Overrides ReadOnly Property Abbreviation As String
        Get
            Return "AU"
        End Get
    End Property
    Public Overrides ReadOnly Property UnlockItem As ItemType?
        Get
            Return ItemType.GoldKey
        End Get
    End Property
    Public Overrides ReadOnly Property UnlockedRouteType As RouteType?
        Get
            Return RouteType.Passageway
        End Get
    End Property
End Class
