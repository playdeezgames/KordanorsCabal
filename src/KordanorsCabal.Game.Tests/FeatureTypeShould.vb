Public Class FeatureTypeShould
    Inherits ThingieShould(Of IFeatureType)
    Public Sub New()
        MyBase.New(AddressOf FeatureType.FromId)
    End Sub
    <Fact>
    Sub have_a_name()
        WithSubject(
            Sub(worldData, id, subject)
                worldData.Setup(Function(x) x.FeatureType.ReadName(It.IsAny(Of Long)))
                subject.Name.ShouldBeNull
                worldData.Verify(Function(x) x.FeatureType.ReadName(id))
            End Sub)
    End Sub
    <Fact>
    Sub have_a_location_type()
        WithSubject(
            Sub(worldData, id, subject)
                worldData.Setup(Function(x) x.FeatureType.ReadLocationType(It.IsAny(Of Long)))
                subject.LocationType.ShouldBeNull
                worldData.Verify(Function(x) x.FeatureType.ReadLocationType(id))
            End Sub)
    End Sub
    <Fact>
    Sub have_an_interaction_mode()
        WithSubject(
            Sub(worldData, id, subject)
                worldData.Setup(Function(x) x.FeatureType.ReadInteractionMode(It.IsAny(Of Long))).Returns(0)
                subject.InteractionMode.ShouldBe(PlayerMode.None)
                worldData.Verify(Function(x) x.FeatureType.ReadInteractionMode(id))
            End Sub)
    End Sub
End Class
