Friend Class IronKeyDescriptor
    Inherits ItemTypeDescriptor

    Public Overrides ReadOnly Property SpawnLocationTypes(level as DungeonLevel) As HashSet(Of LocationType)
        Get
            Return New HashSet(Of LocationType) From {LocationType.Dungeon}
        End Get
    End Property

    Sub New()
        MyBase.New("FE Key")
    End Sub
End Class
