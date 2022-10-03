Friend Class PlateMailDescriptor
    Inherits ItemType
    Sub New()
        MyBase.New(
            StaticWorldData.World,
            OldItemType.PlateMail,
            MakeList(ShoppeType.Blacksmith))
    End Sub
End Class
