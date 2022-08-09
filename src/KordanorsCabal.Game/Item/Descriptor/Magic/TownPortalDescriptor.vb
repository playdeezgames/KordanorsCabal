Friend Class TownPortalDescriptor
    Inherits ItemTypeDescriptor

    Sub New()
        MyBase.New(
            "Town Portal",,,,,,,,,,,,,
            50,
            MakeList(ShoppeType.BlackMage),,,,
            Function(character) character.Location.IsDungeon,
            Sub(character)
                Dim location = character.Location
                location.DestroyRoute(Direction.Outward)
                Dim destination = Game.Location.FromLocationType(LocationType.TownSquare).Single
                destination.DestroyRoute(Direction.Inward)
                Route.Create(location, Direction.Outward, RouteType.Portal, destination)
                Route.Create(destination, Direction.Inward, RouteType.Portal, location)
                character.EnqueueMessage("A portal opens before you!")
            End Sub)
    End Sub
End Class
