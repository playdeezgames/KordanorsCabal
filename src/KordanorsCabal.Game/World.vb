Public Module World
    Public Sub Start()
        Store.Reset()
        Dim startingLocation = Location.Create()
        Dim playerCharacter = Character.Create(CharacterType.N00b, startingLocation)
        'set the player character
        'roll up player character stats
        Throw New NotImplementedException()
    End Sub
    Public ReadOnly Property PlayerCharacter As PlayerCharacter
        Get
            Return New PlayerCharacter
        End Get
    End Property
End Module
