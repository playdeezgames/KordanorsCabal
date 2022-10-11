Public Class DirectionShould
    Inherits ThingieShould(Of IDirection)
    Public Sub New()
        MyBase.New(AddressOf Direction.FromId)
    End Sub
    <Fact>
    Sub has_an_opposite()
        WithSubject(
            Sub(worldData, id, subject)
                worldData.Setup(Function(x) x.Direction.ReadOpposite(It.IsAny(Of Long)))
                subject.Opposite.ShouldBeNull
                worldData.Verify(Function(x) x.Direction.ReadOpposite(id))
            End Sub)
    End Sub
    <Fact>
    Sub has_an_previous_direction()
        WithSubject(
            Sub(worldData, id, subject)
                worldData.Setup(Function(x) x.Direction.ReadPrevious(It.IsAny(Of Long)))
                subject.PreviousDirection.ShouldBeNull
                worldData.Verify(Function(x) x.Direction.ReadPrevious(id))
            End Sub)
    End Sub
    <Fact>
    Sub has_a_next_direction()
        WithSubject(
            Sub(worldData, id, subject)
                worldData.Setup(Function(x) x.Direction.ReadNext(It.IsAny(Of Long)))
                subject.NextDirection.ShouldBeNull
                worldData.Verify(Function(x) x.Direction.ReadNext(id))
            End Sub)
    End Sub
    <Fact>
    Sub has_a_name()
        WithSubject(
            Sub(worldData, id, subject)
                worldData.Setup(Function(x) x.Direction.ReadName(It.IsAny(Of Long)))
                subject.Name.ShouldBeNull
                worldData.Verify(Function(x) x.Direction.ReadName(id))
            End Sub)
    End Sub
    <Fact>
    Sub has_an_abbreviation()
        WithSubject(
            Sub(worldData, id, subject)
                worldData.Setup(Function(x) x.Direction.ReadAbbreviation(It.IsAny(Of Long)))
                subject.Abbreviation.ShouldBeNull
                worldData.Verify(Function(x) x.Direction.ReadAbbreviation(id))
            End Sub)
    End Sub
End Class
