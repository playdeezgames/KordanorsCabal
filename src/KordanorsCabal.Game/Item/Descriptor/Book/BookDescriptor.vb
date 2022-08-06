Friend Class BookDescriptor
    Inherits ItemTypeDescriptor
    Private ReadOnly spellType As SpellType
    Sub New(spellType As SpellType)
        Me.spellType = spellType
    End Sub

    Public Overrides ReadOnly Property SpawnLocationTypes(level As Long) As HashSet(Of LocationType)
        Get
            Return New HashSet(Of LocationType)
        End Get
    End Property

    Public Overrides ReadOnly Property Name As String
        Get
            Return $"Book of {spellType.Name}"
        End Get
    End Property
    Public Overrides ReadOnly Property CanUse(character As Character) As Boolean
        Get
            Return character.CanLearn(spellType)
        End Get
    End Property

    Public Overrides Sub Use(character As Character)
        character.Learn(spellType)
    End Sub
End Class
