Friend Class SilverKeyDescriptor
    Inherits ItemTypeDescriptor

    Sub New()
        MyBase.New(
            ItemType.SilverKey,
            "AG KEY",,
            MakeDictionary(
                (2L, MakeHashSet(LocationType.FromId(StaticWorldData.World, 5L)))))
    End Sub
End Class
