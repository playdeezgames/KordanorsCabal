Friend Class CopperKeyDescriptor
    Inherits ItemTypeDescriptor

    Sub New()
        MyBase.New(
            ItemType.CopperKey,
            "CU Key",,
            MakeDictionary(
                (1L, MakeHashSet(LocationType.FromId(5L)))))
    End Sub
End Class
