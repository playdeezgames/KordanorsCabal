Public Class CharacterInteractionShould
    Inherits ThingieShould(Of ICharacterInteraction)
    Sub New()
        MyBase.New(Function(w, i) CharacterInteraction.FromCharacter(w, Character.FromId(w, i)))
    End Sub
End Class
