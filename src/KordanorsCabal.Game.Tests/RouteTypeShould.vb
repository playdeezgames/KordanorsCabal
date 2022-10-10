Public Class RouteTypeShould
    Inherits ThingieShould(Of IRouteType)
    Public Sub New()
        MyBase.New(AddressOf RouteType.FromId)
    End Sub
    <Fact>
    Sub have_an_abbreviation()
        WithSubject(
            Sub(worldData, id, subject)
                worldData.Setup(Function(x) x.RouteType.ReadAbbreviation(It.IsAny(Of Long)))
                subject.Abbreviation.ShouldBeNull
                worldData.Verify(Function(x) x.RouteType.ReadAbbreviation(id))
            End Sub)
    End Sub
    <Fact>
    Sub have_is_single_use()
        WithSubject(
            Sub(worldData, id, subject)
                worldData.Setup(Function(x) x.RouteType.ReadIsSingleUse(It.IsAny(Of Long)))
                subject.IsSingleUse.ShouldBeFalse
                worldData.Verify(Function(x) x.RouteType.ReadIsSingleUse(id))
            End Sub)
    End Sub
    <Fact>
    Sub have_an_unlocked_route_type()
        WithSubject(
            Sub(worldData, id, subject)
                subject.UnlockedRouteType.ShouldBeNull
            End Sub)
    End Sub
    <Fact>
    Sub have_an_unlock_item()
        WithSubject(
            Sub(worldData, id, subject)
                subject.UnlockItem.ShouldBeNull
            End Sub)
    End Sub
End Class
