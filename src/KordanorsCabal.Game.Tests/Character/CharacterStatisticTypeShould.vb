Public Class CharacterStatisticTypeShould
    Inherits ThingieShould(Of IStatisticType)
    Sub New()
        MyBase.New(AddressOf StatisticType.FromId)
    End Sub
    <Fact>
    Sub have_abbreviation()
        WithSubject(
            Sub(worldData, id, subject)
                worldData.Setup(Function(x) x.CharacterStatisticType.ReadAbbreviation(It.IsAny(Of Long)))
                subject.Abbreviation.ShouldBeNull
                worldData.Verify(Function(x) x.CharacterStatisticType.ReadAbbreviation(id))
            End Sub)
    End Sub
    <Fact>
    Sub have_a_default_value()
        WithSubject(
            Sub(worldData, id, subject)
                worldData.Setup(Function(x) x.CharacterStatisticType.ReadDefaultValue(It.IsAny(Of Long)))
                subject.DefaultValue.ShouldBeNull
                worldData.Verify(Function(x) x.CharacterStatisticType.ReadDefaultValue(id))
            End Sub)
    End Sub
    <Fact>
    Sub have_a_maximum_value()
        WithSubject(
            Sub(worldData, id, subject)
                worldData.Setup(Function(x) x.CharacterStatisticType.ReadMaximumValue(It.IsAny(Of Long))).Returns(0)
                subject.MaximumValue.ShouldBe(0)
                worldData.Verify(Function(x) x.CharacterStatisticType.ReadMaximumValue(id))
            End Sub)
    End Sub
    <Fact>
    Sub have_a_name()
        WithSubject(
            Sub(worldData, id, subject)
                worldData.Setup(Function(x) x.CharacterStatisticType.ReadName(It.IsAny(Of Long)))
                subject.Name.ShouldBeNull
                worldData.Verify(Function(x) x.CharacterStatisticType.ReadName(id))
            End Sub)
    End Sub
    <Fact>
    Sub have_a_minimum_value()
        WithSubject(
            Sub(worldData, id, subject)
                worldData.Setup(Function(x) x.CharacterStatisticType.ReadMinimumValue(It.IsAny(Of Long))).Returns(0)
                subject.MinimumValue.ShouldBe(0)
                worldData.Verify(Function(x) x.CharacterStatisticType.ReadMinimumValue(id))
            End Sub)
    End Sub
End Class
