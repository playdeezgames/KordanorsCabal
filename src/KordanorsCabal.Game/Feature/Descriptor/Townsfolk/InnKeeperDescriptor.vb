﻿Friend Class InnKeeperDescriptor
    Inherits FeatureTypeDescriptor

    Public Overrides ReadOnly Property Name As String
        Get
            Return "Graham the Innkeeper"
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
        Return PlayerMode.InnKeeper
    End Function
End Class
