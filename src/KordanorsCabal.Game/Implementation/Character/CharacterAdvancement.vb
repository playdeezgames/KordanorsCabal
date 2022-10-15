Public Class CharacterAdvancement
    Inherits BaseThingie
    Implements ICharacterAdvancement
    Private character As ICharacter

    Public Sub New(worldData As IWorldData, character As ICharacter)
        MyBase.New(worldData, character.Id)
        Me.character = character
    End Sub

    Shared Function FromCharacter(worldData As IWorldData, character As Character) As ICharacterAdvancement
        Return If(character IsNot Nothing, New CharacterAdvancement(worldData, character), Nothing)
    End Function
End Class
