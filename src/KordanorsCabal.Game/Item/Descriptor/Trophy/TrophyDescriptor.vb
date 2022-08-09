Friend Class TrophyDescriptor
    Inherits ItemTypeDescriptor
    Sub New(
           name As String,
           Optional offer As Long = 0,
           Optional boughtAt As IReadOnlyList(Of ShoppeType) = Nothing,
           Optional price As Long = 0,
           Optional soldAt As IReadOnlyList(Of ShoppeType) = Nothing,
           Optional repairPrice As Long = 0,
           Optional repairedAt As IReadOnlyList(Of ShoppeType) = Nothing,
           Optional purify As Action(Of Item) = Nothing,
           Optional canUse As Func(Of Character, Boolean) = Nothing)
        MyBase.New(name, , , , , , , , , , , offer, boughtAt, price, soldAt, repairPrice, repairedAt, purify, canUse)
    End Sub
End Class
