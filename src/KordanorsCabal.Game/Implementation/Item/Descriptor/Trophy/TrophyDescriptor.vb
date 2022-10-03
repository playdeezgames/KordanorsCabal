Friend Class TrophyDescriptor
    Inherits ItemType
    Sub New(
           itemTypeId As Long,
           Optional soldAt As IReadOnlyList(Of ShoppeType) = Nothing,
           Optional repairedAt As IReadOnlyList(Of ShoppeType) = Nothing,
           Optional purifyActionName As String = Nothing,
           Optional canUseFunctionName As String = Nothing,
           Optional useActionName As String = Nothing)
        MyBase.New(StaticWorldData.World, itemTypeId, soldAt, repairedAt, purifyActionName, canUseFunctionName, useActionName)
    End Sub
End Class
