Friend Class IronKeyDescriptor
    Inherits ItemTypeDescriptor

    Sub New()
        MyBase.New(
            ItemType.IronKey,
            "FE Key",,
            MakeDictionary(
                (1L, MakeHashSet(LocationType.FromName(Dungeon))),
                (2L, MakeHashSet(LocationType.FromName(Dungeon))),
                (3L, MakeHashSet(LocationType.FromName(Dungeon))),
                (4L, MakeHashSet(LocationType.FromName(Dungeon))),
                (5L, MakeHashSet(LocationType.FromName(Dungeon)))))
    End Sub
End Class
