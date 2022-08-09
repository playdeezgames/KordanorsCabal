Public Class Route
    Public ReadOnly Id As Long
    Public Sub New(routeId As Long)
        Id = routeId
    End Sub
    Public Shared Function FromId(routeId As Long) As Route
        Return New Route(routeId)
    End Function
    Public Shared Function Create(location As Location, direction As Direction, routeType As RouteType, toLocation As Location) As Route
        Return FromId(StaticWorldData.Route.Create(location.Id, direction, routeType, toLocation.Id))
    End Function
    Friend ReadOnly Property ToLocation As Location
        Get
            Return Location.FromId(StaticWorldData.Route.ReadToLocation(Id))
        End Get
    End Property
    Property RouteType As RouteType
        Get
            Return CType(StaticWorldData.Route.ReadRouteType(Id), RouteType)
        End Get
        Set(value As RouteType)
            StaticWorldData.Route.WriteRouteType(Id, value)
        End Set
    End Property
    Friend Sub Destroy()
        StaticWorldData.Route.Clear(Id)
    End Sub
    ReadOnly Property IsLocked As Boolean
        Get
            Return RouteType.UnlockItem.HasValue
        End Get
    End Property

    Friend Function CanMove(player As Character) As Boolean
        If Not IsLocked Then
            Return True
        End If
        If Not player.HasItemType(RouteType.UnlockItem.Value) Then
            Return False
        End If
        Return True
    End Function
    Friend Function Move(player As Character) As Location
        If CanMove(player) Then
            If IsLocked Then
                player.Inventory.ItemsOfType(RouteType.UnlockItem.Value).First.Destroy()
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
