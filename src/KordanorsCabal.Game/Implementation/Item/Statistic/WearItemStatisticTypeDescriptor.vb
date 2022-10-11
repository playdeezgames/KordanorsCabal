Friend Class WearItemStatisticTypeDescriptor
    Inherits ItemStatisticType

    Public Sub New(worldData As IWorldData, id As Long)
        MyBase.New(worldData, id)
    End Sub

    Public Overrides ReadOnly Property DefaultValue As Long
        Get
            Return 0
        End Get
    End Property
End Class
