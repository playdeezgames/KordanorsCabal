Public Class CharacterRepair
    Inherits BaseThingie
    Implements ICharacterRepair
    Private character As ICharacter

    Public Sub New(worldData As IWorldData, character As ICharacter)
        MyBase.New(worldData, character.Id)
        Me.character = character
    End Sub

    Shared Function FromCharacter(worldData As IWorldData, character As ICharacter) As ICharacterRepair
        Return If(character IsNot Nothing, New CharacterRepair(worldData, character), Nothing)
    End Function
End Class
