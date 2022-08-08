Friend Class LotionDescriptor
    Inherits ItemTypeDescriptor

    Sub New()
        MyBase.New("Lotion", 2)
    End Sub

    Public Overrides ReadOnly Property MaximumDurability As Long?
        Get
            Return 5
        End Get
    End Property

End Class
