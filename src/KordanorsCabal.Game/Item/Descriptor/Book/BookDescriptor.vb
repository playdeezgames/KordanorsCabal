Friend Class BookDescriptor
    Inherits ItemTypeDescriptor
    Private ReadOnly spellType As SpellType
    Sub New(spellType As SpellType)
        MyBase.New(
            $"Book of {spellType.Name}",,,,,,,,,,,,,,,,,,
            Function(character) character.CanLearn(spellType))
        Me.spellType = spellType
    End Sub
    Public Overrides Sub Use(character As Character)
        character.Learn(spellType)
    End Sub
End Class
