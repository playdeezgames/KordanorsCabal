Public Class CharacterRepairShould
    Inherits ThingieShould(Of ICharacterRepair)
    Sub New()
        MyBase.New(Function(w, i) CharacterRepair.FromCharacter(w, Character.FromId(w, i)))
    End Sub
End Class
