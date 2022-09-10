Friend Class PurifyDescriptor
    Inherits SpellDescriptor

    Public Overrides ReadOnly Property Name As String
        Get
            Return "Purify"
        End Get
    End Property

    Public Overrides ReadOnly Property MaximumLevel As Long
        Get
            Return 1
        End Get
    End Property

    Public Overrides ReadOnly Property RequiredPower(level As Long) As Long
        Get
            Return 0
        End Get
    End Property

    Public Overrides ReadOnly Property CanCast(character As ICharacter) As Boolean
        Get
            Return character.CurrentMana > 0
        End Get
    End Property

    Public Overrides Sub Cast(character As ICharacter)
        character.PurifyItems()
        character.DoFatigue(1)
        character.EnqueueMessage("You purify yer inventory!")
    End Sub
End Class
