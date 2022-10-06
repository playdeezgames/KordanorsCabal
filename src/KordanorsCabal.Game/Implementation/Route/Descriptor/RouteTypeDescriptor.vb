Public Class RouteTypeDescriptor
    Sub New(
           Optional abbreviation As String = "  ",
           Optional unlockItem As Long? = Nothing,
           Optional unlockedRouteType As RouteType? = Nothing,
           Optional isSingleUse As Boolean = False)
        Me.Abbreviation = abbreviation
        Me.UnlockItem = unlockItem
        Me.UnlockedRouteType = unlockedRouteType
        Me.IsSingleUse = isSingleUse
    End Sub
    ReadOnly Property Abbreviation As String
    ReadOnly Property UnlockItem As Long?
    ReadOnly Property UnlockedRouteType As RouteType?
    ReadOnly Property IsSingleUse As Boolean
End Class
Friend Module RouteTypeDescriptorUtility
    Friend ReadOnly RouteTypeDescriptors As IReadOnlyDictionary(Of RouteType, RouteTypeDescriptor) =
        New Dictionary(Of RouteType, RouteTypeDescriptor) From
        {
            {RouteType.CopperLock, New RouteTypeDescriptor("CU", OldItemType.CopperKey, RouteType.Passageway, False)},
            {RouteType.FinalLock, New RouteTypeDescriptor("EO", 6L, RouteType.Passageway, False)},
            {RouteType.GoldLock, New RouteTypeDescriptor("AU", OldItemType.GoldKey, RouteType.Passageway, False)},
            {RouteType.IronLock, New RouteTypeDescriptor("FE", OldItemType.IronKey, RouteType.Passageway, False)},
            {RouteType.MoonPath, New RouteTypeDescriptor},
            {RouteType.Passageway, New RouteTypeDescriptor},
            {RouteType.PlatinumLock, New RouteTypeDescriptor("PT", OldItemType.PlatinumKey, RouteType.Passageway, False)},
            {RouteType.Portal, New RouteTypeDescriptor(,,, True)},
            {RouteType.Road, New RouteTypeDescriptor},
            {RouteType.SilverLock, New RouteTypeDescriptor("AG", OldItemType.SilverKey, RouteType.Passageway, False)},
            {RouteType.Stairs, New RouteTypeDescriptor}
        }
End Module
