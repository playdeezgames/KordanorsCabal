Public Class CharacterItemsShould
    Inherits ThingieShould(Of ICharacterItems)
    Sub New()
        MyBase.New(Function(w, i) CharacterItems.FromCharacter(w, Character.FromId(w, i)))
    End Sub
End Class
