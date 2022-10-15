﻿Public Class CharacterMovement
    Inherits BaseThingie
    Implements ICharacterMovement

    Public Sub New(worldData As IWorldData, character As ICharacter)
        MyBase.New(worldData, character.Id)
        Me.Character = character
    End Sub

    Public ReadOnly Property Character As ICharacter Implements ICharacterMovement.Character

    Public Shared Function FromId(worldData As IWorldData, character As ICharacter) As ICharacterMovement
        Return If(character IsNot Nothing, New CharacterMovement(worldData, character), Nothing)
    End Function
    Public Function CanMoveLeft() As Boolean Implements ICharacterMovement.CanMoveLeft
        Return CanMove(Character.Direction.PreviousDirection)
    End Function
    Public Function CanMoveRight() As Boolean Implements ICharacterMovement.CanMoveRight
        Return CanMove(Character.Direction.NextDirection)
    End Function
    Public Function CanMoveBackward() As Boolean Implements ICharacterMovement.CanMoveBackward
        Return CanMove(Character.Direction.Opposite)
    End Function
    Public Function CanMove(direction As IDirection) As Boolean Implements ICharacterMovement.CanMove
        If Character.IsEncumbered Then
            Return False
        End If
        If Character.Location Is Nothing OrElse Not Character.Location.Routes.Exists(direction) Then
            Return False
        End If
        If Not Character.Location.Routes.Find(direction).CanMove(Character) Then
            Return False
        End If
        If Character.Location.Routes.Find(direction).ToLocation.LocationType.RequiresMP AndAlso Character.IsDemoralized() Then
            Return False
        End If
        Return True
    End Function
    Public Function Move(direction As IDirection) As Boolean Implements ICharacterMovement.Move
        If CanMove(direction) Then
            Dim hungerRate = Math.Max(Character.Highness \ 2 + Character.FoodPoisoning \ 2, 1)
            Character.Hunger += hungerRate
            Character.Drunkenness -= 1
            Character.Highness -= 1
            Character.FoodPoisoning -= 1
            Character.Chafing -= 1
            Character.Location = Character.Location.Routes.Find(direction).Move(Character)
            If Character.Hunger = CharacterStatisticType.FromId(WorldData, Constants.StatisticTypes.CharacterStatisticType20).MaximumValue Then
                Character.Hunger \= 2
                Character.Health.Current -= 1
                Return True
            End If
        End If
        Return False
    End Function
End Class
