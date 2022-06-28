Friend Class FinalLockDescriptor
    Inherits RouteTypeDescriptor
    Public Overrides ReadOnly Property Abbreviation As String
        Get
            Return "EO"
        End Get
    End Property
    Public Overrides ReadOnly Property UnlockItem As ItemType?
        Get
            Return ItemType.ElementalOrb
        End Get
    End Property
End Class
