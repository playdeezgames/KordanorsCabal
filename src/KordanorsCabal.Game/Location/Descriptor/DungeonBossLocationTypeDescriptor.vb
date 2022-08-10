Friend Class DungeonBossLocationTypeDescriptor
    Inherits LocationType

    Public Sub New()
        MyBase.New(6)
    End Sub

    Public Overrides ReadOnly Property IsDungeon As Boolean
        Get
            Return True
        End Get
    End Property

    Public Overrides ReadOnly Property CanMap As Boolean
        Get
            Return True
        End Get
    End Property

    Public Overrides ReadOnly Property RequiresMP As Boolean
        Get
            Return True
        End Get
    End Property
End Class
