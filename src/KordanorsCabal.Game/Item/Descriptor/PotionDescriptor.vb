﻿Friend Class PotionDescriptor
    Inherits ItemTypeDescriptor

    Public Overrides ReadOnly Property SpawnLocationTypes As HashSet(Of LocationType)
        Get
            Return New HashSet(Of LocationType) From
                {
                    LocationType.Dungeon
                }
        End Get
    End Property

    Public Overrides ReadOnly Property Name As String
        Get
            Return "Potion"
        End Get
    End Property

    Public Overrides ReadOnly Property Encumbrance As Single
        Get
            Return 1
        End Get
    End Property

    Public Overrides ReadOnly Property PurchasePrice As Long?
        Get
            Return 25
        End Get
    End Property
End Class
