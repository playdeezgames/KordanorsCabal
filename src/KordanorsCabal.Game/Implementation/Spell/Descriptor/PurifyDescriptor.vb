Friend Class PurifyDescriptor
    Inherits SpellType

    Public Sub New(worldData As IWorldData, id As Long)
        MyBase.New(worldData, id)
    End Sub

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
