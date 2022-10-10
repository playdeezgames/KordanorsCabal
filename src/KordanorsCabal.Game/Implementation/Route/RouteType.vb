Public Class RouteType
    Inherits BaseThingie
    Implements IRouteType
    Sub New(
           worldData As IWorldData,
           id As Long,
           Optional abbreviation As String = "  ",
           Optional unlockItem As Long? = Nothing,
           Optional unlockedRouteType As OldRouteType? = Nothing,
           Optional isSingleUse As Boolean = False)
        MyBase.New(worldData, id)
        Me.Abbreviation = abbreviation
        Me.UnlockItem = unlockItem
        Me.UnlockedRouteType = unlockedRouteType
        Me.IsSingleUse = isSingleUse
    End Sub
    ReadOnly Property Abbreviation As String Implements IRouteType.Abbreviation
    ReadOnly Property UnlockItem As Long? Implements IRouteType.UnlockItem
    ReadOnly Property UnlockedRouteType As OldRouteType? Implements IRouteType.UnlockedRouteType
    ReadOnly Property IsSingleUse As Boolean Implements IRouteType.IsSingleUse
End Class
Friend Module RouteTypeDescriptorUtility
    ReadOnly Property RouteTypeDescriptors(worldData As IWorldData) As IReadOnlyDictionary(Of OldRouteType, RouteType)
        Get

            Return New Dictionary(Of OldRouteType, RouteType) From
            {
                {OldRouteType.CopperLock, New RouteType(worldData, OldRouteType.CopperLock, "CU", 2L, OldRouteType.Passageway, False)},
                {OldRouteType.FinalLock, New RouteType(worldData, OldRouteType.FinalLock, "EO", 6L, OldRouteType.Passageway, False)},
                {OldRouteType.GoldLock, New RouteType(worldData, OldRouteType.GoldLock, "AU", 4L, OldRouteType.Passageway, False)},
                {OldRouteType.IronLock, New RouteType(worldData, OldRouteType.IronLock, "FE", 1L, OldRouteType.Passageway, False)},
                {OldRouteType.MoonPath, New RouteType(worldData, OldRouteType.MoonPath)},
                {OldRouteType.Passageway, New RouteType(worldData, OldRouteType.Passageway)},
                {OldRouteType.PlatinumLock, New RouteType(worldData, OldRouteType.PlatinumLock, "PT", 5L, OldRouteType.Passageway, False)},
                {OldRouteType.Portal, New RouteType(worldData, OldRouteType.Portal, ,,, True)},
                {OldRouteType.Road, New RouteType(worldData, OldRouteType.Road)},
                {OldRouteType.SilverLock, New RouteType(worldData, OldRouteType.SilverLock, "AG", 3L, OldRouteType.Passageway, False)},
                {OldRouteType.Stairs, New RouteType(worldData, OldRouteType.Stairs)}
            }
        End Get
    End Property
End Module
