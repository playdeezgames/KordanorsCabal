Friend Class BlacksmithDescriptor
    Inherits FeatureTypeDescriptor

    Public Overrides ReadOnly Property Name As String
        Get
            Return "Samuli the Blacksmith"
        End Get
    End Property

    Public Overrides ReadOnly Property LocationType As LocationType
        Get
            Return LocationType.Town
        End Get
    End Property

    Public Overrides Function CanInteract(player As Character) As Boolean
        Return True
    End Function

    Public Overrides Function InteractionMode(player As Character) As PlayerMode
        Return PlayerMode.Blacksmith
    End Function
End Class
