Friend Class IronKeyDescriptor
    Inherits ItemTypeDescriptor

    Sub New()
        MyBase.New(
            ItemType.IronKey,
            "FE Key",,
            MakeDictionary(
                (1L, MakeHashSet(LocationType.FromId(Dungeon))),
                (2L, MakeHashSet(LocationType.FromId(Dungeon))),
                (3L, MakeHashSet(LocationType.FromId(Dungeon))),
                (4L, MakeHashSet(LocationType.FromId(Dungeon))),
                (5L, MakeHashSet(LocationType.FromId(Dungeon)))))
    End Sub
End Class
