Friend Class GoldKeyDescriptor
    Inherits ItemTypeDescriptor

    Sub New()
        MyBase.New(
            ItemType.GoldKey,
            "AU Key",,
            MakeDictionary(
                (3L, MakeHashSet(OldLocationType.DungeonDeadEnd))))
    End Sub
End Class
