Friend Class PlatinumKeyDescriptor
    Inherits ItemTypeDescriptor

    Sub New()
        MyBase.New(
            ItemType.PlatinumKey,
            "PT Key",,
            MakeDictionary(
                (OldDungeonLevel.Level4, MakeHashSet(LocationType.DungeonDeadEnd))))
    End Sub
End Class
