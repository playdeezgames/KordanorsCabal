Friend Class CopperKeyDescriptor
    Inherits ItemTypeDescriptor

    Sub New()
        MyBase.New(
            ItemType.CopperKey,
            "CU Key",,
            MakeDictionary(
                (OldDungeonLevel.Level1, MakeHashSet(LocationType.DungeonDeadEnd))))
    End Sub
End Class
