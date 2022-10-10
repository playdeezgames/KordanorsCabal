Public Class Route
    Inherits BaseThingie
    Implements IRoute
    Public Sub New(worldData As IWorldData, routeId As Long)
        MyBase.New(worldData, routeId)
    End Sub
    Public Shared Function FromId(worldData As IWorldData, routeId As Long?) As IRoute
        Return If(routeId.HasValue, New Route(worldData, routeId.Value), Nothing)
    End Function
    Public Shared Function Create(worldData As IWorldData, location As ILocation, direction As IDirection, routeType As OldRouteType, toLocation As ILocation) As IRoute
        Return FromId(worldData, worldData.Route.Create(location.Id, direction.Id, routeType, toLocation.Id))
    End Function
    Public ReadOnly Property ToLocation As ILocation Implements IRoute.ToLocation
        Get
            Return Location.FromId(WorldData, WorldData.Route.ReadToLocation(Id))
        End Get
    End Property
    Property RouteType As OldRouteType Implements IRoute.RouteType
        Get
            Return CType(WorldData.Route.ReadRouteType(Id), OldRouteType)
        End Get
        Set(value As OldRouteType)
            WorldData.Route.WriteRouteType(Id, value)
        End Set
    End Property
    Public Sub Destroy() Implements IRoute.Destroy
        WorldData.Route.Clear(Id)
    End Sub
    ReadOnly Property IsLocked As Boolean
        Get
            Return RouteType.UnlockItem(WorldData).HasValue
        End Get
    End Property

    Public Function CanMove(player As ICharacter) As Boolean Implements IRoute.CanMove
        If Not IsLocked Then
            Return True
        End If
        If Not player.HasItemType(ItemType.FromId(WorldData, RouteType.UnlockItem(WorldData))) Then
            Return False
        End If
        Return True
    End Function
    Friend Function Move(player As ICharacter) As ILocation Implements IRoute.Move
        If CanMove(player) Then
            If IsLocked Then
                player.Inventory.ItemsOfType(ItemType.FromId(WorldData, RouteType.UnlockItem(WorldData).Value)).First.Destroy()
                RouteType = If(RouteType.UnlockedRouteType(WorldData), RouteType)
                Play(Sfx.UnlockDoor)
            End If
            Dim destination = ToLocation
            If RouteType.IsSingleUse(WorldData) Then
                Destroy()
            End If
            Return destination
        End If
        Return player.Location
    End Function
End Class
