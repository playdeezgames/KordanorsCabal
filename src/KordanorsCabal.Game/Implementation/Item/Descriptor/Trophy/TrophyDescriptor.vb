Friend Class TrophyDescriptor
    Inherits ItemType
    Sub New(
           itemTypeId As Long,
           Optional purifyActionName As String = Nothing)
        MyBase.New(StaticWorldData.World, itemTypeId, purifyActionName)
    End Sub
End Class
