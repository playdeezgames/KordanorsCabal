Friend Class GoldKeyDescriptor
    Inherits ItemTypeDescriptor

    Sub New()
        MyBase.New(
            ItemType.GoldKey,
            "AU Key",,
            MakeDictionary(
                (3L, MakeHashSet(LocationType.FromName(DungeonDeadEnd)))))
    End Sub
End Class
