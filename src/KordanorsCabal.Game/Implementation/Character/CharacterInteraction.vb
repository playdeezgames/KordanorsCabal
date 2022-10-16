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
    Public ReadOnly Property CanGamble As Boolean Implements ICharacterInteraction.CanGamble
        Get
            Return character.Statuses.Money >= 5
        End Get
    End Property
    Public Sub Gamble() Implements ICharacterInteraction.Gamble
        If Not CanGamble Then
            character.EnqueueMessage("You cannot gamble at this time!")
            Return
        End If
        Dim lines As New List(Of String)
        lines.Add("You flip the two coins!")
        Dim firstCoin = RNG.FromRange(0, 1)
        lines.Add($"The first coin comes up {If(firstCoin > 0, "heads", "tails")}!")
        Dim secondCoin = RNG.FromRange(0, 1)
        lines.Add($"The second coin comes up {If(secondCoin > 0, "heads", "tails")}!")
        Dim winner = firstCoin > 0 AndAlso secondCoin > 0
        If winner Then
            lines.Add("You win and receive 15 money!")
            character.Statuses.Money += 15
        Else
            lines.Add("You lose and must pay 5 money!")
            character.Statuses.Money -= 5
        End If
        'TODO: sound effect
        character.EnqueueMessage(lines.ToArray)
    End Sub
End Class
