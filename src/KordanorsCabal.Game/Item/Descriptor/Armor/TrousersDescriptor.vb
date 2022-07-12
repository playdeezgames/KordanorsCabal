Friend Class TrousersDescriptor
    Inherits ItemTypeDescriptor

    Public Overrides ReadOnly Property SpawnLocationTypes(level As Long) As HashSet(Of LocationType)
        Get
            Return New HashSet(Of LocationType)
        End Get
    End Property

    Public Overrides ReadOnly Property Name As String
        Get
            Return "Trousers"
        End Get
    End Property

    Public Overrides ReadOnly Property EquipSlot As EquipSlot?
        Get
            Return Game.EquipSlot.Legs
        End Get
    End Property
End Class
