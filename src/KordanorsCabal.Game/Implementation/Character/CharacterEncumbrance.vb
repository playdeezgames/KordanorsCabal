Public Class CharacterEncumbrance
    Inherits BaseThingie
    Implements ICharacterEncumbrance
    Private character As ICharacter

    Public Sub New(worldData As IWorldData, character As ICharacter)
        MyBase.New(worldData, character.Id)
        Me.character = character
    End Sub

    Shared Function FromCharacter(worldData As IWorldData, character As ICharacter) As ICharacterEncumbrance
        Return If(character IsNot Nothing, New CharacterEncumbrance(worldData, character), Nothing)
    End Function
End Class
