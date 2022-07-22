﻿Friend Class LotionDescriptor
    Inherits ItemTypeDescriptor

    Public Overrides ReadOnly Property SpawnLocationTypes(level As Long) As HashSet(Of LocationType)
        Get
            Return New HashSet(Of LocationType)
        End Get
    End Property

    Public Overrides ReadOnly Property Name As String
        Get
            Return "Lotion"
        End Get
    End Property

    Public Overrides ReadOnly Property MaximumDurability As Long?
        Get
            Return 5
        End Get
    End Property

    Public Overrides ReadOnly Property Encumbrance As Single
        Get
            Return 2
        End Get
    End Property
End Class
