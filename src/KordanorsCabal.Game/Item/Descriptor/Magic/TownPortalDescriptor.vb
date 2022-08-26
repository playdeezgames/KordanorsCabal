Friend Class TownPortalDescriptor
    Inherits ItemTypeDescriptor

    Sub New()
        MyBase.New(
            StaticWorldData.World,
            ItemType.TownPortal,
            "Town Portal",,,,,,,,,,,,,
            50,
            MakeList(ShoppeType.BlackMage),,,,
            Function(character) character.Location.IsDungeon,
            Sub(character)
                Dim location = character.Location
                Dim outDirection = Direction.FromId(StaticWorldData.World, 8L)
                Dim inDirection = Direction.FromId(StaticWorldData.World, 7L)
                location.DestroyRoute(outDirection)
                Dim destination = Game.Location.FromLocationType(StaticWorldData.World, LocationType.FromId(StaticWorldData.World, 1L)).Single
                destination.DestroyRoute(inDirection)
                Route.Create(StaticWorldData.World, location, outDirection, RouteType.Portal, destination)
                Route.Create(StaticWorldData.World, destination, inDirection, RouteType.Portal, location)
                character.EnqueueMessage("A portal opens before you!")
            End Sub)
    End Sub
End Class
