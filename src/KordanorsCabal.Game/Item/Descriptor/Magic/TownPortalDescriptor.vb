Friend Class TownPortalDescriptor
    Inherits ItemTypeDescriptor

    Sub New()
        MyBase.New(
            ItemType.TownPortal,
            "Town Portal",,,,,,,,,,,,,
            50,
            MakeList(ShoppeType.BlackMage),,,,
            Function(character) character.Location.IsDungeon,
            Sub(character)
                Dim location = character.Location
                Dim outDirection = DirectionDescriptor.FromName("Out")
                Dim inDirection = DirectionDescriptor.FromName("In")
                location.DestroyRoute(outDirection)
                Dim destination = Game.Location.FromLocationType(LocationType.TownSquare).Single
                destination.DestroyRoute(inDirection)
                Route.Create(location, outDirection, RouteType.Portal, destination)
                Route.Create(destination, inDirection, RouteType.Portal, location)
                character.EnqueueMessage("A portal opens before you!")
            End Sub)
    End Sub
End Class
