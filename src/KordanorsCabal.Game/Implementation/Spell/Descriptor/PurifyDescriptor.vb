Friend Class PurifyDescriptor
    Inherits SpellType

    Public Sub New(worldData As IWorldData, id As Long)
        MyBase.New(worldData, id)
    End Sub

    Public Overrides Sub Cast(character As ICharacter)
        character.PurifyItems()
        character.DoFatigue(1)
        character.EnqueueMessage("You purify yer inventory!")
    End Sub
End Class
