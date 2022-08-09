Friend Class GoldKeyDescriptor
    Inherits ItemTypeDescriptor

    Sub New()
        MyBase.New(
            ItemType.GoldKey,
            "AU Key",,
            MakeDictionary(
                (DungeonLevel.Level3, MakeHashSet(LocationType.DungeonDeadEnd))))
    End Sub
End Class
