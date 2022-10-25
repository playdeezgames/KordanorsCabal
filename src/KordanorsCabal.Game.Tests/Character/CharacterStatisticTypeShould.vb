Public Class CharacterStatisticTypeShould
    Inherits ThingieShould(Of IStatisticType)
    Sub New()
        MyBase.New(AddressOf StatisticType.FromId)
    End Sub
    <Fact>
    Sub have_abbreviation()
        WithSubject(
            Sub(worldData, id, subject)
                worldData.Setup(Function(x) x.StatisticType.ReadAbbreviation(It.IsAny(Of Long)))
                subject.Abbreviation.ShouldBeNull
                worldData.Verify(Function(x) x.StatisticType.ReadAbbreviation(id))
            End Sub)
    End Sub
    <Fact>
    Sub have_a_default_value()
        WithSubject(
            Sub(worldData, id, subject)
                worldData.Setup(Function(x) x.StatisticType.ReadDefaultValue(It.IsAny(Of Long)))
                subject.DefaultValue.ShouldBeNull
                worldData.Verify(Function(x) x.StatisticType.ReadDefaultValue(id))
            End Sub)
    End Sub
    <Fact>
    Sub have_a_maximum_value()
        WithSubject(
            Sub(worldData, id, subject)
                worldData.Setup(Function(x) x.StatisticType.ReadMaximumValue(It.IsAny(Of Long))).Returns(0)
                subject.MaximumValue.ShouldBe(0)
                worldData.Verify(Function(x) x.StatisticType.ReadMaximumValue(id))
            End Sub)
    End Sub
    <Fact>
    Sub have_a_name()
        WithSubject(
            Sub(worldData, id, subject)
                worldData.Setup(Function(x) x.StatisticType.ReadName(It.IsAny(Of Long)))
                subject.Name.ShouldBeNull
                worldData.Verify(Function(x) x.StatisticType.ReadName(id))
            End Sub)
    End Sub
    <Fact>
    Sub have_a_minimum_value()
        WithSubject(
            Sub(worldData, id, subject)
                worldData.Setup(Function(x) x.StatisticType.ReadMinimumValue(It.IsAny(Of Long))).Returns(0)
                subject.MinimumValue.ShouldBe(0)
                worldData.Verify(Function(x) x.StatisticType.ReadMinimumValue(id))
            End Sub)
    End Sub
End Class
