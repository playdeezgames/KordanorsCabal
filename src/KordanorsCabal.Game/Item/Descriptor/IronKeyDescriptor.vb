Friend Class IronKeyDescriptor
    Inherits ItemTypeDescriptor

    Public Overrides ReadOnly Property SpawnLocationTypes As HashSet(Of LocationType)
        Get
            Return New HashSet(Of LocationType) From {LocationType.Dungeon}
        End Get
    End Property

    Public Overrides ReadOnly Property Name As String
        Get
            Return "FE KEY"
        End Get
    End Property
End Class
