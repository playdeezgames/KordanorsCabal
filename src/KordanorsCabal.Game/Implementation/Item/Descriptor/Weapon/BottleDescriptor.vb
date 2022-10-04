Friend Class BottleDescriptor
    Inherits ItemType

    Sub New()
        MyBase.New(
            StaticWorldData.World,
            OldItemType.Bottle)
    End Sub
End Class
