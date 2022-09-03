Friend Class TrophyDescriptor
    Inherits ItemTypeDescriptor
    Sub New(
           itemTypeId As Long,
           Optional offer As Long = 0,
           Optional boughtAt As IReadOnlyList(Of ShoppeType) = Nothing,
           Optional price As Long = 0,
           Optional soldAt As IReadOnlyList(Of ShoppeType) = Nothing,
           Optional repairPrice As Long = 0,
           Optional repairedAt As IReadOnlyList(Of ShoppeType) = Nothing,
           Optional purifyActionName As String = Nothing,
           Optional canUse As Func(Of Character, Boolean) = Nothing,
           Optional useActionName As String = Nothing)
        MyBase.New(StaticWorldData.World, itemTypeId, , , , , , , offer, boughtAt, price, soldAt, repairPrice, repairedAt, purifyActionName, canUse, useActionName)
    End Sub
End Class
