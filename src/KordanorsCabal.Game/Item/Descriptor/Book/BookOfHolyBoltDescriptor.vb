Friend Class BookOfHolyBoltDescriptor
    Inherits ItemTypeDescriptor

    Public Overrides ReadOnly Property SpawnLocationTypes(level As Long) As HashSet(Of LocationType)
        Get
            Return New HashSet(Of LocationType)
        End Get
    End Property

    Public Overrides ReadOnly Property Name As String
        Get
            Return "Book of Holy Bolt"
        End Get
    End Property

    Public Overrides ReadOnly Property CanUse(character As Character) As Boolean
        Get
            Return character.CanLearn(SpellType.HolyBolt)
        End Get
    End Property

    Public Overrides Sub Use(character As Character)
        character.Learn(SpellType.HolyBolt)
    End Sub
End Class
