Friend Class CopperKeyDescriptor
    Inherits ItemTypeDescriptor

    Sub New()
        MyBase.New(
            ItemType.CopperKey,
            "CU Key",,
            MakeDictionary(
                (1L, MakeHashSet(OldLocationType.DungeonDeadEnd))))
    End Sub
End Class
