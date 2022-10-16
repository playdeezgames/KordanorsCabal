Public Class CharacterStatuses
    Inherits BaseThingie
    Implements ICharacterStatuses
    Private character As ICharacter

    Public Sub New(worldData As IWorldData, character As ICharacter)
        MyBase.New(worldData, character.Id)
        Me.character = character
    End Sub

    Shared Function FromCharacter(worldData As IWorldData, character As ICharacter) As ICharacterStatuses
        Return If(character IsNot Nothing, New CharacterStatuses(worldData, character), Nothing)
    End Function
End Class
