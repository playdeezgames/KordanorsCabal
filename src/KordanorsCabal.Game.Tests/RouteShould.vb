Public Class RouteShould
    Inherits ThingieShould(Of IRoute)

    Public Sub New()
        MyBase.New(AddressOf Route.FromId)
    End Sub
End Class
