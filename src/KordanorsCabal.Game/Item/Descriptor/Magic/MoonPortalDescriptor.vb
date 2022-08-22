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
                Dim outDirection = Direction.FromId(StaticWorldData.World, Outward)
                Dim inDirection = Direction.FromId(StaticWorldData.World, 7L)
                location.DestroyRoute(outDirection)
                Dim destination = RNG.FromEnumerable(Game.Location.FromLocationType(StaticWorldData.World, LocationType.FromId(StaticWorldData.World, 8L)))
                destination.DestroyRoute(inDirection)
                Route.Create(location, outDirection, RouteType.Portal, destination)
                Route.Create(destination, inDirection, RouteType.Portal, location)
                character.EnqueueMessage("A portal opens before you!")
            End Sub)
    End Sub
End Class
