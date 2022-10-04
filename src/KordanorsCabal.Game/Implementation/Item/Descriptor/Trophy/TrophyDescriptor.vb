Friend Class TrophyDescriptor
    Inherits ItemType
    Sub New(
           itemTypeId As Long)
        MyBase.New(StaticWorldData.World, itemTypeId)
    End Sub
End Class
