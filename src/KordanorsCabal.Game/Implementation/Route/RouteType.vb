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
Friend Module RouteTypeDescriptorUtility
    ReadOnly Property RouteTypeDescriptors(worldData As IWorldData) As IReadOnlyDictionary(Of OldRouteType, RouteType)
        Get
            Return New Dictionary(Of OldRouteType, RouteType) From
            {
                {OldRouteType.CopperLock, New RouteType(worldData, OldRouteType.CopperLock)},
                {OldRouteType.FinalLock, New RouteType(worldData, OldRouteType.FinalLock)},
                {OldRouteType.GoldLock, New RouteType(worldData, OldRouteType.GoldLock)},
                {OldRouteType.IronLock, New RouteType(worldData, OldRouteType.IronLock)},
                {OldRouteType.MoonPath, New RouteType(worldData, OldRouteType.MoonPath)},
                {OldRouteType.Passageway, New RouteType(worldData, OldRouteType.Passageway)},
                {OldRouteType.PlatinumLock, New RouteType(worldData, OldRouteType.PlatinumLock)},
                {OldRouteType.Portal, New RouteType(worldData, OldRouteType.Portal)},
                {OldRouteType.Road, New RouteType(worldData, OldRouteType.Road)},
                {OldRouteType.SilverLock, New RouteType(worldData, OldRouteType.SilverLock)},
                {OldRouteType.Stairs, New RouteType(worldData, OldRouteType.Stairs)}
            }
        End Get
    End Property
End Module
