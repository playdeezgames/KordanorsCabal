Public Interface IRouteType
    Inherits IBaseThingie
    ReadOnly Property Abbreviation As String
    ReadOnly Property UnlockItem As Long?
    ReadOnly Property UnlockedRouteType As OldRouteType?
    ReadOnly Property IsSingleUse As Boolean
End Interface
