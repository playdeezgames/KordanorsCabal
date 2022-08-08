Friend Class LotionDescriptor
    Inherits ItemTypeDescriptor

    Public Overrides ReadOnly Property SpawnLocationTypes(level as DungeonLevel) As HashSet(Of LocationType)
        Get
            Return New HashSet(Of LocationType)
        End Get
    End Property

    Sub New()
        MyBase.New("Lotion")
    End Sub

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
