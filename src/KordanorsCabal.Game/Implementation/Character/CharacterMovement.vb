Public Class CharacterMovement
    Inherits BaseThingie
    Implements ICharacterMovement

    Public Sub New(worldData As IWorldData, character As ICharacter)
        MyBase.New(worldData, character.Id)
        Me.Character = character
    End Sub
    Property Location As ILocation Implements ICharacterMovement.Location
        Get
            Dim result = WorldData.Character.ReadLocation(Id)
            If result Is Nothing Then
                Return Nothing
            End If
            Return Game.Location.FromId(WorldData, result.Value)
        End Get
        Set(value As ILocation)
            WorldData.Character.WriteLocation(Id, value.Id)
            WorldData.CharacterLocation.Write(Id, value.Id)
        End Set
    End Property
    Public ReadOnly Property Character As ICharacter Implements ICharacterMovement.Character
    Public Function CanMoveForward() As Boolean Implements ICharacterMovement.CanMoveForward
        Return CanMove(Direction)
    End Function
    Public ReadOnly Property CanMap() As Boolean Implements ICharacterMovement.CanMap
        Get
            Dim result = Location
            Return If(result IsNot Nothing, result.LocationType.CanMap, False)
        End Get
    End Property
    Public Property Direction As IDirection Implements ICharacterMovement.Direction
        Get
            Return New Direction(WorldData, WorldData.Player.ReadDirection().Value)
        End Get
        Set(value As IDirection)
            WorldData.Player.WriteDirection(value.Id)
        End Set
    End Property
    Public Function HasVisited(location As ILocation) As Boolean Implements ICharacterMovement.HasVisited
        Return WorldData.CharacterLocation.Read(Id, location.Id)
    End Function
    Public Shared Function FromCharacter(worldData As IWorldData, character As ICharacter) As ICharacterMovement
        Return If(character IsNot Nothing, New CharacterMovement(worldData, character), Nothing)
    End Function
    Public Function CanMoveLeft() As Boolean Implements ICharacterMovement.CanMoveLeft
        Return CanMove(Direction.PreviousDirection)
    End Function
    Public Function CanMoveRight() As Boolean Implements ICharacterMovement.CanMoveRight
        Return CanMove(Direction.NextDirection)
    End Function
    Public Function CanMoveBackward() As Boolean Implements ICharacterMovement.CanMoveBackward
        Return CanMove(Direction.Opposite)
    End Function
    Public Function CanMove(direction As IDirection) As Boolean Implements ICharacterMovement.CanMove
        If Me.Character.Encumbrance.IsEncumbered Then
            Return False
        End If
        If Location Is Nothing OrElse Not Location.Routes.Exists(direction) Then
            Return False
        End If
        If Not Location.Routes.Find(direction).CanMove(Character) Then
            Return False
        End If
        If Location.Routes.Find(direction).ToLocation.LocationType.RequiresMP AndAlso Character.MentalCombat.IsDemoralized() Then
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
            Location = Location.Routes.Find(direction).Move(Character)
            If Character.Hunger = CharacterStatisticType.FromId(WorldData, Constants.StatisticTypes.Hunger).MaximumValue Then
                Character.Hunger \= 2
                Character.Health.Current -= 1
                Return True
            End If
        End If
        Return False
    End Function
End Class
