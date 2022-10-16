Public Class CharacterEquipment
    Inherits BaseThingie
    Implements ICharacterEquipment
    Private character As ICharacter

    Public Sub New(worldData As IWorldData, character As ICharacter)
        MyBase.New(worldData, character.Id)
        Me.character = character
    End Sub

    Shared Function FromCharacter(worldData As IWorldData, character As ICharacter) As ICharacterEquipment
        Return If(character IsNot Nothing, New CharacterEquipment(worldData, character), Nothing)
    End Function
End Class
