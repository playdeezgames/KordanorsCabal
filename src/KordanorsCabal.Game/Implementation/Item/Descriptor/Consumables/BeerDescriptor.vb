Friend Class BeerDescriptor
    Inherits ItemType

    Sub New()
        MyBase.New(
            StaticWorldData.World,
            OldItemType.Beer)
    End Sub
End Class
