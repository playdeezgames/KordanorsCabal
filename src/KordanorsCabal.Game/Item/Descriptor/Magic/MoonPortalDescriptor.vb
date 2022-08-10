﻿Friend Class MoonPortalDescriptor
    Inherits ItemTypeDescriptor

    Sub New()
        MyBase.New(
            ItemType.MoonPortal,
            "Moon Portal",,,,,,,,,,,,,
            5000,
            MakeList(ShoppeType.BlackMage),,,,
            Function(character) character.Location.IsDungeon,
            Sub(character)
                Dim location = character.Location
                Dim outDirection = Direction.FromName(Outward)
                Dim inDirection = Direction.FromName(Inward)
                location.DestroyRoute(outDirection)
                Dim destination = RNG.FromEnumerable(Game.Location.FromLocationType(OldLocationType.Moon))
                destination.DestroyRoute(inDirection)
                Route.Create(location, outDirection, RouteType.Portal, destination)
                Route.Create(destination, inDirection, RouteType.Portal, location)
                character.EnqueueMessage("A portal opens before you!")
            End Sub)
    End Sub
End Class
