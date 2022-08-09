Friend Class BookDescriptor
    Inherits ItemTypeDescriptor
    Sub New(spellType As SpellType)
        MyBase.New(
            $"Book of {spellType.Name}",,,,,,,,,,,,,,,,,,
            Function(character) character.CanLearn(spellType),
            Sub(character) character.Learn(spellType))
    End Sub
End Class
