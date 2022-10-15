Public Class CharacterMentalCombatShould
    Inherits ThingieShould(Of ICharacterMentalCombat)
    Sub New()
        MyBase.New(Function(w, i) CharacterMentalCombat.FromCharacter(w, Character.FromId(w, i)))
    End Sub

End Class
