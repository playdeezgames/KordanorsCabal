Friend Class IronKeyDescriptor
    Inherits ItemTypeDescriptor

    Sub New()
        MyBase.New(
            StaticWorldData.World,
            ItemType.IronKey,
            "FE Key",,
            MakeDictionary(
                (1L, MakeHashSet(LocationType.FromId(StaticWorldData.World, 4L))),
                (2L, MakeHashSet(LocationType.FromId(StaticWorldData.World, 4L))),
                (3L, MakeHashSet(LocationType.FromId(StaticWorldData.World, 4L))),
                (4L, MakeHashSet(LocationType.FromId(StaticWorldData.World, 4L))),
                (5L, MakeHashSet(LocationType.FromId(StaticWorldData.World, 4L)))))
    End Sub
End Class
