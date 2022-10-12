Public Class LocationFactionsShould
    Inherits ThingieShould(Of ILocationFactions)
    Public Sub New()
        MyBase.New(AddressOf LocationFactions.FromId)
    End Sub
End Class
