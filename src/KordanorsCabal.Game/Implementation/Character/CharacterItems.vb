﻿Public Class CharacterItems
    Inherits BaseThingie
    Implements ICharacterItems
    Private character As ICharacter

    Public Sub New(worldData As IWorldData, character As ICharacter)
        MyBase.New(worldData, character.Id)
        Me.character = character
    End Sub

    Shared Function FromCharacter(worldData As IWorldData, character As ICharacter) As ICharacterItems
        Return If(character IsNot Nothing, New CharacterItems(worldData, character), Nothing)
    End Function
End Class
