Friend Class PlatinumKeyDescriptor
    Inherits ItemTypeDescriptor

    Public Overrides ReadOnly Property SpawnLocationTypes As HashSet(Of LocationType)
        Get
            Return New HashSet(Of LocationType) From {LocationType.DungeonDeadEnd}
        End Get
    End Property

    Public Overrides ReadOnly Property Name As String
        Get
            Return "PT KEY"
        End Get
    End Property
End Class
