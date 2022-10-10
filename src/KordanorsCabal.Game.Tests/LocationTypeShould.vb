Public Class LocationTypeShould
    Inherits ThingieShould(Of ILocationType)
    Public Sub New()
        MyBase.New(AddressOf LocationType.FromId)
    End Sub
    <Fact>
    Sub have_requires_mp()
        WithSubject(
            Sub(worldData, id, subject)
                worldData.Setup(Function(x) x.LocationType.ReadRequiresMP(It.IsAny(Of Long)))
                subject.RequiresMP.ShouldBeFalse
                worldData.Verify(Function(x) x.LocationType.ReadRequiresMP(id))
            End Sub)
    End Sub
    <Fact>
    Sub have_is_dungeon()
        WithSubject(
            Sub(worldData, id, subject)
                worldData.Setup(Function(x) x.LocationType.ReadIsDungeon(It.IsAny(Of Long)))
                subject.IsDungeon.ShouldBeFalse
                worldData.Verify(Function(x) x.LocationType.ReadIsDungeon(id))
            End Sub)
    End Sub
    <Fact>
    Sub have_name()
        WithSubject(
            Sub(worldData, id, subject)
                worldData.Setup(Function(x) x.LocationType.ReadName(It.IsAny(Of Long)))
                subject.Name.ShouldBeNull
                worldData.Verify(Function(x) x.LocationType.ReadName(id))
            End Sub)
    End Sub
    <Fact>
    Sub have_can_map()
        WithSubject(
            Sub(worldData, id, subject)
                worldData.Setup(Function(x) x.LocationType.ReadCanMap(It.IsAny(Of Long)))
                subject.CanMap.ShouldBeFalse
                worldData.Verify(Function(x) x.LocationType.ReadCanMap(id))
            End Sub)
    End Sub
End Class
