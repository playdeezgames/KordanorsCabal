Friend Class TownDrunkDescriptor
    Inherits FeatureTypeDescriptor

    Public Overrides ReadOnly Property Name As String
        Get
            Return "Yermom the Drunk"
        End Get
    End Property

    Public Overrides ReadOnly Property LocationType As LocationType
        Get
            Return LocationType.Town
        End Get
    End Property

    Public Overrides Function CanInteract(player As PlayerCharacter) As Boolean
        Return True
    End Function

    Public Overrides Function InteractionMode(player As PlayerCharacter) As PlayerMode
        Return PlayerMode.TownDrunk
    End Function
End Class
