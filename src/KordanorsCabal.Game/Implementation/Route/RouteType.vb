Public Class RouteType
    Inherits BaseThingie
    Implements IRouteType
    Sub New(
           worldData As IWorldData,
           id As Long,
           Optional unlockItem As Long? = Nothing,
           Optional unlockedRouteType As OldRouteType? = Nothing)
        MyBase.New(worldData, id)
        Me.UnlockItem = unlockItem
        Me.UnlockedRouteType = unlockedRouteType
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
    ReadOnly Property UnlockedRouteType As OldRouteType? Implements IRouteType.UnlockedRouteType
    ReadOnly Property IsSingleUse As Boolean Implements IRouteType.IsSingleUse
        Get
            Return WorldData.RouteType.ReadIsSingleUse(Id)
        End Get
    End Property
End Class
Friend Module RouteTypeDescriptorUtility
    ReadOnly Property RouteTypeDescriptors(worldData As IWorldData) As IReadOnlyDictionary(Of OldRouteType, RouteType)
        Get
            '(1,'  ',0)
            '(2,'  ',0)
            '(3,'  ',0)
            '(4,'FE',0)
            '(5,'CU',0)
            '(6,'AG',0)
            '(7,'AU',0)
            '(8,'PT',0)
            '(9,'EO',0)
            '(10,'  ',1)
            '(11,'  ',0)
            Return New Dictionary(Of OldRouteType, RouteType) From
            {
                {OldRouteType.CopperLock, New RouteType(worldData, OldRouteType.CopperLock, 2L, OldRouteType.Passageway)},
                {OldRouteType.FinalLock, New RouteType(worldData, OldRouteType.FinalLock, 6L, OldRouteType.Passageway)},
                {OldRouteType.GoldLock, New RouteType(worldData, OldRouteType.GoldLock, 4L, OldRouteType.Passageway)},
                {OldRouteType.IronLock, New RouteType(worldData, OldRouteType.IronLock, 1L, OldRouteType.Passageway)},
                {OldRouteType.MoonPath, New RouteType(worldData, OldRouteType.MoonPath)},
                {OldRouteType.Passageway, New RouteType(worldData, OldRouteType.Passageway)},
                {OldRouteType.PlatinumLock, New RouteType(worldData, OldRouteType.PlatinumLock, 5L, OldRouteType.Passageway)},
                {OldRouteType.Portal, New RouteType(worldData, OldRouteType.Portal)},
                {OldRouteType.Road, New RouteType(worldData, OldRouteType.Road)},
                {OldRouteType.SilverLock, New RouteType(worldData, OldRouteType.SilverLock, 3L, OldRouteType.Passageway)},
                {OldRouteType.Stairs, New RouteType(worldData, OldRouteType.Stairs)}
            }
        End Get
    End Property
End Module
