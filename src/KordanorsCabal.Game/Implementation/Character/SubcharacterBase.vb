Public MustInherit Class SubcharacterBase
    Inherits BaseThingie
    Protected ReadOnly Property Character As ICharacter

    Protected Sub New(worldData As IWorldData, character As ICharacter)
        MyBase.New(worldData, character.Id)
        Me.Character = character
    End Sub
End Class
