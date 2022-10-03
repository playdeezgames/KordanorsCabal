Friend Class TrophyDescriptor
    Inherits ItemType
    Sub New(
           itemTypeId As Long,
           Optional purifyActionName As String = Nothing,
           Optional canUseFunctionName As String = Nothing)
        MyBase.New(StaticWorldData.World, itemTypeId, purifyActionName, canUseFunctionName)
    End Sub
End Class
