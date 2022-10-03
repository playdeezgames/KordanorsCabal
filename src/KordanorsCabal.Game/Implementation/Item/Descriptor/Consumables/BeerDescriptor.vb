Friend Class BeerDescriptor
    Inherits ItemType

    Sub New()
        MyBase.New(
            StaticWorldData.World,
            OldItemType.Beer,,
            "CanUseBeer")
    End Sub
End Class
