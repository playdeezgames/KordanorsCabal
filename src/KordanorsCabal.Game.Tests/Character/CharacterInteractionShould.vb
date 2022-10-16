Public Class CharacterInteractionShould
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
            End Sub)
    End Sub
    <Fact>
    Sub interact()
        WithSubject(
            Sub(worldData, id, subject)
                worldData.Setup(Function(x) x.Character.ReadLocation(It.IsAny(Of Long)))
                subject.Interact()
                worldData.Verify(Function(x) x.Character.ReadLocation(id))
            End Sub)
    End Sub
End Class
