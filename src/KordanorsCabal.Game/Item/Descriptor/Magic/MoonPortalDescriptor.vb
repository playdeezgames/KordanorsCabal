Friend Class MoonPortalDescriptor
    Inherits ItemTypeDescriptor

    Sub New()
        MyBase.New(
            "Moon Portal",,,,,,,,,,,,,,,,,,
            Function(character) character.Location.IsDungeon)
    End Sub
    Public Overrides Sub Use(character As Character)
        Dim location = character.Location
        location.DestroyRoute(Direction.Outward)
        Dim destination = RNG.FromEnumerable(Game.Location.FromLocationType(LocationType.Moon))
        destination.DestroyRoute(Direction.Inward)
        Route.Create(location, Direction.Outward, RouteType.Portal, destination)
        Route.Create(destination, Direction.Inward, RouteType.Portal, location)
        character.EnqueueMessage("A portal opens before you!")
    End Sub
End Class
