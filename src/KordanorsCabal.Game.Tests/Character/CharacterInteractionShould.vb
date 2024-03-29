﻿Public Class CharacterInteractionShould
    Inherits ThingieShould(Of ICharacterInteraction)
    Sub New()
        MyBase.New(Function(w, i) CharacterInteraction.FromCharacter(w, Character.FromId(w, i)))
    End Sub
    <Fact>
    Sub have_can_interact()
        WithSubject(
            Sub(worldData, id, subject)
                worldData.SetupGet(Function(x) x.Character).Returns((New Mock(Of ICharacterData)).Object)

                subject.CanInteract().ShouldBeFalse

                worldData.Verify(Function(x) x.Character.ReadLocation(id))
                worldData.Verify(Function(x) x.Player.Read())
            End Sub)
    End Sub
    <Fact>
    Sub interact()
        WithSubject(
            Sub(worldData, id, subject)
                worldData.Setup(Function(x) x.Character.ReadLocation(It.IsAny(Of Long)))
                subject.Interact()
                worldData.Verify(Function(x) x.Character.ReadLocation(id))
                worldData.Verify(Function(x) x.Player.Read())
            End Sub)
    End Sub
    <Fact>
    Sub have_can_gamble()
        WithSubject(
            Sub(worldData, id, subject)
                Const statisticTypeId = 14L

                worldData.SetupGet(Function(x) x.CharacterStatistic).Returns((New Mock(Of ICharacterStatisticData)).Object)
                worldData.SetupGet(Function(x) x.StatisticType).Returns((New Mock(Of IStatisticTypeData)).Object)

                subject.CanGamble.ShouldBeFalse

                worldData.Verify(Function(x) x.CharacterStatistic.Read(id, statisticTypeId))
                worldData.Verify(Function(x) x.StatisticType.ReadDefaultValue(statisticTypeId))
                worldData.Verify(Function(x) x.Player.Read())
            End Sub)
    End Sub
    <Fact>
    Sub gamble()
        WithSubject(
            Sub(worldData, id, subject)
                worldData.Setup(Function(x) x.CharacterStatistic.Read(It.IsAny(Of Long), It.IsAny(Of Long)))
                worldData.Setup(Function(x) x.StatisticType.ReadDefaultValue(It.IsAny(Of Long)))
                subject.Gamble()
                worldData.Verify(Function(x) x.CharacterStatistic.Read(id, 14))
                worldData.Verify(Function(x) x.StatisticType.ReadDefaultValue(14))
                worldData.Verify(Function(x) x.Player.Read())
            End Sub)
    End Sub
End Class
