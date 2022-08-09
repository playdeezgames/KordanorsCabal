﻿Friend Class TownPortalDescriptor
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
                location.DestroyRoute(Direction.Outward.ToDescriptor)
                Dim destination = Game.Location.FromLocationType(LocationType.TownSquare).Single
                destination.DestroyRoute(Direction.Inward.ToDescriptor)
                Route.Create(location, Direction.Outward.ToDescriptor, RouteType.Portal, destination)
                Route.Create(destination, Direction.Inward.ToDescriptor, RouteType.Portal, location)
                character.EnqueueMessage("A portal opens before you!")
            End Sub)
    End Sub
End Class
