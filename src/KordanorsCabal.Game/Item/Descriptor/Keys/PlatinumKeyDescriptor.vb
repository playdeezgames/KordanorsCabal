Friend Class PlatinumKeyDescriptor
    Inherits ItemTypeDescriptor

    Sub New()
        MyBase.New(
            ItemType.PlatinumKey,
            "PT Key",,
            MakeDictionary(
                (4L, MakeHashSet(OldLocationType.DungeonDeadEnd))))
    End Sub
End Class
