Friend Class CopperKeyDescriptor
    Inherits ItemTypeDescriptor

    Public Overrides ReadOnly Property SpawnLocationTypes(level as DungeonLevel) As HashSet(Of LocationType)
        Get
            Return New HashSet(Of LocationType) From {LocationType.DungeonDeadEnd}
        End Get
    End Property

    Sub New()
        MyBase.New("CU Key")
    End Sub
End Class
