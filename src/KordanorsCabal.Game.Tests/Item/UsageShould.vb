Public Class UsageShould
    Inherits ThingieShould(Of IUsage)
    Sub New()
        MyBase.New(AddressOf Usage.FromId)
    End Sub

End Class
