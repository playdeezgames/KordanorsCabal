Public Class LocationFactionsShould
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
                subject.Enemy(Character.FromId(worldData.Object, characterId)).ShouldBeNull
                worldData.Verify(Function(x) x.Character.ReadForLocation(id))
            End Sub)
    End Sub
    <Fact>
    Sub have_enemies()
        WithSubject(
            Sub(worldData, id, subject)
                Const characterId = 2L
                worldData.Setup(Function(x) x.Character.ReadForLocation(It.IsAny(Of Long)))
                subject.Enemies(Character.FromId(worldData.Object, characterId)).ShouldBeEmpty
                worldData.Verify(Function(x) x.Character.ReadForLocation(id))
            End Sub)
    End Sub
    <Fact>
    Sub have_friends()
        WithSubject(
            Sub(worldData, id, subject)
                Const characterId = 2L
                worldData.Setup(Function(x) x.Character.ReadForLocation(It.IsAny(Of Long)))
                subject.Allies(Character.FromId(worldData.Object, characterId)).ShouldBeEmpty
                worldData.Verify(Function(x) x.Character.ReadForLocation(id))
            End Sub)
    End Sub
End Class
