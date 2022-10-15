Public Class CharacterMentalCombat
    Inherits BaseThingie
    Implements ICharacterMentalCombat
    Private character As ICharacter
    Public Sub New(worldData As IWorldData, character As ICharacter)
        MyBase.New(worldData, character.Id)
        Me.character = character
    End Sub

    Public Shared Function FromCharacter(worldData As IWorldData, character As ICharacter) As ICharacterMentalCombat
        Return If(character IsNot Nothing, New CharacterMentalCombat(worldData, character), Nothing)
    End Function
End Class
