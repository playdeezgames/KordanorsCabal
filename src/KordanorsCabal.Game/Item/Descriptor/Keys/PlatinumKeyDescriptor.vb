Friend Class PlatinumKeyDescriptor
    Inherits ItemTypeDescriptor

    Sub New()
        MyBase.New(
            ItemType.PlatinumKey,
            "PT Key",,
            MakeDictionary(
                (DungeonLevel.Level4, MakeHashSet(LocationType.DungeonDeadEnd))))
    End Sub
End Class
