Friend Class BookDescriptor
    Inherits ItemTypeDescriptor
    Private ReadOnly spellType As SpellType
    Sub New(spellType As SpellType)
        MyBase.New($"Book of {spellType.Name}")
        Me.spellType = spellType
    End Sub
    Public Overrides ReadOnly Property CanUse(character As Character) As Boolean
        Get
            Return character.CanLearn(spellType)
        End Get
    End Property

    Public Overrides Sub Use(character As Character)
        character.Learn(spellType)
    End Sub
End Class
