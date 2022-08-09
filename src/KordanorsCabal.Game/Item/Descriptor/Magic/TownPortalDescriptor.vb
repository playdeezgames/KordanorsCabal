Friend Class TownPortalDescriptor
    Inherits ItemTypeDescriptor

    Sub New()
        MyBase.New(
            "Town Portal",,,,,,,,,,,,,,,,,,
            Function(character) character.Location.IsDungeon)
    End Sub
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
