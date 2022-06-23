Friend Class ElderDescriptor
    Inherits FeatureTypeDescriptor

    Public Overrides ReadOnly Property Name As String
        Get
            Return "Zooperdan the Elder"
        End Get
    End Property

    Public Overrides ReadOnly Property LocationType As LocationType
        Get
            Return LocationType.TownSquare
        End Get
    End Property

    Public Overrides Function CanInteract(player As PlayerCharacter) As Boolean
        Return True
    End Function

    Public Overrides Function InteractionMode(player As PlayerCharacter) As PlayerMode
        Return PlayerMode.Elder
    End Function
End Class
