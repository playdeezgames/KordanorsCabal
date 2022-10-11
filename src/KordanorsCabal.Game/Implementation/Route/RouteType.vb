Public Class RouteType
    Inherits BaseThingie
    Implements IRouteType
    Sub New(
           worldData As IWorldData,
           id As Long)
        MyBase.New(worldData, id)
    End Sub
    Shared Function FromId(worldData As IWorldData, id As Long?) As IRouteType
        Return If(id.HasValue, New RouteType(worldData, id.Value), Nothing)
    End Function
    ReadOnly Property Abbreviation As String Implements IRouteType.Abbreviation
        Get
            Return WorldData.RouteType.ReadAbbreviation(Id)
        End Get
    End Property
    ReadOnly Property UnlockItem As Long? Implements IRouteType.UnlockItem
        Get
            Return WorldData.RouteTypeLock.ReadUnlockItem(Id)
        End Get
    End Property
    ReadOnly Property UnlockedRouteType As IRouteType Implements IRouteType.UnlockedRouteType
        Get
            Return RouteType.FromId(WorldData, WorldData.RouteTypeLock.ReadUnlockedRouteType(Id))
        End Get
    End Property
    ReadOnly Property IsSingleUse As Boolean Implements IRouteType.IsSingleUse
        Get
            Return WorldData.RouteType.ReadIsSingleUse(Id)
        End Get
    End Property
End Class
