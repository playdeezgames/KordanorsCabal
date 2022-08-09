Friend Class CopperKeyDescriptor
    Inherits ItemTypeDescriptor

    Sub New()
        MyBase.New(
            ItemType.CopperKey,
            "CU Key",,
            MakeDictionary(
                (DungeonLevel.Level1, MakeHashSet(LocationType.DungeonDeadEnd))))
    End Sub
End Class
