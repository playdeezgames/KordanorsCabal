Friend Class IronKeyDescriptor
    Inherits ItemTypeDescriptor

    Sub New()
        MyBase.New(
            ItemType.IronKey,
            "FE Key",,
            MakeDictionary(
                (1L, MakeHashSet(OldLocationType.Dungeon)),
                (2L, MakeHashSet(OldLocationType.Dungeon)),
                (3L, MakeHashSet(OldLocationType.Dungeon)),
                (4L, MakeHashSet(OldLocationType.Dungeon)),
                (5L, MakeHashSet(OldLocationType.Dungeon))))
    End Sub
End Class
