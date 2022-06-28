Public MustInherit Class RouteTypeDescriptor
    Overridable ReadOnly Property Abbreviation As String
        Get
            Return "  "
        End Get
    End Property

    Overridable ReadOnly Property UnlockItem As ItemType?
        Get
            Return Nothing
        End Get
    End Property

    Overridable ReadOnly Property UnlockedRouteType As RouteType?
        Get
            Return Nothing
        End Get
    End Property
End Class
Friend Module RouteTypeDescriptorUtility
    Friend ReadOnly RouteTypeDescriptors As IReadOnlyDictionary(Of RouteType, RouteTypeDescriptor) =
        New Dictionary(Of RouteType, RouteTypeDescriptor) From
        {
            {RouteType.CopperLock, New CopperLockDescriptor},
            {RouteType.FinalLock, New FinalLockDescriptor},
            {RouteType.GoldLock, New GoldLockDescriptor},
            {RouteType.IronLock, New IronLockDescriptor},
            {RouteType.Passageway, New PassagewayDescriptor},
            {RouteType.PlatinumLock, New PlatinumLockDescriptor},
            {RouteType.Road, New RoadDescriptor},
            {RouteType.SilverLock, New SilverLockDescriptor},
            {RouteType.Stairs, New StairsDescriptor}
        }
End Module
