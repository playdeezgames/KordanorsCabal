Friend Class TownDescriptor
    Inherits LocationType

    Public Sub New()
        MyBase.New(2)
    End Sub

    Public Overrides ReadOnly Property IsDungeon As Boolean
        Get
            Return False
        End Get
    End Property

    Public Overrides ReadOnly Property CanMap As Boolean
        Get
            Return False
        End Get
    End Property

    Public Overrides ReadOnly Property RequiresMP As Boolean
        Get
            Return False
        End Get
    End Property
End Class
