﻿Public Class LocationFactionsShould
    Inherits ThingieShould(Of ILocationFactions)
    Public Sub New()
        MyBase.New(AddressOf LocationFactions.FromId)
    End Sub
    <Fact>
    Sub have_enemy()
        WithSubject(
            Sub(worldData, id, subject)
                Const characterId = 2L
                worldData.Setup(Function(x) x.Character.ReadForLocation(It.IsAny(Of Long)))
                subject.FirstEnemy(Character.FromId(worldData.Object, characterId)).ShouldBeNull
                worldData.Verify(Function(x) x.Character.ReadForLocation(id))
                worldData.Verify(Function(x) x.Player.Read())
            End Sub)
    End Sub
    <Fact>
    Sub have_enemies()
        WithSubject(
            Sub(worldData, id, subject)
                Const characterId = 2L
                worldData.Setup(Function(x) x.Character.ReadForLocation(It.IsAny(Of Long)))
                subject.EnemiesOf(Character.FromId(worldData.Object, characterId)).ShouldBeEmpty
                worldData.Verify(Function(x) x.Character.ReadForLocation(id))
                worldData.Verify(Function(x) x.Player.Read())
            End Sub)
    End Sub
    <Fact>
    Sub have_friends()
        WithSubject(
            Sub(worldData, id, subject)
                Const characterId = 2L
                worldData.Setup(Function(x) x.Character.ReadForLocation(It.IsAny(Of Long)))
                subject.AlliesOf(Character.FromId(worldData.Object, characterId)).ShouldBeEmpty
                worldData.Verify(Function(x) x.Character.ReadForLocation(id))
                worldData.Verify(Function(x) x.Player.Read())
            End Sub)
    End Sub
End Class
