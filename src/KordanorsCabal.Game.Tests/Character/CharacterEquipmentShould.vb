Public Class CharacterEquipmentShould
    Inherits ThingieShould(Of ICharacterEquipment)
    Sub New()
        MyBase.New(Function(w, i) CharacterEquipment.FromCharacter(w, Character.FromId(w, i)))
    End Sub
End Class
