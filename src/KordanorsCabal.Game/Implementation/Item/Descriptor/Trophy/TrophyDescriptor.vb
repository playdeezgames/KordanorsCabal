Friend Class TrophyDescriptor
    Inherits ItemType
    Sub New(
           itemTypeId As Long,
           Optional purifyActionName As String = Nothing,
           Optional canUseFunctionName As String = Nothing,
           Optional useActionName As String = Nothing)
        MyBase.New(StaticWorldData.World, itemTypeId, purifyActionName, canUseFunctionName, useActionName)
    End Sub
End Class
