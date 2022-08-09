Friend Class SilverKeyDescriptor
    Inherits ItemTypeDescriptor

    Sub New()
        MyBase.New(
            ItemType.SilverKey,
            "AG KEY",,
            MakeDictionary(
                (DungeonLevel.Level2, MakeHashSet(LocationType.DungeonDeadEnd))))
    End Sub
End Class
