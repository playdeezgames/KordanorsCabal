Public Class FeatureShould
    Inherits ThingieShould(Of IFeature)
    Public Sub New()
        MyBase.New(AddressOf Feature.FromId)
    End Sub
    <Fact>
    Sub have_a_feature_type()
        WithSubject(
            Sub(worldData, id, subject)
                Const featureTypeId = 2L
                worldData.Setup(Function(x) x.Feature.ReadFeatureType(It.IsAny(Of Long))).Returns(featureTypeId)
                subject.FeatureType.ShouldNotBeNull
                worldData.Verify(Function(x) x.Feature.ReadFeatureType(id))
            End Sub)
    End Sub
    <Fact>
    Sub have_a_name()
        WithSubject(
            Sub(worldData, id, subject)
                Const featureTypeId = 2L
                worldData.Setup(Function(x) x.Feature.ReadFeatureType(It.IsAny(Of Long))).Returns(featureTypeId)
                worldData.Setup(Function(x) x.FeatureType.ReadName(It.IsAny(Of Long)))
                subject.Name.ShouldBeNull
                worldData.Verify(Function(x) x.Feature.ReadFeatureType(id))
                worldData.Verify(Function(x) x.FeatureType.ReadName(featureTypeId))
            End Sub)
    End Sub
    <Fact>
    Sub have_an_interaction_mode()
        WithSubject(
            Sub(worldData, id, subject)
                Const featureTypeId = 2L
                worldData.Setup(Function(x) x.Feature.ReadFeatureType(It.IsAny(Of Long))).Returns(featureTypeId)
                worldData.Setup(Function(x) x.FeatureType.ReadInteractionMode(It.IsAny(Of Long))).Returns(0L)
                subject.InteractionMode.ShouldBe(PlayerMode.None)
                worldData.Verify(Function(x) x.Feature.ReadFeatureType(id))
                worldData.Verify(Function(x) x.FeatureType.ReadInteractionMode(featureTypeId))
            End Sub)
    End Sub
End Class
