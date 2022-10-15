Public Class CharacterManaShould
    Inherits ThingieShould(Of ICharacterMana)
    Sub New()
        MyBase.New(Function(w, i) CharacterMana.FromCharacter(w, Character.FromId(w, i)))
    End Sub
End Class
