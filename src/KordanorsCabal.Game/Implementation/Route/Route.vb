Public Class Route
    Inherits BaseThingie
    Public Sub New(worldData As IWorldData, routeId As Long)
        MyBase.New(worldData, routeId)
    End Sub
    Public Shared Function FromId(worldData As IWorldData, routeId As Long?) As Route
        Return If(routeId.HasValue, New Route(worldData, routeId.Value), Nothing)
    End Function
    Public Shared Function Create(worldData As IWorldData, location As ILocation, direction As IDirection, routeType As OldRouteType, toLocation As ILocation) As Route
        Return FromId(worldData, worldData.Route.Create(location.Id, direction.Id, routeType, toLocation.Id))
    End Function
    Friend ReadOnly Property ToLocation As ILocation
        Get
            Return Location.FromId(WorldData, WorldData.Route.ReadToLocation(Id))
        End Get
    End Property
    Property RouteType As OldRouteType
        Get
            Return CType(WorldData.Route.ReadRouteType(Id), OldRouteType)
        End Get
        Set(value As OldRouteType)
            WorldData.Route.WriteRouteType(Id, value)
        End Set
    End Property
    Friend Sub Destroy()
        WorldData.Route.Clear(Id)
    End Sub
    ReadOnly Property IsLocked As Boolean
        Get
            Return RouteType.UnlockItem.HasValue
        End Get
    End Property

    Friend Function CanMove(player As ICharacter) As Boolean
        If Not IsLocked Then
            Return True
        End If
        If Not player.HasItemType(ItemType.FromId(WorldData, RouteType.UnlockItem)) Then
            Return False
        End If
        Return True
    End Function
    Friend Function Move(player As ICharacter) As ILocation
        If CanMove(player) Then
            If IsLocked Then
                player.Inventory.ItemsOfType(ItemType.FromId(WorldData, RouteType.UnlockItem.Value)).First.Destroy()
                RouteType = If(RouteType.UnlockedRouteType, RouteType)
                Play(Sfx.UnlockDoor)
            End If
            Dim destination = ToLocation
            If RouteType.IsSingleUse Then
                Destroy()
            End If
            Return destination
        End If
        Return player.Location
    End Function
End Class
