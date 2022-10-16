Public Class CharacterStatusesShould
    Inherits ThingieShould(Of ICharacterStatuses)
    Sub New()
        MyBase.New(Function(w, i) CharacterStatuses.FromCharacter(w, Character.FromId(w, i)))
    End Sub
End Class
