Friend Class TownPortalDescriptor
    Inherits ItemTypeDescriptor

    Public Overrides ReadOnly Property SpawnLocationTypes(level as DungeonLevel) As HashSet(Of LocationType)
        Get
            Return New HashSet(Of LocationType)
        End Get
    End Property

    Sub New()
        MyBase.New("Town Portal")
    End Sub

    Public Overrides ReadOnly Property CanUse(character As Character) As Boolean
        Get
            Return character.Location.IsDungeon
        End Get
    End Property

    Public Overrides Sub Use(character As Character)
        Dim location = character.Location
        location.DestroyRoute(Direction.Outward)
        Dim destination = Game.Location.FromLocationType(LocationType.TownSquare).Single
        destination.DestroyRoute(Direction.Inward)
        Route.Create(location, Direction.Outward, RouteType.Portal, destination)
        Route.Create(destination, Direction.Inward, RouteType.Portal, location)
        character.EnqueueMessage("A portal opens before you!")
    End Sub
End Class
