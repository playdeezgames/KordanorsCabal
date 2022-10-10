Public Class RouteType
    Sub New(
           Optional abbreviation As String = "  ",
           Optional unlockItem As Long? = Nothing,
           Optional unlockedRouteType As OldRouteType? = Nothing,
           Optional isSingleUse As Boolean = False)
        Me.Abbreviation = abbreviation
        Me.UnlockItem = unlockItem
        Me.UnlockedRouteType = unlockedRouteType
        Me.IsSingleUse = isSingleUse
    End Sub
    ReadOnly Property Abbreviation As String
    ReadOnly Property UnlockItem As Long?
    ReadOnly Property UnlockedRouteType As OldRouteType?
    ReadOnly Property IsSingleUse As Boolean
End Class
Friend Module RouteTypeDescriptorUtility
    Friend ReadOnly RouteTypeDescriptors As IReadOnlyDictionary(Of OldRouteType, RouteType) =
        New Dictionary(Of OldRouteType, RouteType) From
        {
            {OldRouteType.CopperLock, New RouteType("CU", 2L, OldRouteType.Passageway, False)},
            {OldRouteType.FinalLock, New RouteType("EO", 6L, OldRouteType.Passageway, False)},
            {OldRouteType.GoldLock, New RouteType("AU", 4L, OldRouteType.Passageway, False)},
            {OldRouteType.IronLock, New RouteType("FE", 1L, OldRouteType.Passageway, False)},
            {OldRouteType.MoonPath, New RouteType},
            {OldRouteType.Passageway, New RouteType},
            {OldRouteType.PlatinumLock, New RouteType("PT", 5L, OldRouteType.Passageway, False)},
            {OldRouteType.Portal, New RouteType(,,, True)},
            {OldRouteType.Road, New RouteType},
            {OldRouteType.SilverLock, New RouteType("AG", 3L, OldRouteType.Passageway, False)},
            {OldRouteType.Stairs, New RouteType}
        }
End Module
