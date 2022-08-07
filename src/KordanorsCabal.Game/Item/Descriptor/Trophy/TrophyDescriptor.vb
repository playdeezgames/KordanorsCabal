Friend Class TrophyDescriptor
    Inherits ItemTypeDescriptor
    Sub New(
           name As String,
           offer As Long,
           boughtAt As IReadOnlyList(Of ShoppeType))
        MyBase.New(offer, boughtAt)
        Me.Name = name
    End Sub

    Public Overrides ReadOnly Property SpawnLocationTypes(level As Long) As HashSet(Of LocationType)
        Get
            Return New HashSet(Of LocationType)
        End Get
    End Property

    Public Overrides ReadOnly Property Name As String
End Class
