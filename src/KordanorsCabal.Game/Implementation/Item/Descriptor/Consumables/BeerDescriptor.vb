Friend Class BeerDescriptor
    Inherits ItemType

    Sub New()
        MyBase.New(
            StaticWorldData.World,
            OldItemType.Beer,,,
            "CanUseBeer",
            "UseBeer")
    End Sub
End Class
