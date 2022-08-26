Friend Class PlatinumKeyDescriptor
    Inherits ItemTypeDescriptor

    Sub New()
        MyBase.New(
            StaticWorldData.World,
            ItemType.PlatinumKey,
            "PT Key",,
            MakeDictionary(
                (4L, MakeHashSet(LocationType.FromId(StaticWorldData.World, 5L)))))
    End Sub
End Class
