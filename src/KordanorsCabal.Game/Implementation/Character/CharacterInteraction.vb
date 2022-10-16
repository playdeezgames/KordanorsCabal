Public Class CharacterInteraction
    Inherits BaseThingie
    Implements ICharacterInteraction
    Private character As ICharacter

    Public Sub New(worldData As IWorldData, character As ICharacter)
        MyBase.New(worldData, character.Id)
        Me.character = character
    End Sub

    Shared Function FromCharacter(worldData As IWorldData, character As ICharacter) As ICharacterInteraction
        Return If(character IsNot Nothing, New CharacterInteraction(worldData, character), Nothing)
    End Function
    Public ReadOnly Property CanInteract As Boolean Implements ICharacterInteraction.CanInteract
        Get
            Return (character.Movement.Location?.Feature?.Id).HasValue
        End Get
    End Property
    Public Sub Interact() Implements ICharacterInteraction.Interact
        If CanInteract Then
            character.Mode = character.Movement.Location.Feature.InteractionMode()
        End If
    End Sub
End Class
